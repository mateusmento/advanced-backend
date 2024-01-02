import { FormEvent, useState } from "react";
import axios from "axios";

export function Hello() {
  const [email, setEmail] = useState("");

  async function auth(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();
    axios.post("https://api.cookie.com:3000/auth/sign", { email }, { withCredentials: true });
  }

  return (
    <form onSubmit={auth}>
      <input value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email..."/>
      <button>Sign In</button>
    </form>
  );
}
