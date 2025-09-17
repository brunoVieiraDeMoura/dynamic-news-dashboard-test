'use client';
import { type_first, type_second } from '../utils/font';
import { createTheme, responsiveFontSizes } from '@mui/material/styles';

let theme = createTheme({
  components: {
    MuiInputBase: {
      styleOverrides: {
        root: {
          color: 'blue', // Cor do texto padrão
        },
        input: {
          color: '#333',
          '&::placeholder': {
            color: '#333', // Cor do placeholder
            opacity: 1, // Garante que a opacidade seja 100%
          },
        },
      },
    },
    // MuiFormLabel: {
    //   styleOverrides: {
    //     root: {
    //       color: '#fff',
    //     },
    //   },
    // },
    // MuiOutlinedInput: {
    //   styleOverrides: {
    //     notchedOutline: {
    //       borderColor: '#fff', // Cor do contorno padrão
    //     },
    //   },
    // },
  },
  spacing: 8,

  palette: {
    mode: 'light',
    common: {
      black: '#000000',
      white: '#ffffff',
    },
    primary: {
      main: '#5599cc',
      light: '##226699',
      dark: '#115588',
      contrastText: '#ffffff',
    },
    secondary: {
      main: '#EA7B03',
      light: '#eeeeee',
      dark: '#333333',
      contrastText: '#ffffff',
    },
    error: {
      main: '#d32f2f',
      light: '#ef5350',
      dark: '#c62828',
      contrastText: '#ffffff',
    },
    warning: {
      main: '#ed6c02',
      light: '#ff9800',
      dark: '#e65100',
      contrastText: '#ffffff',
    },
    info: {
      main: '#D6F6F9',
      light: '#03a9f4',
      dark: '#01579b',
      contrastText: '#ffffff',
    },
    success: {
      main: '#2e7d32',
      light: '#4caf50',
      dark: '#1b5e20',
      contrastText: '#ffffff',
    },
    grey: {
      50: '#fafafa',
      100: '#f5f5f5',
      200: '#eeeeee',
      300: '#e0e0e0',
      400: '#bdbdbd',
      500: '#9e9e9e',
      600: '#757575',
      700: '#616161',
      800: '#424242',
      900: '#212121',
      A100: '#f5f5f5',
      A200: '#eeeeee',
      A400: '#bdbdbd',
      A700: '#616161',
    },
  },
  typography: {
    htmlFontSize: 16,
    fontFamily: type_first.style.fontFamily,
    fontSize: 14,
    fontWeightLight: 300,
    fontWeightRegular: 400,
    fontWeightMedium: 500,
    fontWeightBold: 700,
    h1: {
      fontFamily: type_first.style.fontFamily,
      fontWeight: 300,
      lineHeight: 1.0,
      letterSpacing: '-0.01562em',
    },
    h2: {
      fontFamily: type_first.style.fontFamily,
      fontWeight: 300,
      lineHeight: 1.0,
      letterSpacing: '-0.00833em',
    },
    h3: {
      fontFamily: type_first.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.0,
      letterSpacing: '0em',
    },
    h4: {
      fontFamily: type_first.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.0,
      letterSpacing: '0.00735em',
    },
    h5: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.1,
      letterSpacing: '0em',
    },
    h6: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 500,
      lineHeight: 1.6,
      letterSpacing: '0.0075em',
    },
    subtitle1: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.75,
      letterSpacing: '0.00938em',
    },
    subtitle2: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 500,
      lineHeight: 1.57,
      letterSpacing: '0.00714em',
    },
    body1: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.2,
      letterSpacing: '0.00938em',
    },
    body2: {
      fontFamily: type_second.style.fontFamily,
      fontWeight: 400,
      lineHeight: 1.2,
      letterSpacing: '0.01071em',
    },
  },
});

theme = responsiveFontSizes(theme);

export default theme;
