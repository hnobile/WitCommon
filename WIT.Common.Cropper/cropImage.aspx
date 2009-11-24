<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Web.UI.HtmlControls" %>
<%@ import Namespace = "System.Collections" %>
<%@ import Namespace = "System.IO" %>
<script language="c#" runat="server">
    private const string SCRIPT_TEMPLATE = "<" + "script " + "type=\"text/javascript\">window.parent.photoUploadComplete('{0}', {1});" + "<" + "/script" + ">";
	private const string SCRIPT_ALERT = "<" + "script " + "type=\"text/javascript\">alert('{0}');" + "<" + "/script" + ">";
	private const string ImageUrlPath = "/cms/tempFiles/";
	
	public int WrapImageWidth = 0;
	private string ImageFileName {
		get { 
			if (Session["imageFileName"].ToString() != null) {
				return Session["imageFileName"].ToString();
			} else {
				return String.Empty;
			}
		}

		set { Session["imageFileName"] = value;}
	}
	void Page_Load(object sender, EventArgs e) {
	
		if (!IsPostBack) {
			ImagesCrop.Style.Add("display","none");
		} else {
			UploadPhoto();
		}
	}

    private void UploadPhoto()
    {
        string script = string.Empty;

        if ((filPhoto.PostedFile != null) && (filPhoto.PostedFile.ContentLength > 0))
        {
            if (!IsValidImageFile(filPhoto))
            {
                script = string.Format(SCRIPT_TEMPLATE, "The uploaded file is not a valid image file.", "true");
            }
        }
        else
        {
            script = string.Format(SCRIPT_TEMPLATE, "Please specify a valid file.", "true");
        }

        if (script == null || script == "")
        {

           	string fn = System.IO.Path.GetFileName(filPhoto.PostedFile.FileName);
			string SaveLocation = @Server.MapPath(ImageUrlPath) +  fn;
			try
			{
				filPhoto.PostedFile.SaveAs(SaveLocation);
				script = string.Format(SCRIPT_TEMPLATE, "Photo uploaded.", "false");
				ImageFileName = filPhoto.PostedFile.FileName;

				string Path = ImageUrlPath + ImageFileName;
				Bitmap bmp = new Bitmap(@Server.MapPath(Path));
				int width = bmp.Width;
				int height = bmp.Height;
				
				bmp.Dispose();
				if (width > 800) {
					/*  Si el ancho es mayor a 800, tengo que achicarla proporcionalmente */
					double proporcion = (double)width / (double)800;
					width = 800;
					
					//En realidad esto va a quedar 600 siempre, si la suben en 4:3
					//Pero todavía no me interesa que el ratio sea 4:3, entonces:
					height = Convert.ToInt32(height / proporcion); 
					
					if (height < 480) {
						throw new Exception("This image is too small. Please, select another one.");
					}
					ResizeImage(width,height);
				}
				
				ImageToCrop.Height = height;
				ImageToCrop.Width = width;
				WrapImageWidth = width;
				
				ImagesCrop.Style.Add("display","block");
				UploadImageForm.Style.Add("display","none");
				ImageToCrop.ImageUrl = ImageUrlPath + filPhoto.PostedFile.FileName;
			}
			catch ( Exception ex )
			{
				script = string.Format(SCRIPT_ALERT, "An error has ocurred: " + ex.Message);
			}
            
        }

        Page.RegisterStartupScript("uploadNotify", script);
    }

    private static bool IsValidImageFile(HtmlInputFile file)
    {
        try
        {
            using (Bitmap bmp = new Bitmap(file.PostedFile.InputStream))
            {
                return true;
            }
        }
        catch (ArgumentException)
        {
            //throws exception if not valid image
        }

        return false;
    }
	
	protected void btnBack_Click(object sender, EventArgs e) 
	{
		Response.Redirect("cropImage.aspx");
	}
	
	protected void btnCrop_Click(object sender, EventArgs e) 
	{
		string Path = ImageUrlPath + ImageFileName;
		CropImage(Convert.ToInt32(x1.Value), Convert.ToInt32(x2.Value), Convert.ToInt32(y1.Value), Convert.ToInt32(y2.Value), Convert.ToInt32(width.Value), Convert.ToInt32(height.Value), Path);
		
		Bitmap _bmp = new Bitmap(@Server.MapPath(Path));
		string header = String.Format("attachment;filename={0}",ImageFileName);
		Response.Clear();
		Response.AddHeader("Content-Disposition",header);
		Response.ContentType = "image/Jpeg";
		_bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
		_bmp.Dispose();
		Response.End();
		
		//Borrar la Imagen.
	}

	void CropImage(int x1, int x2, int y1, int y2, int width, int height, string Path) {

		// La variable Path, contiene el Path relativo, tipo: /cms/tempFiles/img.jpg, para abrirla como bitmap, es:
		Bitmap _bmp = new Bitmap(@Server.MapPath(Path));
		
		Rectangle cropTangle = new Rectangle();
		cropTangle.X = x1;
		cropTangle.Y = y1;
		cropTangle.Width = width;
		cropTangle.Height = height;
		
		Bitmap bmpCrop = _bmp.Clone(cropTangle,_bmp.PixelFormat);
		
		_bmp.Dispose();
		bmpCrop.Save(@Server.MapPath(Path),ImageFormat.Jpeg);
		bmpCrop.Dispose();
	}
	
	void ResizeImage(int width, int height) {
		string Path = ImageUrlPath + ImageFileName;
		
		Bitmap b = new Bitmap(@Server.MapPath(Path));
		Bitmap result = new Bitmap( width, height );
		using( Graphics g = Graphics.FromImage( (System.Drawing.Image) result ) )
			g.DrawImage( b, 0, 0, width, height );
		b.Dispose();
		try {
			result.Save(@Server.MapPath(Path),ImageFormat.Jpeg);
		} catch (Exception ex) {
			throw new Exception();
		}
		result.Dispose();
	}
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<meta http-equiv="Content-type" content="text/html; charset=utf-8" />
	<title>Crop Images</title>
	<script src="../js/prototype.js" type="text/javascript"></script>
	<script src="../js/scriptaculous.js" type="text/javascript"></script>
	<script src="../js/cropper.js" type="text/javascript"></script>
	<script type="text/javascript" charset="utf-8">
	
		Event.observe(
			window,
			'load',
			function() {
				new Cropper.ImgWithPreview(
					'ImageToCrop',
					{ 
						previewWrap: 'previewWrap',
						minWidth: 640,
						minHeight: 480,
						//Ratio 4x3.
						ratioDim: { x: 640, y: 480 }, 
						//640 and 480, are default height and width.
						displayOnInit: true,
						onEndCrop: function( coords, dimensions ) {
							$( 'x1' ).value = coords.x1;
							$( 'y1' ).value = coords.y1;
							$( 'x2' ).value = coords.x2;
							$( 'y2' ).value = coords.y2;
							$( 'width' ).value = dimensions.width;
							$( 'height' ).value = dimensions.height;
						}
					}
				);
			}
		);		

