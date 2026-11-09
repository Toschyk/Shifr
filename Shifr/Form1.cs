using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;

namespace _9._4
{
    public partial class Form1 : Form
    {
        SymmetricAlgorithm sa;

        byte[] k1;
        string A;
        string str;
        byte[] key;
        byte[] te;
        byte[] cipherbytes;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sa.GenerateKey();
            //textBox1.Text = Encoding.Default.GetString(sa.Key);
            textBox1.Text = BitConverter.ToString(sa.Key).Replace('-', ' ');
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sa.GenerateIV();
            textBox2.Text = BitConverter.ToString(sa.IV).Replace('-', ' ');
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            sa = DES.Create();
            button1_Click(null, null);
            button2_Click(null, null);

        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            sa = TripleDES.Create();
            button1_Click(null, null);
            button2_Click(null, null);
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            sa = Rijndael.Create();
            button1_Click(null, null);
            button2_Click(null, null);
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            sa = RC2.Create();
            button1_Click(null, null);
            button2_Click(null, null);
        }
        private void groupBox3_Enter(object sender, EventArgs e)
        {
            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbECB.Checked == true)
                    sa.Mode = CipherMode.ECB;
                else if (rbCBC.Checked == true)
                    sa.Mode = CipherMode.CBC;
                else if (rbCFB.Checked == true)
                    sa.Mode = CipherMode.CFB;
                else if (rbOFB.Checked == true)
                    sa.Mode = CipherMode.OFB;
                else if (rbCTS.Checked == true)
                    sa.Mode = CipherMode.CTS;

                if (rbPKCS7.Checked == true)
                    sa.Padding = PaddingMode.PKCS7;
                else if (rbZeros.Checked == true)
                    sa.Padding = PaddingMode.Zeros;
                else if (rbNone.Checked == true)
                    sa.Padding = PaddingMode.None;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] plainbytes = Encoding.Default.GetBytes(textBox3.Text);
                textBox5.Text = BitConverter.ToString(sa.Key).Replace('-', ' ');
                cs.Write(plainbytes, 0, plainbytes.Length);
                cs.Close();
                cipherbytes = ms.ToArray();
                ms.Close();
                //Вывод зашифровки
                str = Encoding.Default.GetString(cipherbytes);
                textBox4.Text = str;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                te = new Byte[str.Length];
                te = Encoding.Default.GetBytes(str);
                key = sa.Key;
                MemoryStream ms = new MemoryStream(te);
                CryptoStream cs = new CryptoStream(ms, sa.CreateDecryptor(), CryptoStreamMode.Read);
                byte[] plainbytes2 = new Byte[te.Length];
                cs.Read(plainbytes2, 0, te.Length);
                cs.Close();
                ms.Close();
                textBox6.Text = Encoding.Default.GetString(plainbytes2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
