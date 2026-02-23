# E2E Testing Implementation Plan

**Document Version:** 1.0  
**Date:** February 20, 2026  
**Status:** Ready for Implementation

---

## Table of Contents

1. [Overview](#overview)
2. [Current State Analysis](#current-state-analysis)
3. [Phase 1: Playwright Setup](#phase-1-playwright-setup)
4. [Phase 2: Automation Scripts](#phase-2-automation-scripts)
5. [Phase 3: Test Infrastructure](#phase-3-test-infrastructure)
6. [Phase 4: Add data-testid Attributes](#phase-4-add-data-testid-attributes)
7. [Phase 5: Implement E2E Test Specs](#phase-5-implement-e2e-test-specs)
8. [Phase 6: Scripts and Documentation](#phase-6-scripts-and-documentation)
9. [Implementation Checklist](#implementation-checklist)
10. [Troubleshooting Guide](#troubleshooting-guide)

---

## Overview

This document outlines the complete implementation plan for end-to-end (E2E) testing using Playwright for the HomeEconomics Next.js frontend. The E2E tests will run against:

- **Backend:** .NET API on `http://localhost:5000` with `homeeconomics-test` database
- **Frontend:** Next.js dev server on `http://localhost:3000`
- **Execution:** Sequential (single worker) for stability
- **Scope:** Local execution only (no CI integration for now)
- **Automation:** Automatic backend/frontend/database setup before tests

### Design Principles

✅ **Real backend** (not mocked) - validates actual API contracts  
✅ **Test data created via API** - fast, reliable, no UI dependency  
✅ **Cleanup after each test** - isolation prevents flaky tests  
✅ **Use `data-testid` attributes** - more reliable than CSS selectors  
✅ **Spanish copy assertions** - validates user-facing text  
✅ **Screenshot on failure** - easier debugging  

---

## Current State Analysis

### Existing Setup

- ✅ Vitest + RTL already configured for unit/component/integration tests
- ✅ Backend runs on `http://localhost:5000` (HomeEconomics profile)
- ✅ PostgreSQL database: `homeeconomics-dev` (for development)
- ✅ EF Core migrations auto-run on backend startup in development mode
- ✅ Frontend proxies `/api/*` requests to backend via Next.js rewrites

### Missing Components

- ❌ Playwright not yet installed
- ❌ No E2E test files or configuration
- ❌ No `homeeconomics-test` database configuration
- ❌ No automation scripts for test environment setup
- ❌ No `data-testid` attributes in components

---

## Phase 1: Playwright Setup

### 1.1 Install Dependencies

**Commands:**
```bash
cd src/HomeEconomics/frontend
npm install -D @playwright/test
npx playwright install chromium
```

**Expected output:**
- `@playwright/test` added to `devDependencies`
- Chromium browser binaries downloaded

---

### 1.2 Create Playwright Configuration

**File:** `src/HomeEconomics/frontend/playwright.config.ts`

```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e/specs',
  fullyParallel: false,  // Sequential execution
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,  // Single worker for sequential tests
  reporter: 'html',
  timeout: 30000,  // 30 seconds per test
  
  use: {
    baseURL: 'http://localhost:3000',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  // Global setup/teardown
  globalSetup: require.resolve('./e2e/global-setup.ts'),
  globalTeardown: require.resolve('./e2e/global-teardown.ts'),

  // Wait for frontend dev server before running tests
  webServer: [
    {
      command: 'npm run dev',
      url: 'http://localhost:3000',
      reuseExistingServer: !process.env.CI,
      timeout: 120000,
    },
  ],
});
```

**Key configuration decisions:**
- `fullyParallel: false` and `workers: 1` ensure sequential execution
- `timeout: 30000` allows time for API calls + UI operations
- `retries: 0` for predictable test behavior (no flakiness masking)
- Screenshots and videos only on failure to save disk space
- `webServer` config auto-starts Next.js dev server

---

### 1.3 Create Test Database Configuration

**File:** `src/HomeEconomics/appsettings.Test.json`

```json
{
  "ConnectionStrings": {
    "HomeEconomics": "Host=localhost;Port=5432;Database=homeeconomics-test;Username=homeeconomics;Password=homeeconomics"
  }
}
```

**Note:** The backend will auto-create and migrate this database on startup when `ASPNETCORE_ENVIRONMENT=Test` is set.

**Verification:**
After first E2E run, verify database exists:
```bash
psql -U homeeconomics -l | grep homeeconomics-test
```

---

## Phase 2: Automation Scripts

### 2.1 Create Backend Startup Script

**File:** `src/HomeEconomics/frontend/scripts/start-backend.sh`

```bash
#!/bin/bash
set -e

echo "Starting backend with test database..."
cd ../..
export ASPNETCORE_ENVIRONMENT=Test
dotnet run --project src/HomeEconomics --urls "http://localhost:5000" > /dev/null 2>&1 &
BACKEND_PID=$!
echo $BACKEND_PID > /tmp/homeeconomics-backend.pid

# Wait for backend to be ready
echo "Waiting for backend to be ready..."
for i in {1..30}; do
  if curl -s http://localhost:5000/health > /dev/null 2>&1; then
    echo "Backend is ready!"
    exit 0
  fi
  sleep 1
done

echo "Backend failed to start"
kill $BACKEND_PID 2>/dev/null || true
exit 1
```

**Make executable:**
```bash
chmod +x src/HomeEconomics/frontend/scripts/start-backend.sh
```

**How it works:**
1. Starts .NET backend with `ASPNETCORE_ENVIRONMENT=Test`
2. Backend auto-creates `homeeconomics-test` database
3. Backend auto-runs EF Core migrations
4. Saves backend PID to `/tmp/homeeconomics-backend.pid` for cleanup
5. Polls health endpoint for 30 seconds
6. Exits with success when backend is ready

---

### 2.2 Create Global Setup

**File:** `src/HomeEconomics/frontend/e2e/global-setup.ts`

```typescript
import { chromium, FullConfig } from '@playwright/test';
import { execSync } from 'child_process';

async function globalSetup(config: FullConfig) {
  console.log('🚀 Starting backend server...');
  
  // Start backend (which will auto-migrate the test database)
  execSync('bash ./scripts/start-backend.sh', { 
    cwd: __dirname + '/..',
    stdio: 'inherit'
  });
  
  // Wait for backend health check
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  let retries = 30;
  while (retries > 0) {
    try {
      const response = await page.goto('http://localhost:5000/health');
      if (response?.ok()) {
        console.log('✅ Backend is ready');
        break;
      }
    } catch (e) {
      // Continue waiting
    }
    retries--;
    await page.waitForTimeout(1000);
  }
  
  await browser.close();
  
  if (retries === 0) {
    throw new Error('Backend failed to start');
  }
}

export default globalSetup;
```

**Execution flow:**
1. Runs before any tests
2. Calls `start-backend.sh` script
3. Verifies backend health endpoint responds
4. Throws error if backend doesn't start within 30 seconds

---

### 2.3 Create Global Teardown

**File:** `src/HomeEconomics/frontend/e2e/global-teardown.ts`

```typescript
import { execSync } from 'child_process';
import fs from 'fs';

async function globalTeardown() {
  console.log('🛑 Stopping backend server...');
  
  try {
    const pidFile = '/tmp/homeeconomics-backend.pid';
    if (fs.existsSync(pidFile)) {
      const pid = fs.readFileSync(pidFile, 'utf8').trim();
      execSync(`kill ${pid}`, { stdio: 'ignore' });
      fs.unlinkSync(pidFile);
      console.log('✅ Backend stopped');
    }
  } catch (e) {
    console.warn('⚠️  Could not stop backend:', e);
  }
}

export default globalTeardown;
```

**Execution flow:**
1. Runs after all tests complete
2. Reads backend PID from temp file
3. Kills backend process
4. Cleans up PID file

---

## Phase 3: Test Infrastructure

### 3.1 API Client for Test Data Setup

**File:** `src/HomeEconomics/frontend/e2e/utils/api-client.ts`

```typescript
const API_BASE = 'http://localhost:5000/api';

export class TestApiClient {
  // Movements
  async createMovement(data: {
    name: string;
    amount: number;
    type: 'income' | 'expense';
    frequency: 'monthly' | 'bimonthly' | 'quarterly';
  }): Promise<{ id: number }> {
    const response = await fetch(`${API_BASE}/movements`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    if (!response.ok) throw new Error(`Create movement failed: ${response.status}`);
    return response.json();
  }

  async deleteMovement(id: number): Promise<void> {
    const response = await fetch(`${API_BASE}/movements/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok && response.status !== 404) {
      throw new Error(`Delete movement failed: ${response.status}`);
    }
  }

  // Movement Months
  async createMovementMonth(year: number, month: number): Promise<{ id: number }> {
    const response = await fetch(`${API_BASE}/movement-months`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ year, month }),
    });
    if (!response.ok) throw new Error(`Create month failed: ${response.status}`);
    return response.json();
  }

  async getMovementMonth(year: number, month: number): Promise<any> {
    const response = await fetch(`${API_BASE}/movement-months/${year}/${month}`);
    if (!response.ok && response.status !== 404) {
      throw new Error(`Get month failed: ${response.status}`);
    }
    return response.status === 404 ? null : response.json();
  }

  // Month Movements
  async addMonthMovement(monthId: number, data: {
    movementId: number;
    amount: number;
  }): Promise<{ id: number }> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements`,
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      }
    );
    if (!response.ok) throw new Error(`Add month movement failed: ${response.status}`);
    return response.json();
  }

  async deleteMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements/${movementId}`,
      { method: 'DELETE' }
    );
    if (!response.ok && response.status !== 404) {
      throw new Error(`Delete month movement failed: ${response.status}`);
    }
  }

  async payMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/pay`,
      { method: 'POST' }
    );
    if (!response.ok) throw new Error(`Pay failed: ${response.status}`);
  }

  async unpayMonthMovement(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/unpay`,
      { method: 'POST' }
    );
    if (!response.ok) throw new Error(`Unpay failed: ${response.status}`);
  }

  async updateMonthMovementAmount(
    monthId: number,
    movementId: number,
    amount: number
  ): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/update-amount`,
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount }),
      }
    );
    if (!response.ok) throw new Error(`Update amount failed: ${response.status}`);
  }

  async moveToNextMonth(monthId: number, movementId: number): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/month-movements/${movementId}/to-next-movement-month`,
      { method: 'POST' }
    );
    if (!response.ok) throw new Error(`Move to next month failed: ${response.status}`);
  }

  // Status
  async addStatus(monthId: number, account: number, cash: number): Promise<void> {
    const response = await fetch(
      `${API_BASE}/movement-months/${monthId}/add-status`,
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ account, cash }),
      }
    );
    if (!response.ok) throw new Error(`Add status failed: ${response.status}`);
  }
}

