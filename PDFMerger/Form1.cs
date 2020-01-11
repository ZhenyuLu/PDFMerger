using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PDFMerger
{
    public partial class Form1 : Form
    {
        private MergerListView mgv;
        public Form1()
        {
            InitializeComponent();
            mgv = new MergerListView();
            this.panel1.Controls.Add(mgv);
            // this.Controls.Add(mgv);
            mgv.Dock = DockStyle.Fill;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "C:\\";
                ofd.Filter = "pdf files (*.pdf)|*.pdf|All files(*.*)|*.*";
                ofd.Multiselect = true;
                ofd.FilterIndex = 1;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // filePath = ofd.FileName;
                    // mgv.AddFile(filePath);
                    foreach(String file in ofd.FileNames)
                    {
                        mgv.AddFile(file);
                    }
                }
            }
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mgv.FilesLength == 0)
            {
                var result = MessageBox.Show("No files to merge!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.InitialDirectory = "C:\\";
                    sfd.Filter = "pdf files (*.pdf)|*.pdf|All files(*.*)|*.*";
                    sfd.FilterIndex = 1;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        mgv.MergeFiles(sfd.FileName);
                    }
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgv.Clear();
        }

        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgv.DeleteSelection();
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgv.MoveItem("up");
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgv.MoveItem("down");
        }
    }
}
