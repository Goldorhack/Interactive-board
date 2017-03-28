<?php

$msg = "vide";



if(isset($_GET["msg"]))
{
	if("up"==$_GET["msg"])
	{
		$msg = "up";
	}
	else if("down"==$_GET["msg"])
	{
		$msg = "down";
	}
}



$cmd = 'sudo /usr/bin/python2.7 /home/pi/Desktop/AZURIL/hat/hatSay.py '.$msg.' 2>&1';


ob_start();
passthru($cmd);
$output = ob_get_clean(); 