/*   JavaScript Photo Uploading */
        var PROGRESS_INTERVAL = 500;
        var PROGRESS_COLOR = '#000080';

        var _divFrame;
        var _divUploadMessage;
        var _divUploadProgress;

        var _loopCounter = 1;
        var _maxLoop = 10;
        var _photoUploadProgressTimer;

        function initPhotoUpload()
        {
            _divFrame = document.getElementById('divFrame');
            _divUploadMessage = document.getElementById('divUploadMessage');
            _divUploadProgress = document.getElementById('divUploadProgress');

            var btnUpload = document.getElementById('btnUpload');

            btnUpload.onclick = function(event)
            {
                var filPhoto = document.getElementById('filPhoto');

                //Baisic validation for Photo
                _divUploadMessage.style.display = 'none';

                if (filPhoto.value.length == 0)
                {
                    _divUploadMessage.innerHTML = '<span style=\"color:#ff0000\">Please specify the file.</span>';
                    _divUploadMessage.style.display = '';
                    filPhoto.focus();
                    return;
                }

                var regExp = /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.gif|.GIF|.png|.PNG|.bmp|.BMP)$/;

                if (regExp.test(filPhoto.value)) //Somehow the expression does not work in Opera
                {
                    _divUploadMessage.innerHTML = '<span style=\"color:#ff0000\">Invalid file type. Only supports jpg, gif, png and bmp.</span>';
                    _divUploadMessage.style.display = '';
                    filPhoto.focus();
                    return;
                }

                beginPhotoUploadProgress();
                document.getElementById('photoUpload').submit();
                _divFrame.style.display = 'none';
            }
        }

        function beginPhotoUploadProgress()
        {
            _divUploadProgress.style.display = '';
            clearPhotoUploadProgress();
            _photoUploadProgressTimer = setTimeout(updatePhotoUploadProgress, PROGRESS_INTERVAL);
        }

        function clearPhotoUploadProgress()
        {
            for (var i = 1; i <= _maxLoop; i++)
            {
                document.getElementById('tdProgress' + i).style.backgroundColor = 'transparent';
            }

            document.getElementById('tdProgress1').style.backgroundColor = PROGRESS_COLOR;
            _loopCounter = 1;
        }

        function updatePhotoUploadProgress()
        {
            _loopCounter += 1;

            if (_loopCounter <= _maxLoop)
            {
                document.getElementById('tdProgress' + _loopCounter).style.backgroundColor = PROGRESS_COLOR;
            }
            else 
            {
                clearPhotoUploadProgress();
            }

            if (_photoUploadProgressTimer)
            {
                clearTimeout(_photoUploadProgressTimer);
            }

            _photoUploadProgressTimer = setTimeout(updatePhotoUploadProgress, PROGRESS_INTERVAL);
        }

        function photoUploadComplete(message, isError)
        {
            clearPhotoUploadProgress();

            if (_photoUploadProgressTimer)
            {
                clearTimeout(_photoUploadProgressTimer);
            }

			if (_divUploadProgress) {
            _divUploadProgress.style.display = 'none';
            _divUploadMessage.style.display = 'none';
            _divFrame.style.display = '';
			}
			if (_divUploadMessage) {
	            if (message.length)
	            {
	                var color = (isError) ? '#ff0000' : '#008000';

	                _divUploadMessage.innerHTML = '<span style=\"color:' + color + '\;font-weight:bold">' + message + '</span>';
	                _divUploadMessage.style.display = '';

	                if (isError)
	                {
	                    document.getElementById('filPhoto').focus();
	                }
	            }
			}
        }
    </script>
	<style type="text/css">
		label { 
			clear: left;
			margin-left: 50px;
			float: left;
			width: 5em;
		}
		
		html, body { 
			margin: 0;
		}
		
		#ImagesCrop {
			width: 1600px;
			background-color: #eee;
			border: 3px solid #000;
			margin: 10px;
			padding: 5px;
		}

		#ImageForm {
		  width:400px;
		  margin-left: auto;
		  margin-right: auto;
		}
		
		#Wrap {
			width:<%=WrapImageWidth%>px;
			float:left;
		}
		
		#Buttons {
			margin-bottom: 20px;
		}
		
		#Buttons input{
			margin-right: 20px;
		}

		#previewOuterWrap {
			float: right;
			margin-left:20px;
		}

		#previewWrap {
			border: 1px solid #333;
		}

	</style>
