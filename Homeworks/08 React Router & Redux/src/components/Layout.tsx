import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import { Link as RouterLink, Outlet } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { logout, type RootState } from "../store.ts";

export function Layout() {
  const currentEmail = useSelector((state: RootState) => state.auth.currentEmail);
  const dispatch = useDispatch();

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            HW8
          </Typography>
          <Button color="inherit" component={RouterLink} to="/">
            Home
          </Button>
          {currentEmail ? (
            <Button color="inherit" onClick={() => dispatch(logout())}>
              Выйти
            </Button>
          ) : (
            <>
              <Button color="inherit" component={RouterLink} to="/login">
                Login
              </Button>
              <Button color="inherit" component={RouterLink} to="/register">
                Register
              </Button>
            </>
          )}
        </Toolbar>
      </AppBar>
      <Box component="main" sx={{ p: 2 }}>
        <Outlet />
      </Box>
    </>
  );
}
