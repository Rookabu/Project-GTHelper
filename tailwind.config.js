/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [ 
    "./src/index.html",
    "./src/**/*.{fs,js,ts,jsx,tsx}"
],
  theme: {
    extend: {},
  },
  plugins: [require("daisyui")],
}

