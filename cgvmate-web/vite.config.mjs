import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import jsconfigPaths from 'vite-jsconfig-paths';
import path from 'path';

// ----------------------------------------------------------------------

export default defineConfig(({ mode }) => {
  // Load environment variables from `envFile`
  const env = loadEnv(mode, process.cwd(), 'process.env');

  return {
    plugins: [react(), jsconfigPaths()],
    base: '',
    define: {
      global: 'window',
      'process.env': env,  // Pass the loaded environment variables
    },
    resolve: {
      // alias: [
      //   {
      //     find: /^~(.+)/,
      //     replacement: path.join(process.cwd(), 'node_modules/$1')
      //   },
      //   {
      //     find: /^src(.+)/,
      //     replacement: path.join(process.cwd(), 'src/$1')
      //   }
      // ]
    },
    build: {
      outDir: 'build', // 출력 디렉터리를 'build'로 변경
      rollupOptions: {
        external: ['react-helmet-async'], // 외부 모듈로 설정하여 Rollup 경고 해결
      },
    },
    server: {
      open: true,
      port: 3000
    },
    preview: {
      open: true,
      port: 3000
    }
  };
});
