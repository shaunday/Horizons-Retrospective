/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      spacing: {
        '1px': '1px',
        '2px': '2px',
        '3px': '3px',
        '5px': '5px',
      },
    },
  },
  plugins: [],
};