</head>
<body onLoad="initPhotoUpload();">
<form id="photoUpload" enctype="multipart/form-data" runat="server">
<div id="UploadImageForm" runat="server">
        <div id="ImageForm">
            <fieldset>
                <legend>Photo Upload</legend>
                <div id="divFrame">
					    <div>
					        <input id="filPhoto" type="file" runat="server"/>
					    </div>
					    <div id="divUpload" style="padding-top:4px">
					        <input id="btnUpload" type="button" value="Upload Photo" />
					    </div>
                </div>
                <div id="divUploadMessage" style="padding-top:4px;display:none"></div>
                <div id="divUploadProgress" style="padding-top:4px;display:none">
                    <span style="font-size:smaller">Uploading photo...</span>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="2" style="width:100%">
                            <tbody>
                                <tr>
                                    <td id="tdProgress1">&nbsp; &nbsp;</td>
                                    <td id="tdProgress2">&nbsp; &nbsp;</td>
                                    <td id="tdProgress3">&nbsp; &nbsp;</td>
                                    <td id="tdProgress4">&nbsp; &nbsp;</td>
                                    <td id="tdProgress5">&nbsp; &nbsp;</td>
                                    <td id="tdProgress6">&nbsp; &nbsp;</td>
                                    <td id="tdProgress7">&nbsp; &nbsp;</td>
                                    <td id="tdProgress8">&nbsp; &nbsp;</td>
                                    <td id="tdProgress9">&nbsp; &nbsp;</td>
                                    <td id="tdProgress10">&nbsp; &nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </fieldset>
        </div>
</div>
<div id="ImagesCrop" runat="server">
	<h2>Crop Images</h2>
	<div id="Buttons">
		<asp:Button ID="btnBack" Runat="server" OnClick="btnBack_Click" Text="Upload a Photo"/>
		<asp:Button ID="btnCrop" Runat="server" OnClick="btnCrop_Click" Text="Crop"/>		
	</div>
	<div id="Wrap">
		<asp:Image id="ImageToCrop" name="ImageToCrop" runat="server" ImageUrl="" Height="600" Width="800"></asp:Image>
	</div>
	<div id="previewOuterWrap">
		<h2>Crop preview:</h2>
		<div id="previewWrap">a</div>
	</div>
	
	<input type="hidden" name="x1" id="x1" runat="server"/>
	<input type="hidden" name="y1" id="y1" runat="server" />
	<input type="hidden" name="x2" id="x2" runat="server" />
	<input type="hidden" name="y2" id="y2" runat="server" />
	<input type="hidden" name="width" id="width" runat="server" />
	<input type="hidden" name="height" id="height" runat="server" />
</div>
</form>
</body>
</html>