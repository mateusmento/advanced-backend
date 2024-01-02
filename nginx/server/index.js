const express = require("express");

const app = express();

app.get("/", (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.json({hello: "world", port: process.env.PORT});
});

app.listen(process.env.PORT || 3000);
