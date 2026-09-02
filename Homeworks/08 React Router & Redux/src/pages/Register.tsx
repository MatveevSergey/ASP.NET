import { useState } from "react";
import { Alert, Button, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { register, type RootState } from "../store.ts";

export function Register() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [passwordRepeat, setPasswordRepeat] = useState("");
  const [error, setError] = useState<string | null>(null);
  const users = useSelector((state: RootState) => state.auth.users);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const trimmedEmail = email.trim();

    if (password !== passwordRepeat) {
      setError("Пароли не совпадают");
      return;
    }

    const exists = users.some(
      (item) => item.email.toLowerCase() === trimmedEmail.toLowerCase(),
    );

    if (exists) {
      setError("Такой email уже зарегистрирован");
      return;
    }

    dispatch(register({ email: trimmedEmail, password }));
    navigate("/");
  }

  return (
    <Stack component="form" onSubmit={handleSubmit} spacing={2}>
      <Typography variant="h4" component="h1">
        Register
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
      <TextField
        label="Повтор пароля"
        type="password"
        value={passwordRepeat}
        onChange={(event) => {
          setPasswordRepeat(event.target.value);
          setError(null);
        }}
        required
      />
      <Button type="submit" variant="contained">
        Зарегистрироваться
      </Button>
    </Stack>
  );
}
