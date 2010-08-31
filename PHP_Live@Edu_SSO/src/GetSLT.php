<?php
	// start output buffering
	ob_start();
	session_start();
?>

<?php
	require_once "settings.php";
	require_once "SSOManager.class.php";
?>
<head>
<meta HTTP-EQUIV="Pragma" CONTENT="no-cache" />
<title>Redirecting...</title>
</head>
<body>

<?php

	$ssoManager = new SSOManager($siteid, $cer, $private, $lc);
	
	$ssoURL = $ssoManager->getSLT($service, $_SESSION[$usernameSessionKey], $loginSeconds);
	
	header("location:$ssoURL");
	
?>
</body>
<?php
	exit();	
	ob_end_flush();
?>