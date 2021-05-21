using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace PowerPointGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // If there is nothing in the text body, then none of the below is neccessary
            if (richTextBox1.Text != "")
            {
                StringBuilder s = new StringBuilder();
                HashSet<String> hs = new HashSet<string>();

                // Build a string without punctuation
                foreach (char c in richTextBox1.Text)
                {
                    if (!char.IsPunctuation(c))
                    {
                        s.Append(c);
                    }
                }

                // Add unique words to a hashset (hashset will only contain unique values)
                foreach (string stringToParse in s.ToString().Split(' '))
                {
                    hs.Add(stringToParse);
                }

                Console.WriteLine(textBox1.Text);

/*                foreach (string hsString in hs)
                {
                    Console.WriteLine(hsString);
                }*/
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
    }
}
