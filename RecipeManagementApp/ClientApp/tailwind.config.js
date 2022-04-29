const colors = require('tailwindcss/colors')

module.exports = {
	content: ['./src/**/*.{js,jsx,ts,tsx}', './index.html'],
	theme: {
		extend: {},
	},
  plugins: [
    require('@tailwindcss/forms'),
  ],
}