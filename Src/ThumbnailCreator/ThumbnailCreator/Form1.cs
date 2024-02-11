using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using EO.WebBrowser;
namespace ThumbnailCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static string filename = "", innerhtml = "", path = @"thumbnailcreator.txt", openpath = "";
        private void Form1_Shown(object sender, EventArgs e)
        {
            Rearrange();
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            EO.WebBrowser.Runtime.DefaultEngineOptions.SetDefaultBrowserOptions(options);
            EO.WebEngine.Engine.Default.Options.AllowProprietaryMediaFormats();
            EO.WebEngine.Engine.Default.Options.SetDefaultBrowserOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Create(pictureBox1.Handle);
            this.webView1.Engine.Options.AllowProprietaryMediaFormats();
            this.webView1.SetOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Engine.Options.DisableGPU = false;
            this.webView1.Engine.Options.DisableSpellChecker = true;
            this.webView1.Engine.Options.CustomUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            this.webView1.KeyDown += WebView1_KeyDown;
            LoadPage();
            webView1.RegisterJSExtensionFunction("saveDocument", new JSExtInvokeHandler(WebView_JSSaveDocument));
            webView1.RegisterJSExtensionFunction("getFilename", new JSExtInvokeHandler(WebView_JSGetFilename));
        }
        private void WebView1_KeyDown(object sender, EO.Base.UI.WndMsgEventArgs e)
        {
            Keys key = (Keys)e.WParam;
            OnKeyDown(key);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Rearrange();
        }
        private void Rearrange()
        {
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button2.Location = new System.Drawing.Point(60, 0);
            this.button2.Size = new System.Drawing.Size(60, 23);
            this.button5.Location = new System.Drawing.Point(120, 0);
            this.button5.Size = new System.Drawing.Size(60, 23);
            this.button4.Location = new System.Drawing.Point(180, 0);
            this.button4.Size = new System.Drawing.Size(60, 23);
            this.button7.Location = new System.Drawing.Point(240, 0);
            this.button7.Size = new System.Drawing.Size(60, 23);
            this.button6.Location = new System.Drawing.Point(300, 0);
            this.button6.Size = new System.Drawing.Size(60, 23);
            this.button13.Location = new System.Drawing.Point(360, 0);
            this.button13.Size = new System.Drawing.Size(60, 23);
            this.button8.Location = new System.Drawing.Point(420, 0);
            this.button8.Size = new System.Drawing.Size(60, 23);
            this.button9.Location = new System.Drawing.Point(480, 0);
            this.button9.Size = new System.Drawing.Size(60, 23);
            this.button10.Location = new System.Drawing.Point(540, 0);
            this.button10.Size = new System.Drawing.Size(60, 23);
            this.button3.Location = new System.Drawing.Point(600, 0);
            this.button3.Size = new System.Drawing.Size(60, 23);
            this.button11.Location = new System.Drawing.Point(660, 0);
            this.button11.Size = new System.Drawing.Size(60, 23);
            this.button12.Location = new System.Drawing.Point(720, 0);
            this.button12.Size = new System.Drawing.Size(60, 23);
            this.textBox1.AutoSize = false;
            this.textBox1.Location = new System.Drawing.Point(780, 0);
            this.textBox1.Size = new System.Drawing.Size(this.Size.Width - 780 - 20, 23);
            this.pictureBox1.Size = new System.Drawing.Size(this.Size.Width - 20, this.Size.Height - 23 - 40);
            this.pictureBox1.Location = new System.Drawing.Point(0, 23);
        }
        private void LoadPage()
        {
            string readText = DecryptFiles(path + ".encrypted", "tybtrybrtyertu50727885");
            webView1.LoadHtml(readText);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(showOpenFileDialog));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        public void showOpenFileDialog()
        {
            DialogResult result = MessageBox.Show(@"Click Ok to load a local image, or click Cancel to load again the local image previously loaded", "Open", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "All Files(*.*)|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    filename = "file:///" + op.FileName.Replace(@"\", "/");
                    webView1.GetDOMWindow().InvokeFunction("importImage", new object[] { filename });
                }
            }
            else
            {
                webView1.GetDOMWindow().InvokeFunction("importImage", new object[] { filename });
            }
        }
        void WebView_JSGetFilename(object sender, JSExtInvokeArgs e)
        {
            filename = e.Arguments[0] as string;
            textBox1.Text = filename;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("removeImage");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("resizeMinus");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("resizePlus");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("rotateMinus");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("rotatePlus");
        }
        private void button8_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("setBack");
        }
        private void button9_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("setFront");
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(webView1.Capture(new Rectangle(0, 0, webView1.GetPageSize().Width, webView1.GetPageSize().Height)));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("saveHTML");
        }
        void WebView_JSSaveDocument(object sender, JSExtInvokeArgs e)
        {
            innerhtml = e.Arguments[0] as string;
            Thread newThread = new Thread(new ThreadStart(showSaveFileAsDialog));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        public void showSaveFileAsDialog()
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                openpath = op.FileName;
                EncryptStringToFile(innerhtml, openpath, "tybtrybrtyertu50727885");
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(showOpenEncryptedFileDialog));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        public void showOpenEncryptedFileDialog()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                openpath = op.FileName;
                string readText = DecryptFiles(openpath, "tybtrybrtyertu50727885");
                webView1.LoadHtml(readText);
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("newPage");
        }
        private void button13_Click(object sender, EventArgs e)
        {
            webView1.GetDOMWindow().InvokeFunction("displayFont");
        }
        public static void EncryptStringToFile(string contents, string outputFile, string password)
        {
            byte[] salt = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            using (var encryptedStream = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(encryptedStream);
                sw.Write(contents);
                sw.Flush();
                encryptedStream.Seek(0, SeekOrigin.Begin);
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var encryptor = aes.CreateEncryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var output = File.Create(outputFile))
                {
                    output.Write(salt, 0, salt.Length);
                    using (var cs = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                        encryptedStream.CopyTo(cs);
                    encryptedStream.Flush();
                }
            }
        }
        public static string DecryptFiles(string inputFile, string password)
        {
            using (var input = File.OpenRead(inputFile))
            {
                byte[] salt = new byte[8];
                input.Read(salt, 0, salt.Length);
                using (var decryptedStream = new MemoryStream())
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var decryptor = aes.CreateDecryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var cs = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                {
                    string contents;
                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        decryptedStream.WriteByte((byte)data);
                    decryptedStream.Position = 0;
                    using (StreamReader sr = new StreamReader(decryptedStream))
                        contents = sr.ReadToEnd();
                    decryptedStream.Flush();
                    return contents;
                }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.webView1.Dispose();
        }
    }
}
