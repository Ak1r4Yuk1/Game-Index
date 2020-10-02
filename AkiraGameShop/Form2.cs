using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace AkiraGameShop
{
    public partial class Form2 : Form
    {
        internal object textbox;

        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        //Bottone Download
        private void button1_Click(object sender, System.EventArgs e)
        {

             var gameSelected = label1.Text;
             var title = GetGameInfo.GetOriginalTitle(gameSelected).ToString();
             string link = GameExtractor.FindLink(title).ToString();
        
             var dwn = GetGameInfo.GetDownloadLink(link);
             dwn = dwn.Replace("https://linksunlocked.com/?token=", "https://www.uploadhaven.com/download/");
             Process.Start("chrome.exe", $"{dwn}");
        }
    }
}