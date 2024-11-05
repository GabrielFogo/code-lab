module.exports = {
  content: [
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml'
  ],
  theme: {
    extend: {
      colors: {
        gray:{
          100: '#d9d9d9',
          200: '#9f9f9f',
          400: '#595959',
          500: '#212121',
          600: '#252525',
          700: '#1e1e1e',
          800: '#121214',
          900: '#0e0e0e'
        },
        orange:{
          DEFAULT: '#ff4d00'
        },
        blue:{
          DEFAULT: '#0038FF'
        },
        yellow:{
          DEFAULT: '#faff00'
        },
        cyan:{
          100: '#2f9cc0',
          200:'#16495a'
        },
        green:{
          DEFAULT:'#3bce07'
        },
        red:{
          500: '#bb0000'
        }
      }
    },
    fontSize: {
      '2xs': '0.625rem',
      'xs': '0.75rem',
      sm: '0.875rem',
      md: '1rem',
      lg: '1.125rem',
      xl: '1.5rem',
    },
    borderRadius: {
      'none': '0px',
      'sm': '0.1875rem',
      'DEFAULT': '0.5rem',
      'md': '0.4375rem',
      'lg': '1.25rem',
      'xl': '1.5rem',
      'full': '50%',
    },
    backgroundImage: {
      'cyan-gradient': 'linear-gradient(267.92deg, #16495A 0%, #2F9CC0 100%)',
    },
    boxShadow: {
      'custom-light': '0px 7px 5px 2px #00000080',
      'custom-inset': '0px 0px 10px 10px #00000040 inset',
    },
  },
    plugins: [
        require('tailwindcss'),
        require('autoprefixer'),
    ],
}