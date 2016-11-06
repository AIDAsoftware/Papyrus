const express = require('express');
const path = require('path');

const app = express();

app.use('/', express.static('.'));

const port = 8080;

app.listen(port, (err) => {
  if (err) {
    console.log(err);
    return;
  }

  console.log(' --------------------------------------');
  console.log(`    Local: http://0.0.0.0:${port}`);
  console.log(' --------------------------------------');
});
