<?php
	session_start();
	include("../src/settings.php");

	$_SESSION[$usernameSessionKey] = $_GET["username"];
	
	echo 'Username: ' . $_SESSION[$usernameSessionKey];
?>
<form action="../src/GetSLT.php" method="post">

	<input type="submit" value="Submit"/>
	
</form> 