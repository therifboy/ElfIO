using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElfIO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Create new instance
                ElfIO elfio = new ElfIO();
                // Load ELF
                elfio.Load(dialog.FileName);
                // Read from address
                byte[] buff = new byte[8];
                elfio.Read(ref buff, (uint)elfio.Elf.Header.EntryPoint, 8);
                // Dump Elf info
                elfio.Dump();
                // Save as new file
                elfio.Save("test.elf");
            }
        }
    }
}