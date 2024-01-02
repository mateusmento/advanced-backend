import { Configuration } from "webpack";
import HtmlWebpackPlugin from "html-webpack-plugin";
import * as path from "path";

export default {
  entry: "./src/index.tsx",
  output: {
    path: path.resolve(__dirname, "..", "dist"),
    filename: "index.js"
  },
  resolve: {
    extensions: [".ts", ".tsx", ".js", ".jsx"]
  },
  module: {
    rules: [
      {
        test: /\.(ts|tsx)$/,
        exclude: /node_modules/,
        use: "babel-loader"
      }
    ]
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: "./src/index.html"
    }),
  ],
} as Configuration;
