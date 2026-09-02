import { configureStore, createSlice, type PayloadAction } from "@reduxjs/toolkit";

export type User = {
  email: string;
  password: string;
};

type AuthState = {
  users: User[];
  currentEmail: string | null;
};

const initialState: AuthState = {
  users: [],
  currentEmail: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    register(state, action: PayloadAction<User>) {
      state.users.push(action.payload);
      state.currentEmail = action.payload.email;
    },
    login(state, action: PayloadAction<User>) {
      state.currentEmail = action.payload.email;
    },
    logout(state) {
      state.currentEmail = null;
    },
  },
});

export const { login, register, logout } = authSlice.actions;

export const store = configureStore({
  reducer: {
    auth: authSlice.reducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
