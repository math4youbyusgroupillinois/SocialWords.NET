using System.Windows.Forms;
using System.Drawing;
using System.Net;
using mshtml;
using System.Reflection;
using System.Runtime.InteropServices;
using System;
using System.Drawing.Drawing2D;

using System.IO;
using System.Threading;

namespace SocialWords.Web
{

    /// <summary>
    /// Thanks for the solution to the "sometimes not painting sites to Piers Lawson
    /// Who performed some extensive research regarding the origianl implementation.
    /// You can find his codeproject profile here:
    /// http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=39324
    /// </summary>
    [InterfaceType(1)]
    [Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLElementRender2
    {
        void DrawToDC(IntPtr hdc);
        void SetDocumentPrinter(string bstrPrinterName, ref _RemotableHandle hdc);
    }

    /// <summary>
    /// Code by Adam Najmanowicz
    /// http://www.codeproject.com/script/Membership/Profiles.aspx?mid=923432
    /// http://blog.najmanowicz.com/
    /// 
    /// Some improvements suggested by Frank Herget
    /// http://www.artviper.net/
    /// </summary>
    public class WebPageBitmap
    {
        private WebBrowser webBrowser;
        private string url;
        private string word;
        private int width;
        private int height;
        private bool isOk;
        private string errorMessage;
        private ShareMode mode;
        private String webroot;

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        public bool IsOk
        {
            get { return isOk; }
            set { isOk = value; }
        }

        public WebPageBitmap(String word, ShareMode mode, String path, int width = 400, int height = 209, int wait = 0)
        {
            //            url = "http://pearson.lexicum.net/"
            //            url = "images/" + word;
            this.word = word;
            //this.url = "http://localhost:37829/Render/Q?id=" + word;
            //this.url = "http://192.168.10.38/SocialWords.Web/Render/Q?id=" + word;
            //this.url = "http://localhost/test/";

            if (Config.TestMode)
                this.url = path + "/Render/T?id=" + word;
            else
                this.url = path + "/Render/Q?id=" + word;
            this.webroot = path;

            this.width = width;
            this.height = height;
            this.mode = mode;

            try
            // needed as the script throws an exeception if the host is not found
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.url);
                req.AllowAutoRedirect = true;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; .NET CLR 3.5.21022; .NET CLR 1.0.3705; .NET CLR 1.1.4322)";
                req.Referer = "http://lexicum.net";
                req.ContentType = "text/html";
                req.Accept = "*/*";
                req.KeepAlive = false;

                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    string x = resp.StatusDescription;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isOk = false;
                return;
            }
            isOk = true;                                                      // public, to check in program.cs if the domain is found, so the image can be saved

            webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted +=
            new WebBrowserDocumentCompletedEventHandler(documentCompletedEventHandler);
            webBrowser.Size = new Size(width, height);
            webBrowser.ScrollBarsEnabled = false;
        }

        /// <summary>
        /// Fetches the image 
        /// </summary>
        /// <returns>true is the operation ended with a success</returns>
        public bool Fetch()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = true;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; .NET CLR 3.5.21022; .NET CLR 1.0.3705; .NET CLR 1.1.4322)";
            req.Referer = "http://www.cognifide.com";
            req.ContentType = "text/html";
            req.AllowWriteStreamBuffering = true;
            req.AutomaticDecompression = DecompressionMethods.GZip;
            req.Method = "GET";
            req.Proxy = null;
            req.ReadWriteTimeout = 20;

            HttpStatusCode status;
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                status = resp.StatusCode;
            }

            if (status == HttpStatusCode.OK || status == HttpStatusCode.Moved)
            {
                webBrowser.Navigate(url);
                while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void documentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Document.Window.Error +=
                new HtmlElementErrorEventHandler(SuppressScriptErrorsHandler);
        }

        public void SuppressScriptErrorsHandler(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("Error!");
        }

        internal Bitmap GetBitmap(int thumbwidth, int thumbheight)
        {
            IHTMLDocument2 rawDoc = (IHTMLDocument2)webBrowser.Document.DomDocument;
            IHTMLElement rawBody = rawDoc.body;
            IHTMLElementRender2 render = (IHTMLElementRender2)rawBody;

            Bitmap bitmap = new Bitmap(width, height);
            Rectangle bitmapRect = new Rectangle(0, 0, width, height);

            // Interesting thing that despite using the renderer later 
            // this following line is still necessary or 
            // the background may not be painted on some websites.
            webBrowser.DrawToBitmap(bitmap, bitmapRect);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                IntPtr graphicshdc = graphics.GetHdc();
                render.DrawToDC(graphicshdc);

                graphics.ReleaseHdc(graphicshdc);
                graphics.Dispose();

                if (thumbheight == height && thumbwidth == width)
                {
                    return bitmap;
                }
                else
                {
                    Bitmap thumbnail = new Bitmap(thumbwidth, thumbheight);
                    using (Graphics gfx = Graphics.FromImage(thumbnail))
                    {
                        // high quality image sizing
                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;                                                                       // make it look pretty 
                        gfx.DrawImage(bitmap, new Rectangle(0, 0, thumbwidth, thumbheight), bitmapRect, GraphicsUnit.Pixel);
                    }
                    bitmap.Dispose();
                    return thumbnail;
                }
            }
        }

        public String SaveBitmap(int thumbwidth = 400, int thumbheight = 209)
        {
            try
            {
                String loc = GetFilename(word);
                Bitmap image = null;

                WebPageBitmap wpb = new WebPageBitmap(word, mode, webroot);
                if (!wpb.Fetch())
                {
                    throw new ApplicationException("Cannot generate image");
                }
                image = wpb.GetBitmap(thumbwidth, thumbheight);
                image.Save(System.IO.Path.Combine(Config.SaveFolder, loc));
                return loc;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Saving bitmap failed", e);
            }
        }

        public String GetFilename(String word)
        {
            return GetFilename(word, mode);
        }

        public static String GetFilename(String word, ShareMode mode)
        {
            switch(mode)// word has to be clean
            {
                case ShareMode.Definition:
                    return "d-" + word + ".png"; 

                case ShareMode.Question:
                    return "q-" + word + ".png";
 
                default:
                    throw new ApplicationException("Operation not definied");
            }
        }

        // lazy get - create only if not existing
        public static void InitBitmap(String word, ShareMode mode, String path)
        {

            if (!File.Exists(Path.Combine(Config.SaveFolder, GetFilename(word, mode))))
            {
                WebPageBitmap wpb = new WebPageBitmap(word, mode, path);
                wpb.SaveBitmap();
            }
            return;
        }
        public static void LoadBitmapSTA(String word, String path)
        {
            Thread thread = new Thread(() => WebPageBitmap.InitBitmap(word, ShareMode.Question, path)); // make sure bitmap exists
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
