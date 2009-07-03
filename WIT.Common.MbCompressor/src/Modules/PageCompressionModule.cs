///////////////////////////////////////////////////////////////////////
//                      PageCompressionModule                        //
//            Written by: Miron Abramson. Date: 2-10-07              //
//                    Compress http page response                    //
//                  using GZIP or DEFLATE algorithm                  //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.IO.Compression;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;

using Miron.Web.MbCompression.Filters;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// HttpModule to compress the output of System.Web.UI.Page handler using gzip/deflate algorithm,
    /// corresponding to the client browser support.
    /// There is option to exclude specified pages in your project (use it to exclude pages that using
    /// Response.End();), or exclude pages that generate content from specified mime type as "image/jpeg" or "application/ms-excel".
    /// </summary>
    public sealed class PageCompressionModule : IHttpModule
    {
        Settings setting;

        #region IHttpModule Members

        /// <summary>
        /// Release resources used by the module (Nothing realy in our case), but must implement (interface).
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose in our case;
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context"></param>
        void IHttpModule.Init(HttpApplication context)
        {
            setting = Settings.Instance;
            if (setting.CompressPage)
            {
                context.PostReleaseRequestState += new EventHandler(OnPostReleaseRequestState);
            }
        }

        #endregion


        #region  // Page Compression

        /// <summary>
        /// Handles the PostReleaseRequestState event.
        /// </summary>
        /// <param name="sender">The object that raised the event (HttpApplication)</param>
        /// <param name="e">The event data</param>
        void OnPostReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            // This part of the module compress only handlers from type System.Web.UI.Page
            // Other types such JavaScript or CSS files will be compressed in an httpHandelr.
            // Here we check if the current handler if a Page, if so, we compress it.
            // Because there is a problem with async postbacks compression, we check here if the current request if an 'MS AJAX' call.
            // If so, we will not compress it.
            // Important !!! : I didn't check this module with another Ajax frameworks such 'infragistics' or 'SmatClient'.
            // probebly you will have to change the RequestUtil.IsAjaxPostBackRequest method.
            if (app.Context.CurrentHandler is System.Web.UI.Page && !RequestUtil.IsAjaxPostBackRequest(HttpContext.Current))
            {
                // Check if the path is not excluded.
                if (!setting.IsValidPath(app.Request.AppRelativeCurrentExecutionFilePath))
                    return;

                // Check if the mime type is not excluded. (Use to exclude pages that generate specific mime type (such image or Excel...))
                if (!setting.IsValidType(app.Response.ContentType))
                    return;

                bool compressionSupported = RequestUtil.IsCompressionSupported(HttpContext.Current, true);
                if (compressionSupported)
                {
                    // Check if GZIP is supported by the client
                    if (RequestUtil.IsSpecificEncodingSupported(app.Context, Constants.GZIP))
                    {
                        app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
                        ResponseUtil.SetEncodingType(app.Context.Response, Constants.GZIP);
                    }
                    // If GZIP is not supported, so only DEFLATE is.
                    else
                    {
                        app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                        ResponseUtil.SetEncodingType(app.Context.Response, Constants.DEFLATE);
                    }
                }

                // Optimize the response html by removing whire spaces and lines
                if (compressionSupported && setting.OptimizeHtml)
                {
                    app.Response.Filter = new RemoveWhiteSpacesFilterStream(app.Response.Filter);
                }

                // If js compression is supported and the flag of compression third party scripts was set,
                // apply the filter that prepare the html code for the compression
                if (RequestUtil.IsCompressionSupported(HttpContext.Current) && setting.CompressJavaScript && setting.CompressThirdPartyJavaScript)
                    app.Response.Filter = new PrepareScriptsToComnpressionStream(app.Response.Filter);
            }
        }

        #endregion

    }
}