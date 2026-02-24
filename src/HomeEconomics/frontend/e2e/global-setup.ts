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
      const response = await page.goto('http://localhost:5000/self');
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