export const apiClient = new TestApiClient();
```

**Usage in tests:**
```typescript
const { id } = await apiClient.createMovement({
  name: 'Test Rent',
  amount: 1500,
  type: 'expense',
  frequency: 'monthly'
});
```

---

### 3.2 Test Fixtures and Helpers

**File:** `src/HomeEconomics/frontend/e2e/fixtures/test-data.ts`

```typescript
let testCounter = 0;

export function generateUniqueTestName(prefix: string): string {
  return `${prefix} ${Date.now()}-${testCounter++}`;
}

export const testMovementDefaults = {
  income: {
    amount: 3000,
    type: 'income' as const,
    frequency: 'monthly' as const,
  },
  expense: {
    amount: 1200,
    type: 'expense' as const,
    frequency: 'monthly' as const,
  },
};

export function getCurrentYearMonth(): { year: number; month: number } {
  const now = new Date();
  return {
    year: now.getFullYear(),
    month: now.getMonth() + 1,
  };
}

export function getNextYearMonth(): { year: number; month: number } {
  const now = new Date();
  const next = new Date(now.getFullYear(), now.getMonth() + 1, 1);
  return {
    year: next.getFullYear(),
    month: next.getMonth() + 1,
  };
}
```

**Usage in tests:**
```typescript
const movementName = generateUniqueTestName('E2E Rent');
const { year, month } = getCurrentYearMonth();
```

---

### 3.3 Page Selectors

**File:** `src/HomeEconomics/frontend/e2e/utils/selectors.ts`

```typescript
export const selectors = {
  // Movement form
  movementForm: {
    nameInput: '[data-testid="movement-form-name"]',
    amountInput: '[data-testid="movement-form-amount"]',
    typeSelect: '[data-testid="movement-form-type"]',
    frequencySelect: '[data-testid="movement-form-frequency"]',
    submitButton: '[data-testid="movement-form-submit"]',
    cancelButton: '[data-testid="movement-form-cancel"]',
  },

  // Movements list
  movementsList: {
    container: '[data-testid="movements-list"]',
    item: (name: string) => `[data-testid="movement-item-${name}"]`,
    editButton: (name: string) => `[data-testid="movement-edit-${name}"]`,
    deleteButton: (name: string) => `[data-testid="movement-delete-${name}"]`,
    addToMonthButton: (name: string) => `[data-testid="movement-add-to-month-${name}"]`,
  },

  // Delete confirmation dialog
  deleteDialog: {
    container: '[data-testid="confirm-delete-movement-dialog"]',
    confirmButton: '[data-testid="delete-movement-confirm"]',
    cancelButton: '[data-testid="delete-movement-cancel"]',
    errorMessage: '[data-testid="delete-movement-error"]',
  },

  // Month selector
  monthSelector: {
    currentMonthButton: '[data-testid="month-selector-current"]',
    nextMonthButton: '[data-testid="month-selector-next"]',
    createCurrentMonthButton: '[data-testid="create-current-month"]',
    createNextMonthButton: '[data-testid="create-next-month"]',
  },

  // Current month movements list
  currentMonthList: {
    container: '[data-testid="current-month-movements-list"]',
    item: (name: string) => `[data-testid="month-movement-${name}"]`,
    paidIndicator: (name: string) => `[data-testid="month-movement-paid-${name}"]`,
    payButton: (name: string) => `[data-testid="month-movement-pay-${name}"]`,
    unpayButton: (name: string) => `[data-testid="month-movement-unpay-${name}"]`,
    editAmountButton: (name: string) => `[data-testid="month-movement-edit-amount-${name}"]`,
    deleteButton: (name: string) => `[data-testid="month-movement-delete-${name}"]`,
    moveButton: (name: string) => `[data-testid="month-movement-move-${name}"]`,
  },

  // Paid toggle
  paidToggle: '[data-testid="paid-toggle"]',

  // Edit amount dialog
  editAmountDialog: {
    container: '[data-testid="edit-amount-dialog"]',
    amountInput: '[data-testid="edit-amount-input"]',
    saveButton: '[data-testid="edit-amount-save"]',
    cancelButton: '[data-testid="edit-amount-cancel"]',
  },

  // Status form
  statusForm: {
    accountInput: '[data-testid="status-account-input"]',
    cashInput: '[data-testid="status-cash-input"]',
    balanceDisplay: '[data-testid="status-balance"]',
    successMessage: '[data-testid="status-success"]',
  },
};
```

**Usage in tests:**
```typescript
await page.click(selectors.movementForm.submitButton);
await page.fill(selectors.statusForm.accountInput, '5000');
```

---

## Phase 4: Add data-testid Attributes

### 4.1 Movement Form Component

**File:** `src/HomeEconomics/frontend/components/movement-form.tsx`

**Attributes to add:**
- Name TextField: `data-testid="movement-form-name"`
- Amount TextField: `data-testid="movement-form-amount"`
- Type Select: `data-testid="movement-form-type"`
- Frequency Select: `data-testid="movement-form-frequency"`
- Submit Button: `data-testid="movement-form-submit"`
- Cancel Button: `data-testid="movement-form-cancel"`

**Example:**
```tsx
<TextField
  label="Nombre"
  data-testid="movement-form-name"
  // ... other props
