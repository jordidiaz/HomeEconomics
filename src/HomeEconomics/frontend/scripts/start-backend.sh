#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/../../../.." && pwd)"
PROJECT_PATH="$REPO_ROOT/src/HomeEconomics"
PID_FILE="/tmp/homeeconomics-backend.pid"
LOG_FILE="/tmp/homeeconomics-backend.log"
PORT=5000
READY_ENDPOINT="http://localhost:${PORT}/self"
MAX_WAIT_SECONDS=45

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet is not installed or not available in PATH"
  exit 1
fi

if [[ -f "$PID_FILE" ]]; then
  EXISTING_PID="$(cat "$PID_FILE" 2>/dev/null || true)"
  if [[ -n "$EXISTING_PID" ]] && kill -0 "$EXISTING_PID" 2>/dev/null; then
    echo "Backend already running with PID $EXISTING_PID"
    echo "Stop it first or remove $PID_FILE if stale"
    exit 1
  fi
  rm -f "$PID_FILE"
fi

if command -v lsof >/dev/null 2>&1 && lsof -i ":${PORT}" -sTCP:LISTEN >/dev/null 2>&1; then
  echo "Port ${PORT} is already in use"
  echo "Run: lsof -i :${PORT}"
  exit 1
fi

if command -v pg_isready >/dev/null 2>&1; then
  if ! pg_isready -h localhost -p 5432 >/dev/null 2>&1; then
    echo "PostgreSQL is not ready on localhost:5432"
    exit 1
  fi
fi

echo "Starting backend with test database..."
echo "Repo root: $REPO_ROOT"
echo "Logs: $LOG_FILE"

export ASPNETCORE_ENVIRONMENT=Test
export DOTNET_ENVIRONMENT=Test
dotnet run --no-launch-profile --project "$PROJECT_PATH" --urls "http://localhost:${PORT}" >"$LOG_FILE" 2>&1 &
BACKEND_PID=$!
echo "$BACKEND_PID" >"$PID_FILE"

echo "Waiting for backend to be ready..."
for ((i = 1; i <= MAX_WAIT_SECONDS; i++)); do
  if ! kill -0 "$BACKEND_PID" 2>/dev/null; then
    echo "Backend process exited before becoming ready"
    echo "Last log lines:"
    tail -n 40 "$LOG_FILE" || true
    rm -f "$PID_FILE"
    exit 1
  fi

  if curl -fsS "$READY_ENDPOINT" >/dev/null 2>&1; then
    echo "Backend is ready at $READY_ENDPOINT"
    exit 0
  fi

  sleep 1
done

echo "Backend failed to start within ${MAX_WAIT_SECONDS}s"
echo "Last log lines:"
tail -n 40 "$LOG_FILE" || true
kill "$BACKEND_PID" 2>/dev/null || true
rm -f "$PID_FILE"
exit 1
