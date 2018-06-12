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

require_once 'src/autoload.php';
require_once 'FBServerKey.php';

if(isset($_GET['statsadmin']))
{
	$cache = phpFastCache();
	print_r( $cache->stats() );
	die;
}
$group = explode('$',$_GET['process']);
$method = $group[0];
$extraparam = $group[1];
if($method == "login")
{
	
	if(isset($_GET['gettoken']))
	{
		header('Content-type:application/json;charset=utf-8');
		$server_Key = $_GET['serverKey'];
		if(ValidatePublicKey( $server_Key) == 0)
		{
			echo "{\"iserror\":true,\"error_description\":\"Mismatched server key. Upload PHP files on your own servers and set server key. \"}";
			die;
		}
		$id  = $_GET['gettoken'];
		$cache = phpFastCache();
		$data = "";
		$time = time();
		do
		{
			$data = $cache->get($id );	
			if((time() - $time) > 60)
			{

				echo "{\"iserror\":true,\"error_description\":\"Timeout reached!\"}";
				die;
			}
			sleep(2);
		}
		while ($data == false);

		echo json_encode($data);
		$cache->delete($id);

		die;
	}



	$data = array(); 

	$id = $_GET['state'];
	if(isset($_GET['error']))
	{
		$data['iserror'] = true; 

		$reason = $_GET['error_reason'];
		if($reason == "user_denied")
		{
			$data['error_description'] = "User Cancelled the process";
		}
		else
		{
			$data['error_description'] = str_replace("+", " ", $_GET['error_description']);
		}
	}
	else
	{

		$code = $_GET['code'];
		
		$data['iserror'] = false; 
		$data['code'] = $code;
		$data['id'] = $id;
	}

	$cache = phpFastCache();
	$cache->set($id ,$data,120 );
}
else if($method == "share")
{
	//echo "Sharingg";
	$id = $extraparam.'_share';

	if(isset($_GET['getstatus']))
	{
		header('Content-type:application/json;charset=utf-8');
		
		$cache = phpFastCache();
		$data = "";
		$time = time();
		do
		{
			if((time() - $time) > 120)
			{

				echo "{\"iserror\":true,\"error_description\":\"Timeout reached!\"}";
				die;
			}
			sleep(2);
			$data = $cache->get($id );	
			
		}
		while ($data == false);

		echo json_encode($data);
		$cache->delete($id);
		die;
	}
	else
	{

		$data = array(); 
		if(isset($_GET['error_code']))
		{
			$data['iserror'] = true; 
			$data['error_code'] = $_GET['error_code'];
			$data['error_message'] = $_GET['error_message'];
		}
		else
		{
			$data['iserror'] = false; 
			$data['post_id'] = $_GET['post_id'];
		}
		$cache = phpFastCache();
		$cache->set($id ,$data,240 );
		// print_r($data);
		// echo "<br/>";
		// echo $id ;
		// echo "<br/>";
		header('Location: FBPostLogin.php');
		die;
	}
	//[process] => share [post_id] => 1079983808735604
	// ( [process] => share [error_code] => 4201 [error_message] => User canceled the Dialog flow )
	
	
}
//print_r($_POST);
//print_r($_GET);
header('Location: FBPostLogin.php');
?>