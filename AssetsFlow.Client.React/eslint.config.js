module.exports = {
  env: { browser: true, es2020: true },
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:react/jsx-runtime',
    'plugin:react-hooks/recommended',
    'plugin:import/recommended', // added import plugin recommended config
    'prettier', // Ensure this is the last in the extends array
  ],
  parserOptions: { ecmaVersion: 'latest', sourceType: 'module' },
  settings: { react: { version: 'detect' } },
  plugins: ['react-refresh', 'import'], // added import plugin here
  rules: {
    'react-refresh/only-export-components': 'warn',
  },
};
