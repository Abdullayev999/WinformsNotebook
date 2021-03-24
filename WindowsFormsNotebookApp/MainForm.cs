using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsNotebookApp
{
    public partial class MainForm : Form
    {
        public int CurrentIndex { get; set; } = 0;
        public int Index { get; set; } = 0;
        public List<string> History { get; set; }
        public int Current { get; set; }
        public string Copy { get; set; }
        public MainForm()
        {
            InitializeComponent();
            History = new List<string>();
            Current = 0;

            toolStripComboBoxSize.SelectedIndex = 0;
            toolStripButtonFont.Text = richTextBox.Font.Name;
            toolStripButtonFont.Font = richTextBox.Font;


            Bitmap bmp = new Bitmap(toolStripButtonColor.Width, toolStripButtonColor.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.Black);
            toolStripButtonColor.Image = bmp;


            toolStripStatusLabelLineSize.Text = $"Line : 0";
            toolStripStatusLabelSizeSymb.Text = $"Symbol : 0";

            toolStripStatusLabelZoom.Text = "Zoom : " + richTextBox.ZoomFactor * 100;

            cutOutToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = true;
            InsertoolStripMenuItem1.Enabled = false;
            removeToolStripMenuItem.Enabled = false;

        }

        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            var color = new ColorDialog();
            var result = color.ShowDialog();
            if (result == DialogResult.OK)
            {
                richTextBox.SelectionColor = color.Color;

                Bitmap bmp = new Bitmap(toolStripButtonColor.Width, toolStripButtonColor.Height);
                Graphics graphics = Graphics.FromImage(bmp);
                graphics.Clear(color.Color);
                toolStripButtonColor.Image = bmp;
            }
        }

        private void toolStripButtonFont_Click(object sender, EventArgs e)
        {
            var font = new FontDialog();
            var result = font.ShowDialog();
            if (result == DialogResult.OK)
            {
                richTextBox.SelectionFont = font.Font;
                toolStripButtonFont.Text = font.Font.Name;
                toolStripButtonFont.Text = font.Font.Style.ToString();

                toolStripButtonFont.Font = new Font(font.Font.FontFamily, 14, font.Font.Style);
            }
        }

        private void toolStripComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float.TryParse(toolStripComboBoxSize.SelectedItem.ToString(), out float size);
            var font = new Font(richTextBox.Font.FontFamily, size);
            richTextBox.SelectionFont = font;
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox.Text))
            {
                History.Add(richTextBox.Text);
            }


            toolStripStatusLabelLineSize.Text = $"Line : {richTextBox.Text.Count(x => x == '\n') + 1}";
            toolStripStatusLabelSizeSymb.Text = $"Symbol : {richTextBox.Text.Length}";

            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionBackColor = Color.White;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "Text file (*.txt)|*.txt|RTF files (*.rtf)|*.rtf";
            var result = save.ShowDialog();

            if (result == DialogResult.OK)
            {
                string text = "";
                switch (Path.GetExtension(save.FileName))
                {
                    case ".txt":
                        text = richTextBox.Text;
                        break;
                    case ".rtf":
                        text = richTextBox.Rtf;
                        break;
                    default:
                        break;
                }
                File.WriteAllText(save.FileName, text);
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Save();
        }

        public void Save()
        {
            var result = MessageBox.Show("Do you want to save data", "Notebook", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var save = new SaveFileDialog();
                save.Filter = "Text file (*.txt)|*.txt|RTF files (*.rtf)|*.rtf";
                result = save.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string text = "";
                    switch (Path.GetExtension(save.FileName))
                    {
                        case ".txt":
                            text = richTextBox.Text;
                            break;
                        case ".rtf":
                            text = richTextBox.Rtf;
                            break;
                    }

                    File.WriteAllText(save.FileName, text);
                }
            }
        }

        private void stateStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = stateStringToolStripMenuItem.Checked;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = toolStripMenuItemAdditionaComponent.Checked;
        }

        private void newPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm newPage = new MainForm();
            newPage.Show();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to save data", "Notebook", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                saveToolStripMenuItem_Click(sender, e);
                MainForm mainForm = new MainForm();
                this.Hide();
                mainForm.ShowDialog();
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                MainForm mainForm = new MainForm();
                this.Hide();
                mainForm.ShowDialog();
                this.Close();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!!string.IsNullOrEmpty(richTextBox.SelectedText))
            {
                Copy = richTextBox.SelectedText;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int i = richTextBox.SelectionStart;
            richTextBox.Text = richTextBox.Text.Insert(i, Copy);
            richTextBox.SelectionStart = i + Copy.Length;
        }

        private void cutOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectedText != null)
            {
                int i = richTextBox.SelectionStart;
                Copy = richTextBox.SelectedText;
                richTextBox.Text = richTextBox.Text.Remove(richTextBox.Text.IndexOf(Copy), Copy.Length);
                richTextBox.SelectionStart = i;
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Current == 0)
            {
                Current = History.Count - 1;
                richTextBox.Text = History[Current--];
            }
            if (Current >= 0 && Current < History.Count)
            {
                richTextBox.Text = History[Current--];
            }
        }

        private void richTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox.SelectedText != null || Copy != null)
            {
                cutOutToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
                InsertoolStripMenuItem1.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
            }
            else
            {
                cutOutToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = true;
                InsertoolStripMenuItem1.Enabled = false;
                removeToolStripMenuItem.Enabled = false;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectedText != null)
            {
                string del = richTextBox.SelectedText;
                int i = richTextBox.SelectionStart;
                richTextBox.Text = richTextBox.Text.Remove(richTextBox.Text.IndexOf(del), del.Length);
                richTextBox.SelectionStart = i;
            }
        }

        private void timeAndDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy = richTextBox.SelectedText;
            richTextBox.SelectAll();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonFont_Click(sender, e);
        }

        private void replacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Replacement replacement = new Replacement(this.richTextBox);
            replacement.ShowDialog();

        }

        private void searchWithBingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Searching searching = new Searching(this.richTextBox);
            searching.ShowDialog();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(richTextBox.SelectedText))
            {
                if (CurrentIndex == 0)
                {
                    Index = richTextBox.Text.IndexOf(richTextBox.SelectedText);
                    CurrentIndex = Index;
                }


                richTextBox.SelectionColor = Color.Black;
                richTextBox.SelectionBackColor = Color.White;

                if (CurrentIndex == -1)
                {
                    CurrentIndex = 0;
                    return;
                }

                if (Index != CurrentIndex)
                {
                    CurrentIndex = richTextBox.Text.IndexOf(richTextBox.SelectedText, CurrentIndex, StringComparison.OrdinalIgnoreCase);

                }
                else
                {
                    CurrentIndex = richTextBox.Text.IndexOf(richTextBox.SelectedText);
                }
                if (CurrentIndex == -1)
                {
                    MessageBox.Show("Search not fount");
                    Index = richTextBox.Text.IndexOf(richTextBox.SelectedText);
                    CurrentIndex = Index;                    
                    return;
                }



                richTextBox.Select(CurrentIndex, richTextBox.SelectedText.Length);
                richTextBox.SelectionColor = Color.White;
                richTextBox.SelectionBackColor = Color.Blue;

                if (CurrentIndex + richTextBox.SelectedText.Length == richTextBox.Text.Length - 1)
                {
                    CurrentIndex = 0;
                }
                else
                {
                    CurrentIndex++;
                }
            }
        }

        private void richTextBox_Click(object sender, EventArgs e)
        {
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionBackColor = Color.White;
        }

        private void richTextBox_TabIndexChanged(object sender, EventArgs e)
        {
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionBackColor = Color.White;
        }

        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            float zoom = richTextBox.ZoomFactor;
            if ((zoom * 2 < 64) && (zoom / 2 > 0.015625))
            {
                if (e.KeyCode == Keys.Add && e.Control)
                {
                    richTextBox.ZoomFactor = zoom * 2;
                }
                if (e.KeyCode == Keys.Subtract && e.Control)
                {
                    richTextBox.ZoomFactor = zoom / 2;
                }
            }

            toolStripStatusLabelZoom.Text = "Zoom : " + richTextBox.ZoomFactor * 100;
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox.ZoomFactor;
            richTextBox.ZoomFactor = zoom / 2;
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox.ZoomFactor;
            richTextBox.ZoomFactor = zoom * 2;
        }

        private void restoreDefaultScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 1;
        }
    }
}
