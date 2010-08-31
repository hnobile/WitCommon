<?php
	class SSOManager {

		var $username;
		var $siteId;
		var $sslcert;
		var $sslkey;
		var $cainfo;
		var $lc; // location

		var $passportUrl = "https://ppsacredential.service.passport.net/pksecure/PPSACredentialPK.srf";
		var $urls = Array(
			"outlooklive"=>"https:%2F%2Foutlook.com%2Fowa%2F",
			"officelive" => "http:%2F%2Fworkspace.office.live.com",
			"skydrive" => "http:%2F%2Fskydrive.live.com%2Fhome.aspx%3Fprovision%3D1",
			"spaces" => "http:%2F%2Fspaces.live.com%2F%3Flc%3D1049",
			"home" => "http:%2F%2Fhome.live.com%2Fdefault.aspx%3Fwa%3Dwsignin1.0%26lc%3D1049"
		);

		public function SSOManager($_siteId, $_sslcert, $_sslkey, $_lc)
		{
			$this->siteId = $_siteId;
			$this->sslcert = $_sslcert;
			$this->sslkey = $_sslkey;
			$this->lc = $_lc;
		}

		public function getSLT($_service, $_username, $_loginSeconds)
		{
			return $this->ServiceUrl($_service) . "&slt=" . $this->Request($_username, $_loginSeconds);
		}
		
		private function ServiceUrl($serv)
		{
			$sec="MBI";
			if($serv=="outlooklive") $sec.="_SSL";
			$template="https://login.live.com/ppsecure/post.srf?wa=wsignin1.0&rpsnv=10&ct=".time()."&rver=5.5.4177.0&wp=$sec&lc=".$this->lc.
			"&wreply=".$this->urls[$serv];

			return $template;
		}

		private function Request($_username, $_loginSeconds) 
		{
			$slt = '';

			$data =         "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
			$data = $data . "   <soap:Header>";
			$data = $data . "     <WSSecurityHeader xmlns=\"http://schemas.microsoft.com/Passport/SoapServices/CredentialServiceAPI/V1\">";
			$data = $data . "         <version>eshHeader25</version>";
			$data = $data . "         <ppSoapHeader25><![CDATA[<s:ppSoapHeader xmlns:s=\"http://schemas.microsoft.com/Passport/SoapServices/SoapHeader\" version=\"1.0\"><s:lcid>1033</s:lcid><s:sitetoken><t:siteheader xmlns:t=\"http://schemas.microsoft.com/Passport/SiteToken\" id=\"" . $this->siteId . "\"/></s:sitetoken></s:ppSoapHeader>]]></ppSoapHeader25>";
			$data = $data . "     </WSSecurityHeader>";
			$data = $data . "   </soap:Header>";
			$data = $data . "   <soap:Body>";
			$data = $data . "      <GetSLT xmlns=\"http://schemas.microsoft.com/Passport/SoapServices/CredentialServiceAPI/V1\">";
			$data = $data . "         <PassIDIn>";
			$data = $data . "            <pit>PASSID_SIGNINNAME</pit>";
			$data = $data . "            <bstrID>" . $_username . "</bstrID>";
			$data = $data . "         </PassIDIn>";
			$data = $data . "         <LoginSeconds>" . $_loginSeconds . "</LoginSeconds>";
			$data = $data . "      </GetSLT>";
			$data = $data . "   </soap:Body>";
			$data = $data . "</soap:Envelope>";

			$tuCurl = curl_init();
			curl_setopt($tuCurl, CURLOPT_URL, $this->passportUrl);
			curl_setopt($tuCurl, CURLOPT_PORT , 443);
			curl_setopt($tuCurl, CURLOPT_VERBOSE, 1);
			curl_setopt($tuCurl, CURLOPT_HEADER, 1);
			curl_setopt($tuCurl, CURLOPT_SSLVERSION, 3);
			curl_setopt($tuCurl, CURLOPT_SSLCERT, $this->sslcert);
			curl_setopt($tuCurl, CURLOPT_SSLKEY, $this->sslkey);
		    curl_setopt($tuCurl, CURLOPT_SSL_VERIFYPEER, FALSE);
		    curl_setopt($tuCurl, CURLOPT_SSL_VERIFYHOST, FALSE);
			curl_setopt($tuCurl, CURLOPT_POST, 1);
			curl_setopt($tuCurl, CURLOPT_POSTFIELDS, $data);
			curl_setopt($tuCurl, CURLOPT_RETURNTRANSFER, 1);
			curl_setopt($tuCurl, CURLOPT_HTTPHEADER, Array("Content-Type: text/xml","SOAPAction: \"#GetSLT\"", "Content-length: ".strlen($data)));
			$xml = curl_exec($tuCurl);

			curl_close($tuCurl);

			if (  preg_match( "/<SLT>(.+)<\/SLT>/", $xml, $match_array ) == 1 ) {
	    		$slt = $match_array[1];
	  		}
			return $slt;
		}
	}
?>