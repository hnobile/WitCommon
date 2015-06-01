////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                      MbCompression    version: 1.0.1.5                                                //
//	                  Written by: Miron Abramson. Date: 04-10-07                                              //
//                             Last update: 05-05-08			                                              //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////


Implementation:
----------------
To use the compression modules & handlers, first you need to drop the compiled MbCompression.dll file in your 
Bin folder.

Javascript compression handler:
`````````````````````````````````
add the line:
<add verb="*" path="jslib.axd" type="Miron.Web.MbCompression.JavaScriptCompressionHandler" validate="false"/>
to the <httpHandlers> section in your web.config.
Use the following method to register your script files (instead of adding them into your page header in the aspx file):

/// <summary>
/// Adds a JavaScript reference to the HTML head tag.
/// Usualy this method will be place in our BasePage
/// </summary>
protected virtual void AddJavaScriptInclude(string path)
{
    HtmlGenericControl script = new HtmlGenericControl("script");
    script.Attributes["type"] = "text/javascript";

    // Change the 'src' to jslib.axd file, but keep the releative directory (for relative urls in the js file)
    script.Attributes["src"] = ResolveUrl(path).Replace(System.IO.Path.GetFileName(path), "jslib.axd?d=" + Server.UrlEncode(path));
    Page.Header.Controls.Add(script);
}
For example,
To add the Javascript file /Scripts/utils.js to your page use:
AddJavaScriptInclude("~/Scripts/utils.js");   // Note that it must be absolute position start with '~'

Or, alternative way to register the file  /Scripts/utils.js this way:
<script type="text/javascript" src="Scripts/jslib.axd?d=~/Scripts/utils.js">
(Directly in your .aspx file)


Css compression handler:
````````````````````````
add the line:
<add verb="*" path="css.axd" type="Miron.Web.MbCompression.CssCompressionHandler" validate="false"/>
to the <httpHandlers> section in your web.config.
Add your css files to header as usual, but with absolute position starting with '~', 
and run the following method on the OnPreRender event:

/// <summary>
/// Chande the path for all the CSS files included in the header to css.axd,
/// so the handler will compress them
/// Usualy this method will be place in our BasePage
/// </summary>
protected virtual void PrepareToCompressCSS()
{
    for (int i = 0; i < Page.Header.Controls.Count; i++)
    {
            HtmlLink link = Page.Header.Controls[i] as HtmlLink;
            if (link != null && link.Attributes["type"] != null && link.Attributes["type"].Equals("text/css", StringComparison.OrdinalIgnoreCase) && link.Attributes["href"].StartsWith("~"))
            {
                // Change the 'href' to css.axd file, but keep the releative directory (for relative urls in the css file)
                link.Attributes["href"] = ResolveUrl(link.Attributes["href"]).Replace(System.IO.Path.GetFileName(link.Attributes["href"]), "css.axd?d=" + Server.UrlEncode(link.Attributes["href"]));
            }
    }
}

To add the stylesheet file /styles/style.css to your page, 
add <link type="text/css" href="~/styles/style.css" rel="stylesheet" /> to your header, and run the above method in the 
OnPreRender event.

Or, alternative way to register the file  /styles/style.css this way:
<link type="text/css" href="styles/css.axd?d=~/styles/style.css" rel="stylesheet" />

Note that if you use Themes (via Web.config or page directive) you don't need to change nothing, only to run the 
method PrepareToCompressCSS().


Page compression module
````````````````````````
Just add the following line to the <httpModules> section in your web.config.
<add name="PageCompressionModule" type="Miron.Web.MbCompression.PageCompressionModule"/>
So simple.

Microsoft Ajax Compression module
`````````````````````````````````
Just add the following line to the <httpModules> section in your web.config.
<add name="MicrosoftAjaxCompressionModule" type="Miron.Web.MbCompression.MicrosoftAjaxCompressionModule"/>
So simple.

WebResourcce.axd compresion module
``````````````````````````````````
Just add the following line to the <httpModules> section in your web.config.
<add name="WebResourceCompressionModule" type="Miron.Web.MbCompression.WebResourceCompressionModule"/>
Note that if you are using a share hosting that not allow reflection within your code (such GoDaddy), 
You must add the attribute:  reflectionAlloweded="false" to the CompressorSettings section.

Using the extra features:
`````````````````````````
Add the section
<sectionGroup name="Miron.web">
  <section name="CompressorSettings" type="Miron.Web.MbCompression.SettingsConfigSection"/>
</sectionGroup>
into the configSections in your Web.Config.

Add the following section right after the </system.web>:

<Miron.web>
    <CompressorSettings reflectionAlloweded="Boolen" daysInCahe="Int" optimizeHtml="Boolen" compressCSS="Boolen" compressJavaScript="Boolen" compressPage="Boolen" compressWebResource="Boolen" compressThirdParityScripts="Boolean">
      <excludeTypes>
        <add key="" />
      </excludeTypes>
      <excludePaths>
        <add key="" />
      </excludePaths>
    </CompressorSettings>
</Miron.web>

For example, if you want the compressor to ignore pages that generate content from mime type 'gif' of 'jpeg', and ignore 
the specified path "~/DontCompressMe.aspx", but remove white spaces, compress all the rest and put it on the cache for 30 days, and your site
is in share hosting that not allow reflection use the follownig:

<Miron.web>
    <CompressorSettings reflectionAlloweded="false" daysInCahe="30" optimizeHtml="true" compressCSS="true" compressJavaScript="true" compressPage="true" compressWebResource="true" compressThirdParityScripts="false">
      <excludeTypes>
        <add key="image/gif" />
        <add key="image/jpeg" />
      </excludeTypes>
      <excludePaths>
        <add key="~/DontCompressMe.aspx" />
      </excludePaths>
    </CompressorSettings>
</Miron.web>

The attribute 'compressThirdParityScripts' is usuful when third party controls are used and you cann't modify the scripts url's.
For example, if using 'Telerik' (RadControls) controls and you set them to don't use webresources, they inject their scripts url into your page and you can not
modify tham as described in the Javascript compression handler section above, so setting this attribute to true will modify this urls, 
so they will be compressed in the Javascript compression (if compressJavaScript also was set to true)