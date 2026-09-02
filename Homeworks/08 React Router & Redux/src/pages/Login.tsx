import { useState } from "react";
import { Alert, Button, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { login, type RootState } from "../store.ts";

export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const users = useSelector((state: RootState) => state.auth.users);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const trimmedEmail = email.trim();

    const user = users.find(
      (item) => item.email.toLowerCase() === trimmedEmail.toLowerCase(),
    );

    if (!user || user.password !== password) {
      setError("Неверный email или пароль");
      return;
    }

    dispatch(login({ email: user.email, password }));
    navigate("/");
  }

  return (
    <Stack component="form" onSubmit={handleSubmit} spacing={2}>
      <Typography variant="h4" component="h1">
        Login
      </Typography>
      {error && <Alert severity="error">{error}</Alert>}
      <TextField
        label="Email"
        type="email"
        value={email}
        onChange={(event) => {
          setEmail(event.target.value);
          setError(null);
        }}
        required
      />
      <TextField
        label="Пароль"
        type="password"
        value={password}
        onChange={(event) => {
          setPassword(event.target.value);
          setError(null);
        }}
        required
      />
      <Button type="submit" variant="contained">
        Войти
      </Button>
    </Stack>
  );
}
