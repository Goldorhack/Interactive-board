<?php



?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
      <link rel="shortcut icon" href="../favicon.ico" />

    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>test_bash.php</title>
    
  </head>
  <body>

  <p>
      <a href="../" >retour</a>
  </p>

<h1>
test_bash.php
</h1>

<p>

Create Date : 2017-01-03<br />
Last Update : 2017-01-03<br />

<?php

date_default_timezone_set('Europe/Paris');

echo date("Y-m-d H:i:s")." <br />";

$cmd = 'sudo /usr/bin/python2.7 /home/pi/Desktop/AZURIL/hat/hatSay.py php 2>&1';

ob_start();
passthru($cmd);
// passthru('ls -lart');
$output = ob_get_clean(); 

echo date("Y-m-d H:i:s")." <br />";

?>

</p>

  </body>
</html>

