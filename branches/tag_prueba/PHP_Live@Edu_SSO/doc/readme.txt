PHP Live@Edu SSO
----------------

1- Enable SSO: 

You must request SSO support from Live@Edu's web interface and you will receive an e-mail containing a link from where you can download the certificate (.pfx), another e-mail containing a code to enter in the previous link and the siteId.

2- Generate Private Key and Certificates:

a- Rename your pfx certificate to cer.pfx and convert your pfx file to a pem certificate using "generate-pem.bat". Alternatively, you can use the following command: "openssl pkcs12 -in yourcertificatefile.pfx -out all.pem -nodes", where "yourcertificatefile.pfx" is the name of your certificate file. In any case, when propted for a password press enter leaving the password blank.

b- Copy the resulting file to seperate files, cer.pem and private.pem

c- Edit private.pem to look like this (contains only the private key)

	Bag Attributes
	localKeyID: 01 00 00 00
	friendlyName: {CA7DB1AD-1EAD-47DD-A141-696CDAA7586A}
	Microsoft CSP Name: Microsoft RSA SChannel Cryptographic Provider
	Key Attributes
	X509v3 Key Usage: 10 
	-BEGIN RSA PRIVATE KEY-----
	... 
	-END RSA PRIVATE KEY-----

d- Edit cer.pem to look like this (contains only the user certificate)

	Bag Attributes
	localKeyID: 01 00 00 00
	friendlyName: school.com
	subject=/C=US/ST=WA/L=Redmond/O=wledutraining.com/OU=EDU/CN=sapipartner.com
	issuer=/DC=com/DC=microsoft/DC=corp/DC=redmond/CN=Microsoft Secure Server A 
	-BEGIN CERTIFICATE-----
	... 
	-END CERTIFICATE-----

3- Configure settings.php to reflect the desired parameters.