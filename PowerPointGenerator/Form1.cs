using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Drawing.Imaging;

namespace PowerPointGenerator
{
    public partial class Form1 : Form
    {
        private const string QUERY_PARAMETER = "?q=";
        private const string MKT_PARAMETER = "&mkt=";
        private const string COUNT_PARAMETER = "&count=6";
        private static string apiKey = ""; // REMEMBER TO REMOVE
        private static string baseUri = "https://api.bing.microsoft.com/v7.0/images/search";
        private static string clientIdHeader = null;
        private static HashSet<String> wordSet = new HashSet<string>();
        private static HttpClient client = new HttpClient();
        private static PictureBox[] pics = new PictureBox[50];
        private static FlowLayoutPanel[] panel = new FlowLayoutPanel[50];
        private static List<String> selectedImages = new List<string>();
        private static int ndx = 0;
        private static int offset = 0;

        // NOTE: IMAGES WILL BE SAVED IN PowerPointGenerator\PowerPointGenerator\bin\Debug at the moment
        private static async Task FindImages()
        {
            try
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

                foreach (string word in wordSet) {
                    string queryString = QUERY_PARAMETER + Uri.EscapeDataString(word) + MKT_PARAMETER + "en-us" + COUNT_PARAMETER;
                    HttpResponseMessage response = await MakeRequestAsync(queryString).ConfigureAwait(false);
                    clientIdHeader = response.Headers.GetValues("X-MSEdge-ClientID").FirstOrDefault();
                    string contentString = await response.Content.ReadAsStringAsync();

                    Dictionary<string, object> searchResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);

                    if (response.IsSuccessStatusCode)
                    {
                        ListImages(searchResponse);
                    }
                    else
                    {
                        PrintErrors(response.Headers, searchResponse);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task<HttpResponseMessage> MakeRequestAsync(string queryString)
        {
            return await client.GetAsync(baseUri + queryString).ConfigureAwait(false);
        }

        static void ListImages(Dictionary<string, object> response)
        {
            var images = response["value"] as Newtonsoft.Json.Linq.JToken;

            
            foreach (Newtonsoft.Json.Linq.JToken image in images)
            {
                panel[ndx] = new FlowLayoutPanel();
                panel[ndx].Name = "panel" + ndx;
                panel[ndx].Location = new Point(3, offset);
                panel[ndx].Size = new Size(317, 122);

                pics[ndx] = new PictureBox
                {
                    Location = new Point(953, 95 + offset),
                    Name = "pic" + ndx,
                    Size = new Size(300, 75),
                    ImageLocation = (string)image["thumbnailUrl"],
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                panel[ndx].Controls.Add(pics[ndx]);
                PictureBox pic = new PictureBox();
                pic = pics[ndx];

                pic.Click += (sender, e) =>
                {
                    pic.Visible = false;
                    MessageBox.Show("Image Selected");
                    selectedImages.Add(pic.ImageLocation);
                };
                ndx++;
                offset += 130;
            }
        }

        static void PrintErrors(HttpResponseHeaders headers, Dictionary<String, object> response)
        {
            Console.WriteLine("The response contains the following errors:\n");

            object value;

            if (response.TryGetValue("error", out value))  // typically 401, 403
            {
                PrintError(response["error"] as Newtonsoft.Json.Linq.JToken);
            }
            else if (response.TryGetValue("errors", out value))
            {
                // Bing API error

                foreach (Newtonsoft.Json.Linq.JToken error in response["errors"] as Newtonsoft.Json.Linq.JToken)
                {
                    PrintError(error);
                }

                IEnumerable<string> headerValues;
                if (headers.TryGetValues("BingAPIs-TraceId", out headerValues))
                {
                    Console.WriteLine("\nTrace ID: " + headerValues.FirstOrDefault());
                }
            }

        }

        static void PrintError(Newtonsoft.Json.Linq.JToken error)
        {
            string value = null;

            Console.WriteLine("Code: " + error["code"]);
            Console.WriteLine("Message: " + error["message"]);

            if ((value = (string)error["parameter"]) != null)
            {
                Console.WriteLine("Parameter: " + value);
            }

            if ((value = (string)error["value"]) != null)
            {
                Console.WriteLine("Value: " + value);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // If there is nothing in the title and text area, then none of the below work is neccessary
            if (textBox1.Text != "" && richTextBox1.Text != "")
            {
                StringBuilder textAreaString = new StringBuilder();
                StringBuilder titleAreaString = new StringBuilder();

                // Build a string from the text area without punctuation
                foreach (char c in richTextBox1.Text)
                {
                    if (!char.IsPunctuation(c))
                    {
                        textAreaString.Append(c);
                    }
                }

                // Build a string from the title area without punctuation
                foreach (char c in textBox1.Text)
                {
                    if (!char.IsPunctuation(c))
                    {
                        titleAreaString.Append(c);
                    }
                }

                // Add words from text area to hashset (note that these will be uinque words due to the nature of a hashset)
                foreach (string stringToParse in textAreaString.ToString().Split(' '))
                {
                    wordSet.Add(stringToParse);
                }

                // Add words from title area to hashset (note that these will be uinque words due to the nature of a hashset)
                foreach (string stringToParse in titleAreaString.ToString().Split(' '))
                {
                    wordSet.Add(stringToParse);
                }

                FindImages().Wait();

                foreach (FlowLayoutPanel flow in panel)
                {
                    flowLayoutPanel1.Controls.Add(flow);
                }
            }
            else
            {
                MessageBox.Show("Enter a title AND text area contents before searching");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(textBox1.Font, FontStyle.Bold);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No images were selected");
            }
            else
            {
                int counter = 0;
                foreach (string s in selectedImages)
                {
                    // Credit for below code: https://stackoverflow.com/a/24797557
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData(s);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (Image image = Image.FromStream(mem))
                            {
                                image.Save("PPTSelectedImage" + counter + ".png", ImageFormat.Png);
                            }
                        }

                    }
                    counter++;
                }
                MessageBox.Show("Images downloaded!");
            }
        }
    }
}
