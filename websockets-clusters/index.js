const { createServer } = require('http');
const express = require('express');
const cookieParser = require("cookie-parser");
const io = require('socket.io');
const { createClient } = require('redis');
const { createAdapter } = require('@socket.io/redis-adapter');
const cors = require('cors');

(async () => {
  const app = express();
  app.use(cors({ origin: 'http://localhost:8080', credentials: true }));
  app.use(express.static(__dirname + "/public"));
  app.use(express.json());
  app.use(cookieParser("cookie-secret"));
  
  const server = createServer(app);

  const websockets = io(server, {
    cors: {
      origin: "http://localhost:8080",
      credentials: true
    },
    adapter: await createRedisAdapter(),
  });
 
  server.listen(3000, () => {
    console.log(`Listening to port ${3000}`);
  });

  app.get('/hello', (req, res) => {
    console.log('hello');
    res.json({ hello: 'world' });
  });
  
  websockets.on('connection', (socket) => {
    socket.on('join', (ack) => {
      ack('ok');
      console.log('user joined');
      socket.broadcast.emit('joined');
    });
  });

})();

async function createRedisAdapter() {
  const pub = createClient({ url: "redis://redis:6379" });
  const sub = pub.duplicate();
  await Promise.all([pub.connect(), sub.connect()]);
  return createAdapter(pub, sub);
}
