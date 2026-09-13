import { Typography } from "@mui/material";
import { useSelector } from "react-redux";
import { withAuth } from "../hoc/withAuth.tsx";
import type { RootState } from "../store.ts";

function HomePageView() {
  const currentEmail = useSelector((state: RootState) => state.auth.currentEmail);

  return (
    <>
      <Typography variant="h4" component="h1" gutterBottom>
        Home
      </Typography>
      <Typography>Вы вошли как {currentEmail}</Typography>
    </>
  );
}

export const HomePage = withAuth(HomePageView);