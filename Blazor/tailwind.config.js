/** @type {import('tailwindcss').Config} */
module.exports = {
  content:  ['./**/*.{razor,html,css}', './**/**/*.{razor,html,css}'],
  theme: {
    extend: {},
  },
  plugins: [
    require("daisyui")
  ],
  daisyui: {
    themes: ["light", "dark"],
  },
}

