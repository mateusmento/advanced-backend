import ReactDOM from "react-dom/client";
import { Hello } from "./Hello";

const root = document.getElementById("root") as HTMLElement;
ReactDOM.createRoot(root)
  .render(<Hello/>);
