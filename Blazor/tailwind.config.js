/** @type {import('tailwindcss').Config} */
module.exports = {
  content:  ['./**/*.{razor,html,css}', './**/**/*.{razor,html,css}'],
  theme: {
    extend: {
      width: {
        '134': '34rem',
        '188': '47rem',
      }
    },
  },
  plugins: [
    require("daisyui"),
    require("tailwindcss-flip"),
    require('@tailwindcss/typography'),
  ],
  daisyui: {
    themes: false,
    rtl: true
  },
}

