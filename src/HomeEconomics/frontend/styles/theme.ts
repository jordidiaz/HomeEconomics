import { createTheme } from "@mui/material/styles";

export const theme = createTheme({
  palette: {
    mode: "light",
    primary: {
      main: "#0f766e",
      contrastText: "#f8fafc",
    },
    secondary: {
      main: "#b45309",
    },
    success: {
      main: "#15803d",
    },
    error: {
      main: "#b42318",
    },
    warning: {
      main: "#b45309",
    },
    info: {
      main: "#1d4ed8",
    },
    background: {
      default: "#f5f2ed",
      paper: "#ffffff",
    },
    text: {
      primary: "#1f2933",
      secondary: "#5b6470",
    },
    divider: "#e2e8f0",
  },
  shape: {
    borderRadius: 12,
  },
  typography: {
    fontFamily: '"DM Sans", "Segoe UI", sans-serif',
    h1: {
      fontFamily: '"Fraunces", "DM Sans", sans-serif',
      fontSize: "2.2rem",
      fontWeight: 600,
      letterSpacing: "-0.02em",
    },
    h2: {
      fontFamily: '"Fraunces", "DM Sans", sans-serif',
      fontSize: "1.8rem",
      fontWeight: 600,
      letterSpacing: "-0.01em",
    },
    h3: {
      fontFamily: '"Fraunces", "DM Sans", sans-serif',
      fontSize: "1.5rem",
      fontWeight: 600,
    },
    h4: {
      fontSize: "1.25rem",
      fontWeight: 600,
    },
    h6: {
      fontSize: "1.05rem",
      fontWeight: 600,
    },
    body1: {
      fontSize: "0.98rem",
    },
    body2: {
      fontSize: "0.9rem",
    },
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        "@import": [
          "url('https://fonts.googleapis.com/css2?family=DM+Sans:wght@400;500;600;700&family=Fraunces:wght@500;600;700&display=swap')",
        ],
        "*": {
          boxSizing: "border-box",
        },
        body: {
          background:
            "linear-gradient(180deg, rgba(245,242,237,1) 0%, rgba(240,244,248,1) 100%)",
          color: "#1f2933",
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: "none",
          fontWeight: 600,
          borderRadius: 12,
          padding: "10px 18px",
        },
      },
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          backgroundColor: "#fbfbfd",
          borderRadius: 12,
          transition: "box-shadow 0.2s ease, border-color 0.2s ease",
          "& .MuiOutlinedInput-notchedOutline": {
            borderColor: "#d7dee6",
          },
          "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: "#b3c3d5",
          },
          "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
            borderColor: "#0f766e",
            boxShadow: "0 0 0 3px rgba(15, 118, 110, 0.15)",
          },
        },
      },
    },
    MuiInputLabel: {
      styleOverrides: {
        root: {
          fontWeight: 600,
        },
      },
    },
    MuiToggleButtonGroup: {
      styleOverrides: {
        root: {
          borderRadius: 14,
          backgroundColor: "#ffffff",
          border: "1px solid #e2e8f0",
          padding: 4,
        },
      },
    },
    MuiToggleButton: {
      styleOverrides: {
        root: {
          textTransform: "none",
          borderRadius: 10,
          border: "none",
          padding: "10px 16px",
          "&.Mui-selected": {
            backgroundColor: "rgba(15, 118, 110, 0.12)",
            color: "#0f766e",
          },
          "&.Mui-selected:hover": {
            backgroundColor: "rgba(15, 118, 110, 0.2)",
          },
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          fontWeight: 600,
        },
      },
    },
    MuiAlert: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          boxShadow: "0 8px 20px rgba(15, 23, 42, 0.08)",
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: "none",
        },
      },
    },
  },
});