/>
```

---

### 4.2 Movements List Component

**File:** `src/HomeEconomics/frontend/components/movements-list.tsx`

**Attributes to add:**
- Container Box: `data-testid="movements-list"`
- Each ListItem: `data-testid="movement-item-${movement.name}"`
- Edit IconButton: `data-testid="movement-edit-${movement.name}"`
- Delete IconButton: `data-testid="movement-delete-${movement.name}"`
- Add to Month Button: `data-testid="movement-add-to-month-${movement.name}"`

**Example:**
```tsx
<Box data-testid="movements-list">
  {movements.map((movement) => (
    <ListItem key={movement.id} data-testid={`movement-item-${movement.name}`}>
      <IconButton data-testid={`movement-edit-${movement.name}`}>
        <EditIcon />
      </IconButton>
    </ListItem>
  ))}
</Box>
```

---

### 4.3 Delete Confirmation Dialog

**File:** `src/HomeEconomics/frontend/components/confirm-delete-movement-dialog.tsx`

**Attributes to add:**
- Dialog: `data-testid="confirm-delete-movement-dialog"`
- Confirm Button: `data-testid="delete-movement-confirm"`
- Cancel Button: `data-testid="delete-movement-cancel"`
- Error Alert: `data-testid="delete-movement-error"`

---

### 4.4 Month Selector

**File:** `src/HomeEconomics/frontend/components/month-movement-month-selector.tsx`

**Attributes to add:**
- Current Month ToggleButton: `data-testid="month-selector-current"`
- Next Month ToggleButton: `data-testid="month-selector-next"`
- Create Current Month Button: `data-testid="create-current-month"`
- Create Next Month Button: `data-testid="create-next-month"`

---

### 4.5 Current Month Movements List

**File:** `src/HomeEconomics/frontend/components/current-month-movements-list.tsx`

**Attributes to add:**
- Container: `data-testid="current-month-movements-list"`
- Each ListItem: `data-testid="month-movement-${movement.name}"`
- Paid Chip: `data-testid="month-movement-paid-${movement.name}"`
- Pay Button: `data-testid="month-movement-pay-${movement.name}"`
- Unpay Button: `data-testid="month-movement-unpay-${movement.name}"`
- Edit Amount Button: `data-testid="month-movement-edit-amount-${movement.name}"`
- Delete Button: `data-testid="month-movement-delete-${movement.name}"`
- Move Button: `data-testid="month-movement-move-${movement.name}"`

---

### 4.6 Paid Toggle

**File:** `src/HomeEconomics/frontend/app/page.tsx`

**Attribute to add:**
- Paid FormControlLabel Switch: `data-testid="paid-toggle"`

**Example:**
```tsx
<FormControlLabel
  control={<Switch data-testid="paid-toggle" />}
  label="Mostrar solo pendientes"
