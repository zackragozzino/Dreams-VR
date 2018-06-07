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


?>
<!-- Customize the Page Design here * -->
<h1>Now go back to Game..</h1>

<script>
	function loaded()
	{
    	window.setTimeout(CloseMe, 5000);
	}

	function CloseMe() 
	{
   	 	window.close();
	}
	loaded();
</script>