const express = require('express');
const cors = require('cors');
const app = express();
app.use(cors({origin: "http://localhost:8080"}));
app.get('/hello', (req, res) => res.json({ hello: 'world' }));
app.listen(3000, () => console.log('Listening on port 3000'));
