const { execSync } = require('child_process');
const { join } = require('path');
const { copySync, removeSync } = require('fs-extra');

const buildDir = join(__dirname, 'build');
const clientDir = join(__dirname, '..', 'ADOPSE', 'ClientApp');
const serverDir = join(__dirname, '..', 'ADOPSE');

const buildClient = () => {
  console.log('Cleaning client build folder...');
  removeSync(join(buildDir, 'wwwroot'));
  console.log('Building client...');
  execSync('npm run build', { cwd: clientDir, stdio: 'inherit' });
  console.log('Copying client build to server...');
  copySync(join(clientDir, 'build'), join(buildDir, 'wwwroot'));
}

const buildServer = () => {
  console.log('Cleaning server build folder...');
  removeSync(join(buildDir, 'server'));
  console.log('Building server...');
  execSync('dotnet publish -c Release -r linux-x64', { cwd: serverDir, stdio: 'inherit' });
  console.log('Copying server build to build folder...');
  copySync(join(serverDir, 'bin', 'Release', 'net7.0', 'linux-x64', 'publish'), join(buildDir, 'server'));
}

const command = process.argv[2];

if (command === 'buildClient') {
  buildClient();
} else if (command === 'buildServer') {
  buildServer();
} else {
  console.error(`Unknown command: ${command}`);
  process.exit(1);
}