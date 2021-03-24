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
    public partial class Searching : Form
    {
        public RichTextBox richTextBox { get; set; }
        public int CurrentIndex { get; set; } = 0;
        public int Index { get; set; } = 0;
        public Searching(RichTextBox richTextBox)
        {
            InitializeComponent();
            this.richTextBox = richTextBox;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            richTextBox.BackColor = Color.White;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxWhat_TextChanged(object sender, EventArgs e)
        {
            Index = richTextBox.Text.IndexOf(textBoxWhat.Text);
            CurrentIndex = Index;
        }

        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionBackColor = Color.White;

            if (!checkBoxCaseSensitive.Checked)
            {
                SearchWordsNext(richTextBox.Text, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                SearchWordsNext(richTextBox.Text, StringComparison.CurrentCulture);
            }
        }


        public void SearchWordsNext(string text, StringComparison stringComparison)
        {
            if (CurrentIndex == -1)
            {
                MessageBox.Show("Search not fount");
                CurrentIndex = 0;
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


    }
}
