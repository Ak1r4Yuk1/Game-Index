using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Wordprocessing;
using RestSharp;

namespace AkiraGameShop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            var client = new RestClient("https://steamunlocked.net");
            var request = new RestRequest("/all-games/");
            var response = client.Get(request);
            var sourcePage = response.Content;

            var titles = new List<string>();

            GameExtractor.Extract(sourcePage, out titles);

            foreach (var title in titles)
                listBox1.Items.Add(title);
        }
        



        private void listBox1_DoubleClick(object sender, System.EventArgs e)
        {
            var gameSelected = listBox1.SelectedItem.ToString();

            var form2 = new Form2();
            form2.Show();
            form2.label1.Text = gameSelected;

            //inserimento immagine
            var bytesImage = GetGameInfo.GetImage(gameSelected);
            var ms = new MemoryStream(bytesImage);
            form2.pictureBox1.Image = Image.FromStream(ms);

            //inserimento descrizione
            var description = GetGameInfo.GetDescription(gameSelected);
            form2.richTextBox1.Text = description;
        }
            private void btn_search_Click(object sender, EventArgs e)
            {
            listBox1.SelectedItems.Clear();
            for (int i=listBox1.Items.Count-1; i>0; i--)
            {
                if (listBox1.Items[i].ToString().ToUpper().Contains(textBox1.Text.ToUpper()))
                {
                    listBox1.SetSelected(i, true);
                    
                }
                label1.Text = listBox1.SelectedItems.Count.ToString()+"items found";
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("chrome.exe", "https://universal-bypass.org");
        }
    }
}