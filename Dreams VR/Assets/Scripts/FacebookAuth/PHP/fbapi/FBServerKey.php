<?php

if(!isset($_GET['log']))
{
	error_reporting(0);
}
else
{
	error_reporting(E_ALL);
}
header("Cache-Control: no-store, no-cache, must-revalidate, max-age=0");
header("Cache-Control: post-check=0, pre-check=0", false);
header("Pragma: no-cache");

define(SERVER_KEY, "rjproz_secret_1990"); // Change this

function ValidatePublicKey($key)
{
   return 1;
	/* Implement you own logic to check serverkey */
	if(strcmp($key , SERVER_KEY) == 0 )
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

?>