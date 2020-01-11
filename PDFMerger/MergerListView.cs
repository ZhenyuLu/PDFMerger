using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Internal;
using PdfSharp.Drawing;
using PdfSharp.Charting;
using PdfSharp.SharpZipLib;

namespace PDFMerger
{
    public partial class MergerListView : UserControl
    {
        private IList<String> files2merge;
        private const int filePathColIndex = 0;

        public MergerListView()
        {
            InitializeComponent();
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.View = View.Details;
            this.listView1.Items.Clear();

            this.listView1.Columns.Add("File Path", 400);
            this.listView1.Columns.Add("File Name", 200);
            this.listView1.Columns.Add("File Size", 100);
            this.listView1.Columns.Add("Creation Date", 300);

            this.Controls.Add(listView1);
            files2merge = new List<String>();
        }

        public void AddFile(string filePath)
        {
            bool fileExist = false;
            // Check if the file is in the list already.
            foreach (string existingFile in files2merge)
            {
                if (existingFile == filePath)
                {
                    var result = MessageBox.Show("File already exist. Do you still want to add it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        break;
                    }
                    else
                    {
                        fileExist = true;
                        break;
                    }
                }
            }

            if (!fileExist)
            {
                files2merge.Add(filePath);
                // Add the file to the list item
                string fileName = Path.GetFileName(filePath);
                long fileSize = new FileInfo(filePath).Length;
                var creationDate = new FileInfo(filePath).CreationTime;
                ListViewItem lv = new ListViewItem(filePath);
                lv.SubItems.Add(fileName);
                lv.SubItems.Add(fileSize.ToString());
                lv.SubItems.Add(creationDate.ToString());
                this.listView1.Items.Add(lv);
            }
        }
        
        public void MergeFiles(string outputFilePath)
        {
            PdfDocument outputPDFDocument = new PdfDocument();
            foreach (string pdfFile in files2merge)
            {
                PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                outputPDFDocument.Version = inputPDFDocument.Version;
                foreach (PdfPage page in inputPDFDocument.Pages)
                {
                    outputPDFDocument.AddPage(page);
                }
            }
            outputPDFDocument.Save(outputFilePath);

            var result = MessageBox.Show("Files have been merged!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public int FilesLength
        {
            get { return files2merge.Count; }
        }

        public void Clear()
        {
            // Remove all the items
            this.listView1.Items.Clear();
            files2merge = new List<String>();
        }

        public void DeleteSelection()
        {
            foreach (ListViewItem lvi in this.listView1.SelectedItems)
            {
                listView1.Items.Remove(lvi);
                files2merge.Remove(lvi.SubItems[filePathColIndex].Text);
            }
        }

        public void MoveItem(string direction)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (direction.ToLower() == "up")
                {
                    if (lvi.Index > 0)
                    {
                        int origIndex = lvi.Index;
                        int index = lvi.Index - 1;
                        listView1.Items.RemoveAt(lvi.Index);
                        listView1.Items.Insert(index, lvi);

                        files2merge.RemoveAt(origIndex);
                        files2merge.Insert(index, lvi.SubItems[filePathColIndex].Text);
                    }
                }
                else
                {
                    if (lvi.Index < listView1.Items.Count - 1)
                    {
                        int origIndex = lvi.Index;
                        int index = lvi.Index + 1;
                        listView1.Items.RemoveAt(lvi.Index);
                        listView1.Items.Insert(index, lvi);

                        files2merge.RemoveAt(origIndex);
                        files2merge.Insert(index, lvi.SubItems[filePathColIndex].Text);
                    }
                }
            }
        }

    }
}