/>
```

---

### 4.7 Edit Amount Dialog

**File:** `src/HomeEconomics/frontend/components/edit-month-movement-amount-dialog.tsx`

**Attributes to add:**
- Dialog: `data-testid="edit-amount-dialog"`
- Amount TextField: `data-testid="edit-amount-input"`
- Save Button: `data-testid="edit-amount-save"`
- Cancel Button: `data-testid="edit-amount-cancel"`

---

### 4.8 Status Form

**File:** `src/HomeEconomics/frontend/components/movement-month-status-form.tsx`

**Attributes to add:**
- Account TextField: `data-testid="status-account-input"`
- Cash TextField: `data-testid="status-cash-input"`
- Balance Typography: `data-testid="status-balance"`
- Success Alert: `data-testid="status-success"`

---

## Phase 5: Implement E2E Test Specs

### Test 1: Movements Lifecycle

**File:** `src/HomeEconomics/frontend/e2e/specs/movements-lifecycle.spec.ts`

**Scenario:** Create → Edit → Delete a movement

**Test flow:**
1. Navigate to home page
2. Fill movement form with test data
3. Submit and verify movement appears in list
4. Click edit button
5. Update amount and submit
6. Verify updated amount is displayed
7. Click delete button
8. Confirm deletion in dialog
9. Verify movement is removed from list

**Key assertions:**
- Movement appears after creation
- Updated amount is reflected in list
- Movement is removed after deletion
- Delete dialog shows correct movement name

---

### Test 2: Month Setup

**File:** `src/HomeEconomics/frontend/e2e/specs/month-setup.spec.ts`

**Scenario:** Create current month if missing, verify list loads

**Test flow:**
1. Navigate to home page
2. Check if "create current month" button is visible
3. If visible, click to create month
4. Verify current month movements list appears

**Key assertions:**
- Month list is visible after creation
- No errors during month creation

---

### Test 3: Add to Current Month

**File:** `src/HomeEconomics/frontend/e2e/specs/add-to-current-month.spec.ts`

**Scenario:** Add a movement to current month from movements list

**Test setup:**
1. Create movement via API
2. Create current month via API

**Test flow:**
1. Navigate to home page
2. Click "add to current month" button for test movement
3. Verify movement appears in current month movements list

**Key assertions:**
- Movement appears in current month list
- Movement shows correct name and amount

---

### Test 4: Pay/Unpay

**File:** `src/HomeEconomics/frontend/e2e/specs/pay-unpay.spec.ts`

**Scenario:** Toggle paid state and verify filter behavior

**Test setup:**
1. Create movement via API
2. Create current month via API
3. Add movement to current month via API

**Test flow:**
1. Navigate to home page
2. Verify movement is visible and unpaid
3. Click pay button
4. Verify paid indicator appears
5. Toggle "show only unpaid" filter
6. Verify paid movement is hidden
7. Toggle filter off
8. Click unpay button
9. Verify paid indicator disappears

**Key assertions:**
- Paid indicator appears after paying
- Filter correctly hides paid movements
- Unpay action removes paid indicator

---

### Test 5: Edit Amount

**File:** `src/HomeEconomics/frontend/e2e/specs/edit-amount.spec.ts`

**Scenario:** Update month movement amount via dialog

**Test setup:**
1. Create movement with 500 amount via API
2. Create current month via API
3. Add movement to month via API

**Test flow:**
1. Navigate to home page
2. Click edit amount button for movement
3. Verify edit dialog appears
4. Update amount to 550
5. Click save button
6. Verify dialog closes
7. Verify new amount (550) is displayed in list

**Key assertions:**
- Dialog opens with correct movement
- Updated amount is reflected in list
- Dialog closes on successful save

---

### Test 6: Move to Next Month

**File:** `src/HomeEconomics/frontend/e2e/specs/move-to-next-month.spec.ts`

**Scenario:** Move a movement from current month to next month

**Test setup:**
1. Create movement via API
2. Create current month via API
3. Create next month via API
4. Add movement to current month via API

**Test flow:**
1. Navigate to home page
2. Verify movement is in current month list
3. Click move to next month button
4. Confirm in dialog (if applicable)
5. Verify movement is removed from current month list
6. Click next month selector button
7. Verify movement appears in next month list

**Key assertions:**
- Movement is removed from current month
- Movement appears in next month
- No errors during move operation

---

### Test 7: Status Form

**File:** `src/HomeEconomics/frontend/e2e/specs/status-form.spec.ts`

**Scenario:** Update account/cash status and verify balance calculation

**Test setup:**
1. Create current month via API

**Test flow:**
1. Navigate to home page
2. Enter account value (5000)
3. Blur account input
4. Verify success message appears
5. Enter cash value (200)
6. Blur cash input
7. Verify balance displays 5200

**Key assertions:**
- Success message appears after saving
- Balance correctly calculates account + cash
- Values persist on page

---

## Phase 6: Scripts and Documentation

### 6.1 Update package.json

**File:** `src/HomeEconomics/frontend/package.json`

Add these scripts:
```json
{
  "scripts": {
    "dev": "next dev",
    "build": "next build",
    "start": "next start",
    "test": "vitest",
    "test:ci": "vitest run",
    "test:coverage": "vitest run --coverage",
    "e2e": "playwright test",
    "e2e:ui": "playwright test --ui",
    "e2e:headed": "playwright test --headed",
    "e2e:debug": "playwright test --debug",
    "e2e:report": "playwright show-report"
  }
}
```

---

### 6.2 Update .gitignore

**File:** `.gitignore` (repo root)

Add these entries:
```
# Playwright
test-results/
playwright-report/
playwright/.cache/
/tmp/homeeconomics-backend.pid
```

---

### 6.3 Update README.md

**File:** `src/HomeEconomics/frontend/README.md`

Add E2E section (see Phase 6.3 in implementation checklist below).

---

## Implementation Checklist

### Phase 1: Setup ✅
- [ ] Install Playwright: `npm install -D @playwright/test`
- [ ] Install Chromium: `npx playwright install chromium`
- [ ] Create `playwright.config.ts`
- [ ] Create `appsettings.Test.json` in backend

### Phase 2: Automation ✅
- [ ] Create `scripts/start-backend.sh`
- [ ] Make script executable: `chmod +x scripts/start-backend.sh`
- [ ] Create `e2e/global-setup.ts`
- [ ] Create `e2e/global-teardown.ts`

### Phase 3: Test Infrastructure ✅
- [ ] Create `e2e/utils/api-client.ts`
- [ ] Create `e2e/fixtures/test-data.ts`
- [ ] Create `e2e/utils/selectors.ts`

### Phase 4: data-testid Attributes ✅
- [ ] Update `components/movement-form.tsx`
- [ ] Update `components/movements-list.tsx`
- [ ] Update `components/confirm-delete-movement-dialog.tsx`
- [ ] Update `components/month-movement-month-selector.tsx`
- [ ] Update `components/current-month-movements-list.tsx`
- [ ] Update `components/edit-month-movement-amount-dialog.tsx`
- [ ] Update `components/movement-month-status-form.tsx`
- [ ] Update `app/page.tsx`

### Phase 5: Test Specs ✅
- [ ] Create `e2e/specs/movements-lifecycle.spec.ts`
- [ ] Create `e2e/specs/month-setup.spec.ts`
- [ ] Create `e2e/specs/add-to-current-month.spec.ts`
- [ ] Create `e2e/specs/pay-unpay.spec.ts`
- [ ] Create `e2e/specs/edit-amount.spec.ts`
- [ ] Create `e2e/specs/move-to-next-month.spec.ts`
- [ ] Create `e2e/specs/status-form.spec.ts`

### Phase 6: Documentation ✅
- [ ] Update `package.json` with E2E scripts
- [ ] Update `.gitignore`
- [ ] Update `README.md` with E2E section

### Verification ✅
- [ ] Run first E2E test: `npm run e2e`
- [ ] Verify test database created
- [ ] Check Playwright report: `npm run e2e:report`
- [ ] Fix any failing tests
- [ ] Run all tests sequentially

---

## Troubleshooting Guide

### Backend fails to start

**Symptoms:**
```
Backend failed to start
```

**Solutions:**
1. Check if port 5000 is already in use:
   ```bash
   lsof -i :5000
   kill -9 <PID>
   ```

2. Verify PostgreSQL is running:
   ```bash
   pg_isready
   # If not running:
   brew services start postgresql@14
   ```

3. Check test database exists:
   ```bash
   psql -U homeeconomics -l | grep homeeconomics-test
   ```

4. Manually create database if needed:
   ```bash
   psql -U homeeconomics -c "CREATE DATABASE \"homeeconomics-test\";"
   ```

---

### Frontend fails to start

**Symptoms:**
```
webServer failed to start
```

**Solutions:**
1. Check if port 3000 is in use:
   ```bash
   lsof -i :3000
   kill -9 <PID>
   ```

2. Clear Next.js cache:
   ```bash
   rm -rf .next
   ```

3. Reinstall dependencies:
   ```bash
   rm -rf node_modules package-lock.json
   npm install
   ```

---

### Tests are flaky

**Symptoms:**
- Tests pass sometimes, fail other times
- Timeouts or race conditions

**Solutions:**
1. Verify sequential execution in `playwright.config.ts`:
   ```typescript
   workers: 1,
   fullyParallel: false,
   ```

2. Increase timeout for slow operations:
   ```typescript
   test('slow test', async ({ page }) => {
     test.setTimeout(60000); // 60 seconds
     // ...
   });
   ```

3. Add explicit waits:
   ```typescript
   await page.waitForSelector('[data-testid="element"]');
   ```

4. Check network requests in Playwright report:
   ```bash
   npm run e2e:report
   ```

---

### Database state issues

**Symptoms:**
- Tests fail due to existing data
- Unique constraint violations

**Solutions:**
1. Ensure cleanup runs in `afterEach`:
   ```typescript
   test.afterEach(async () => {
     if (movementId) {
       await apiClient.deleteMovement(movementId).catch(() => {});
     }
   });
   ```

2. Use unique test names:
   ```typescript
   const name = generateUniqueTestName('E2E Test');
   ```

3. Manually reset test database:
   ```bash
   psql -U homeeconomics -c "DROP DATABASE \"homeeconomics-test\";"
   psql -U homeeconomics -c "CREATE DATABASE \"homeeconomics-test\";"
   # Backend will auto-migrate on next run
   ```

---

### Playwright UI debugging

**Commands:**
```bash
# Run with UI for debugging
npm run e2e:ui

