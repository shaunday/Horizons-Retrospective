import js from "@eslint/js";
import nodePlugin from "eslint-plugin-n";
import importPlugin from "eslint-plugin-import";
import tsPlugin from "@typescript-eslint/eslint-plugin";
import tsParser from "@typescript-eslint/parser";

export default [
    js.configs.recommended,
    {
        files: ["src/**/*.{ts,tsx,js,mjs}"],
        ignores: ["node_modules/", "dist/", "build/", "coverage/", ".types/", ".generated/"],
        languageOptions: {
            parser: tsParser,
            ecmaVersion: 2022,
            sourceType: "module",
            globals: {
                process: "readonly",
                URL: "readonly",
                console: "readonly",
                setTimeout: "readonly",
            }
        },
        plugins: {
            n: nodePlugin,
            import: importPlugin,
            "@typescript-eslint": tsPlugin
        },
        settings: {
            "import/resolver": {
                typescript: {
                    alwaysTryTypes: true,
                    project: "./tsconfig.json"
                },
                node: {
                    extensions: [".js", ".ts", ".tsx"]
                }
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
