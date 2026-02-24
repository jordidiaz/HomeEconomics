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
