import js from "@eslint/js";
import nodePlugin from "eslint-plugin-n";
import importPlugin from "eslint-plugin-import";

export default [
    js.configs.recommended,
    {
        files: ["src/**/*.js", "src/**/*.mjs"],
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
        },
        languageOptions: {
            ecmaVersion: 2021,
            sourceType: "module",
        },
        rules: {
            ...nodePlugin.configs.recommended.rules,
            ...importPlugin.configs.recommended.rules,
            // Add your custom rules or overrides here
        },
    },
];
