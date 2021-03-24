using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsNotebookApp
{
    public partial class Replacement : Form
    {
        public RichTextBox richTextBox { get; set; }
        public int CurrentIndex { get; set; } = 0;
        public int Index { get; set; } = 0;
        public Replacement(RichTextBox richTextBox)
        {
            InitializeComponent();
            this.richTextBox = richTextBox;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            richTextBox.BackColor = Color.White;
        }
        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionBackColor = Color.White;

            if (!checkBoxCaseSensitive.Checked)
            {
                SearchWords(richTextBox.Text, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                SearchWords(richTextBox.Text, StringComparison.CurrentCulture);
            }
        }


        public void SearchWords(string text, StringComparison stringComparison)
        {
            if (CurrentIndex == -1)
            {
                MessageBox.Show("Search not fount");
                return;
            }

            if (Index != CurrentIndex)
            {
                CurrentIndex = text.IndexOf(textBoxWhat.Text, CurrentIndex, stringComparison);
                
            }
            else
            {
                CurrentIndex = text.IndexOf(textBoxWhat.Text);
            }
            if (CurrentIndex == -1)
            {
                MessageBox.Show("Search not fount");
                Index = text.IndexOf(textBoxWhat.Text);
                CurrentIndex = Index;
                return;
            }



            richTextBox.Select(CurrentIndex, textBoxWhat.Text.Length);
            richTextBox.SelectionColor = Color.White;
            richTextBox.SelectionBackColor = Color.Blue;

            if (CurrentIndex + textBoxWhat.Text.Length == richTextBox.Text.Length - 1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;
            }

        }

        private void textBoxWhat_TextChanged(object sender, EventArgs e)
        {
            Index = richTextBox.Text.IndexOf(textBoxWhat.Text);
            CurrentIndex = Index;
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            if (CurrentIndex!=-1 && textBoxThan.Text!=null)
            {
                try
                {
                    var index = richTextBox.Text.IndexOf(textBoxWhat.Text, CurrentIndex - 1);
                    var words = richTextBox.Text.Substring(index, textBoxWhat.Text.Length);
                    richTextBox.Text = richTextBox.Text.Remove(index, words.Length);
                    richTextBox.Text = richTextBox.Text.Insert(index, textBoxThan.Text);
                }
                catch (Exception){ }
            }
           
        }

        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            if (textBoxThan.Text!=null )
            {
                try
                {
                    richTextBox.Text = richTextBox.Text.Replace(textBoxWhat.Text, textBoxThan.Text);
                }
                catch (Exception) { }
            }
            else
            {
                MessageBox.Show("Please fill in the field" , "Error" , MessageBoxButtons.OK,MessageBoxIcon.Error);
            }  
        }



    }



}
