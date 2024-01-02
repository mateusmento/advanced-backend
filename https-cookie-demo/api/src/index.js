const express = require("express");
const bodyParser = require("body-parser");
const cookieParser = require("cookie-parser");
const cors = require("cors");
const path = require("path");
const { existsSync, readFileSync } = require("fs");
const https = require('https');

const app = express();

const httpsPath = path.resolve(__dirname, "..", "https");
const key = readFileSync(`${httpsPath}/cookie.com.key`, "utf8");
const cert = readFileSync(`${httpsPath}/cookie.com.crt`, "utf8");
const server = https.createServer({ key, cert }, app);

app.use(cors({
    origin: "https://app.cookie.com:8080",
    credentials: true
}));

app.use(bodyParser.json());
app.use(cookieParser("cookie-secret"));

app.post("/auth/sign", (req, res) => {
    res.cookie("token", req.body.email, {
        maxAge: 6000000,
        httpOnly: true,
        secure: true,
        domain: "cookie.com",
    });
    res.status(200);
    res.send({authenticated: true});
});

app.get("*", (req, res) => {
    console.log(req.cookies);
    const appDir = path.resolve(__dirname, "../../app/dist");
    if (existsSync(appDir + req.url))
        res.sendFile(appDir + req.url);
    else
        res.sendFile(appDir + "/index.html");
});

server.listen(3000, "api.cookie.com", () => {
    console.log("listening to port 3000");
});
