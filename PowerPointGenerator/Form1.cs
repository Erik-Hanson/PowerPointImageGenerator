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
            // If there is nothing in the title and text area, then none of the below work is neccessary
            if (textBox1.Text != "" && richTextBox1.Text != "")
            {
                StringBuilder textAreaString = new StringBuilder();
                StringBuilder titleAreaString = new StringBuilder();
                HashSet<String> hs = new HashSet<string>();

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
                    hs.Add(stringToParse);
                }

                // Add words from title area to hashset (note that these will be uinque words due to the nature of a hashset)
                foreach (string stringToParse in titleAreaString.ToString().Split(' '))
                {
                    hs.Add(stringToParse);
                }

                foreach (string hsString in hs)
                {
                    Console.WriteLine(hsString);
                }
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