# Run with visible browser
npm run e2e:headed

# Debug mode (pauses at each step)
npm run e2e:debug

# Run single test file
npx playwright test e2e/specs/movements-lifecycle.spec.ts

# Run specific test by name
npx playwright test -g "should create, edit, and delete"
```

---

## Summary

### Total Files to Create: 15

**Configuration (2):**
1. `playwright.config.ts`
2. `../../appsettings.Test.json`

**Automation Scripts (3):**
3. `scripts/start-backend.sh`
4. `e2e/global-setup.ts`
5. `e2e/global-teardown.ts`

**Test Infrastructure (3):**
6. `e2e/utils/api-client.ts`
7. `e2e/fixtures/test-data.ts`
8. `e2e/utils/selectors.ts`

**Test Specs (7):**
9. `e2e/specs/movements-lifecycle.spec.ts`
10. `e2e/specs/month-setup.spec.ts`
11. `e2e/specs/add-to-current-month.spec.ts`
12. `e2e/specs/pay-unpay.spec.ts`
13. `e2e/specs/edit-amount.spec.ts`
14. `e2e/specs/move-to-next-month.spec.ts`
15. `e2e/specs/status-form.spec.ts`

### Total Files to Modify: 11

**Components (8):**
1. `components/movement-form.tsx`
2. `components/movements-list.tsx`
3. `components/confirm-delete-movement-dialog.tsx`
4. `components/month-movement-month-selector.tsx`
5. `components/current-month-movements-list.tsx`
6. `components/edit-month-movement-amount-dialog.tsx`
7. `components/movement-month-status-form.tsx`
8. `app/page.tsx`

**Config/Docs (3):**
9. `package.json`
10. `README.md`
11. `../../.gitignore`

---

## Next Steps

1. Review this implementation plan
2. Start with Phase 1 (Playwright setup)
3. Proceed sequentially through phases
4. Test each phase before moving to next
5. Run full E2E suite at the end
6. Update `testing-strategy.md` to mark section 6 as implemented

---

**Document End**
