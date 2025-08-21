import js from "@eslint/js";
import nodePlugin from "eslint-plugin-n";
import importPlugin from "eslint-plugin-import";
import tsPlugin from "@typescript-eslint/eslint-plugin";

export default [
    js.configs.recommended,
    {
        files: ["src/**/*.{ts,tsx,js,mjs}"],
        ignores: ["node_modules/", "dist/", "build/", "coverage/", ".types/", ".generated/"],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "module",
            globals: {
                process: "readonly",
                URL: "readonly"
            }
        },
        plugins: {
            n: nodePlugin,
            import: importPlugin,
            "@typescript-eslint": tsPlugin
        },
        settings: {
            "import/resolver": {
                typescript: { project: "./tsconfig.json" },
                node: { extensions: [".js", ".ts", ".tsx"] }
            }
        },
        rules: {
            ...nodePlugin.configs.recommended.rules,
            ...importPlugin.configs.recommended.rules,
            ...tsPlugin.configs.recommended.rules,
            "import/extensions": [
                "error",
                "ignorePackages",
                { ts: "never", tsx: "never", js: "never", mjs: "never" }
            ],
            "import/no-unresolved": "off"
        }
    }
];
