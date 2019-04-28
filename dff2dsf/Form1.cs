using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dff2dsf
{
    public partial class Form1 : Form
    {
        private readonly BindingList<FileToProcess> bindingList = new BindingList<FileToProcess>();

        public Form1()
        {
            InitializeComponent();

            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var path = fbd.SelectedPath;

                    var files = Directory.GetFiles(path);

                    bindingList.Clear();

                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file).EndsWith(".dff", StringComparison.CurrentCultureIgnoreCase))
                            bindingList.Add(new FileToProcess
                            {
                                SrcFile = file,
                                DestFile = Path.ChangeExtension(file, ".dsf")
                            });
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var converter = "assets/dff2dsf_win32.exe";
            if (!File.Exists(converter))
                MessageBox.Show("no converter found!");

            var fileIndex = 0;
            foreach (var file in bindingList)
            {
                try
                {
                    var tempSrcFile = Path.Combine(Path.GetDirectoryName(file.SrcFile), $"Temp-{++fileIndex}.dff");
                    var tempTargetFile = Path.Combine(Path.GetDirectoryName(file.SrcFile), $"Temp-{fileIndex}.dsf");

                    if (File.Exists(file.SrcFile))
                        File.Move(file.SrcFile, tempSrcFile);

                    using (var process = new Process())
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = Path.GetFullPath(converter),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true,
                            Arguments = $"{tempSrcFile} {tempTargetFile}"
                        };

                        process.StartInfo = startInfo;
                        process.Start();

                        file.Status = process.StandardError.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(file.Status))
                            file.Status = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        if (File.Exists(tempTargetFile))
                        {
                            File.Move(tempTargetFile, file.DestFile);
                            if (checkBox1.Checked && File.Exists(tempSrcFile))
                                File.Delete(tempSrcFile);
                        }
                        else
                        {
                            if (File.Exists(tempSrcFile))
                                File.Move(tempSrcFile, file.SrcFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    file.Status = ex.Message;
                }
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=26999");
        }
    }
}
