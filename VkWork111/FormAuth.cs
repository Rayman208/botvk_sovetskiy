using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkWork111
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(String.Format("https://api.vk.com/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", VkApi.AppId, VkApi.Scope));

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.Contains("access_token") == true)
            {
                int startPosAt = url.IndexOf('=') + 1;
                int finishPosAt = url.IndexOf('&');

                string accessToken = url.Substring(startPosAt, finishPosAt - startPosAt);

                int startPosUid = url.LastIndexOf('=')+1;

                string userId = url.Substring(startPosUid);

                VkApi.InitializeComponent(userId, accessToken);

                this.Hide();
                new FormWork().Show();
            }        
        }
    }
}
