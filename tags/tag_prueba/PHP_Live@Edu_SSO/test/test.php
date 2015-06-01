<?php
	session_start();
	include("../src/settings.php");

	$error = !isset($_GET["username"]) || empty($_GET["username"]);
	
	echo "Username: ";
	if(!$error) {
		$_SESSION[$usernameSessionKey] = $_GET["username"];
	    echo $_SESSION[$usernameSessionKey];
	}
	else {
		echo "<b>ERROR</b>. Please, set 'username' at query string";
	}
?>
<form action="../src/GetSLT.php" method="post" style="padding-top:0px;">
	<div>
		<p>
			Certificado: <b>
			<?php 
				if(file_exists($cer) && fopen($cer, "r"))
					echo("OK");
				else {
					echo("ERROR");
					$error = true;
				}
			?></b>
			<br/>
			Key privada: <b>
			<?php
				if(file_exists($private) && fopen($private, "r"))
					echo("OK");
				else {
					echo("ERROR");
					$error = true;
				}
			?> </b>
		</p>
	</div>
	
	<?php if(!$error) { ?>
		<input type="submit" value="Login"/>
	<?php } ?>
</form> 