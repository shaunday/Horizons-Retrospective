// eslint.config.js
import js from "@eslint/js";
import reactPlugin from "eslint-plugin-react";
import reactHooksPlugin from "eslint-plugin-react-hooks";
import importPlugin from "eslint-plugin-import";
import reactRefreshPlugin from "eslint-plugin-react-refresh";

export default [
  js.configs.recommended,
  {
    files: ["**/*.js", "**/*.jsx"],
    ignores: [
      "node_modules/",
      "dist/",
      "build/",
      "coverage/",
      ".types/",
      ".generated/",
    ],

    plugins: {
      react: reactPlugin,
      "react-hooks": reactHooksPlugin,
      "react-refresh": reactRefreshPlugin,
      import: importPlugin,
    },

    languageOptions: {
      ecmaVersion: "latest",
      sourceType: "module",
      globals: {
        window: "readonly",
        document: "readonly",
      },
      parserOptions: {
        ecmaFeatures: { jsx: true },
      },
    },

    settings: {
      react: { version: "detect" },
      "import/resolver": {
        alias: {
          map: [
            ["@src", "./src"],
            ["@components", "./src/Components"],
            ["@journalComponents", "./src/Components/Journal"],
            ["@views", "./src/Views"],
            ["@hooks", "./src/Hooks"],
            ["@services", "./src/Services"],
            ["@constants", "./src/Constants"],
            ["@context", "./src/Contexts"],
          ],
          extensions: [".js", ".jsx", ".json", ".ts", ".tsx"],
        },
      },
    },

    rules: {
      ...reactPlugin.configs.recommended.rules,
      ...reactHooksPlugin.configs.recommended.rules,
      ...importPlugin.configs.recommended.rules,
      "no-unused-vars": ["warn", { argsIgnorePattern: "^_" }],
      "react-refresh/only-export-components": "warn",
      "react/react-in-jsx-scope": "off",
    },
  },
];
