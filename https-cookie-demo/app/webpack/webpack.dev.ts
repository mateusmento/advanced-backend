import { Configuration } from "webpack";
import "webpack-dev-server";

export default {
  mode: "development",
  devServer: {
    hot: true,
    open: true,
    host: "app.cookie.com",
    port: "auto",
    historyApiFallback: true,
    server: {
      type: "https",
      options: {
        key: "./https/key.pem",
        cert: "./https/cert.pem"
      }
    }
  },
} as Configuration;
