const colors = require('tailwindcss/colors')

module.exports = {
  purge: ["./**/*.cshtml","../**/*.cshtml", "../**/*.html","./**/*.html", "./**/*.razor"],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      colors: {
        violet: colors.violet,
      }
    },
  },
  variants: {
    extend: { },
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}