<?

$usernameSessionKey="username_key"; // Session key where the username is stored

$service="outlooklive"; // Service to redirect. Can be: 	outlooklive / officelive / skydrive / spaces / home
$loginSeconds="30";
$lc="11274"; // Windows Live Location.  Enter to www.hotmail.com and see the lc value in query string. ES-AR: 11274. EN-US: 1033.

$siteid="272756"; // Site ID received from ed-desk
// for security reasons, place this files outside public folder
$cer=getcwd() . "/../certificates/cer.pem"; // Certificate File
$private=getcwd() . "/../certificates/private.pem"; // Private Key File

?>