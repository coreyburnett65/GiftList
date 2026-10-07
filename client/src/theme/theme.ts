import { createTheme } from '@mui/material/styles';

// Warm, friendly, uncluttered per SPEC.md 9 — large tap targets, readable on a
// phone, light holiday touches without being seasonal-only.
export const theme = createTheme({
  palette: {
    primary: {
      main: '#c0552f',
    },
    secondary: {
      main: '#2f7a5c',
    },
    background: {
      default: '#fff8f1',
      paper: '#ffffff',
    },
  },
  shape: {
    borderRadius: 12,
  },
  typography: {
    fontFamily: ['"Segoe UI"', 'Roboto', 'sans-serif'].join(','),
  },
  components: {
    MuiButton: {
      defaultProps: {
        size: 'large',
      },
      styleOverrides: {
        root: {
          textTransform: 'none',
        },
      },
    },
  },
});
