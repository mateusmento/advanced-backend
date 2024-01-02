import commonConfig from "./webpack/webpack.common";
import merge from "webpack-merge";

export default (env: any) => {
  const config = require(`./webpack/webpack.${env.mode}.ts`).default;
  return merge(commonConfig, config);
};
