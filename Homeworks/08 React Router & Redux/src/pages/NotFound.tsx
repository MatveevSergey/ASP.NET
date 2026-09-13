import { Button, Typography } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

export function NotFound() {
  return (
    <>
      <Typography variant="h4" component="h1" gutterBottom>
        404
      </Typography>
      <Typography variant="body1" sx={{ mb: 2 }}>
        Страница не найдена
      </Typography>
      <Button component={RouterLink} to="/" variant="contained">
        На главную
      </Button>
    </>
  );
}