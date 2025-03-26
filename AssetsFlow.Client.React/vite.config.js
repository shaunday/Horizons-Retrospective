import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  server: {
    https: false,  
    host: 'localhost',
    port: 5173
  },
  resolve: {
    alias: {
      '@components': path.resolve(__dirname, 'src/Components'),
      '@journalComponents': path.resolve(__dirname, 'src/Components/Journal'),
      '@views': path.resolve(__dirname, 'src/Views'),
      '@hooks': path.resolve(__dirname, 'src/Hooks'),
      '@services': path.resolve(__dirname, 'src/Services'),
      '@constants': path.resolve(__dirname, 'src/Constants'),
      '@context': path.resolve(__dirname, 'src/Contexts'),
    },
  },
});