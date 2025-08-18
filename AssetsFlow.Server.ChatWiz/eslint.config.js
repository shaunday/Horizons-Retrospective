import js from "@eslint/js";
import nodePlugin from "eslint-plugin-n";
import importPlugin from "eslint-plugin-import";
import tsPlugin from "@typescript-eslint/eslint-plugin";

export default [
    js.configs.recommended,
    {
        files: ["src/**/*.{ts,tsx,js,mjs}"],
        ignores: [
            "node_modules/",
            "dist/",
            "build/",
            "coverage/",
            ".types/",
            ".generated/",
        ],
        plugins: {
            n: nodePlugin,
            import: importPlugin,
            "@typescript-eslint": tsPlugin,
        },
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "module",
        },
        rules: {
            ...nodePlugin.configs.recommended.rules,
            ...importPlugin.configs.recommended.rules,
            ...tsPlugin.configs.recommended.rules,
            // Add your custom rules or overrides here
        },
    },
];