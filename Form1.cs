using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static DoAn.Form1;
using static System.Net.WebRequestMethods;

namespace DoAn
{
    public partial class Form1 : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        List<Film> films;
        private WebClient myClient;

        public class Film
        {
            public string Link_Img { get; set; }
            public string Link_Uri { get; set; }
            public string Film_Name { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            Load_Movies();
        }

       private void Load_Movies()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri("https://chieuphimquocgia.com.vn/");
            Stream replied = myClient.OpenRead(uri);


            string html = myClient.DownloadString(uri);
            document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);


            replied.Close();
            myClient.Dispose();

            //beta
            //
            // //*[@id=\"tab-1\"]/div/div/div/div/div/div/img
            // //*[@id=\"tab-1\"]/div/div/div/div[2]/div/h3/a

            ////*[@id="tab-1"]/div/div[1]/div/div[2]/div[1]/h3/a
            //
            // film_List.ToList().ForEach(film =>
            // {
            //   films.Add(new Film { Link_Img = film.Attributes["src"].Value });
            // });
            //  var temp_list = film_List.ToList();
            //for (int i = 0; i < temp_list.Count; i++)
            //{
            //    films[i].Link_Uri = "https://betacinemas.vn" + temp_list[i].Attributes["href"].Value;
            //    films[i].Film_Name = temp_list[i].InnerText;
            // }
            //

            //
            //img
            ///html/body/div[5]/div/div/div[1]/div[1]/div[2]/div[1]/div[4]/div[1]/img
            ///html/body/div[1]/div[3]/div[2]/div/div/div/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[1]/img
            ///html/body/div[1]/div[3]/div[2]/div/div/div/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[1]/img

            ///html/body/div[1]/div[3]/div[2]/div/div/div/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/a/h2

            films = new List<Film>();
            if (document == null) return;
            var film_List = document.DocumentNode.SelectNodes("/html/body/div[5]/div/div/div/div[1]/div[1]/div/div/div/div/img");
            film_List.ToList().ForEach(film =>
            {
                string input = film.Attributes["src"].Value;
                string pattern = @"_next.*?\.jpg";
                MatchCollection matches = Regex.Matches(input, pattern);
                films.Add(new Film { Link_Img = "https://chieuphimquocgia.com.vn/"+matches[0].Value+"&w=384&q=75"});
            });

            film_List = document.DocumentNode.SelectNodes("/html/body/div[5]/div/div/div/div[1]/div[1]/div/div/div/div[2]");
            var temp_list = film_List.ToList();
            for (int i = 0; i < temp_list.Count; i++)
            {
                Console.WriteLine(films[i].Link_Img);
                //films[i].Link_Uri = "https://betacinemas.vn" + temp_list[i].Attributes["href"].Value;
                films[i].Film_Name = temp_list[i].InnerText;
            }

            List<Panel> panels = new List<Panel>();

            for (int i = 0; i < films.Count; i++)
            {
                // 
                // Pbx_MoviePoster
                //
                PictureBox Pbx_MoviePoster = new PictureBox();
                Pbx_MoviePoster.Location = new System.Drawing.Point(0, 0);
                Pbx_MoviePoster.Name = "Pbx_MoviePoster";
                Pbx_MoviePoster.Size = new System.Drawing.Size(315, 406);
                Pbx_MoviePoster.TabIndex = 0;
                Pbx_MoviePoster.TabStop = false;
                Pbx_MoviePoster.ImageLocation = films[i].Link_Img;
                Pbx_MoviePoster.SizeMode = PictureBoxSizeMode.StretchImage;
                // 
                // Label_MovieName
                //
                Label Label_MovieName= new Label();
                
                Label_MovieName.AutoSize = true;
                Label_MovieName.Location = new System.Drawing.Point(0, 409);
                Label_MovieName.MaximumSize = new System.Drawing.Size(314, 75);
                Label_MovieName.Name = "Label_MovieName";
                Label_MovieName.Size = new System.Drawing.Size(286, 42);
                Label_MovieName.TabIndex = 1;
                Label_MovieName.Text = films[i].Film_Name;

                Panel Panel_Carousel = new Panel();
                Panel_Carousel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                Panel_Carousel.Controls.Add(Label_MovieName);
                Panel_Carousel.Controls.Add(Pbx_MoviePoster);
                Panel_Carousel.Location = new System.Drawing.Point(200 + 55*(i%3-1) + 315 *(i%3), 40 + 20*(i/3-1)+478 *(i/3));
                Panel_Carousel.Name = "Panel_Carousel";
                Panel_Carousel.Size = new System.Drawing.Size(315, 478);
                Panel_Carousel.TabIndex = 0;

                this.Panel_Outer.Controls.Add(Panel_Carousel);
            }
        }
    }
}
