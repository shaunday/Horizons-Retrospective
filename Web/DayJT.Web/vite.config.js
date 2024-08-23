import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'; // Import path module

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
  alias: {
        '@components': path.resolve(__dirname, 'src/TradesJournal/Components'),
        '@hooks': path.resolve(__dirname, 'src/Hooks'),
        '@services': path.resolve(__dirname, 'src/Services'),
        '@constants': path.resolve(__dirname, 'src/Constants'),
    },
},
})
