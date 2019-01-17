using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Speech;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Caesar
{
    public partial class Base : Form
    {
        public Base()
        {
            InitializeComponent();
        }

        Cypher _cypher = new Cypher();
        
        public void Exit()
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void Talk(string str)
        {
            System.Speech.Synthesis.SpeechSynthesizer say = new System.Speech.Synthesis.SpeechSynthesizer();
            say.Volume = 100;
            say.Rate = 0;
            say.SpeakAsync(str);
        }

        private void Button1_Click(object sender, EventArgs e) // сдвиг по всему алфавиту
        {
            string buf = "";
            textBox3.Text = "";
            string IN = _cypher.MakeE(textBox1.Text);

            var k = radioButton1.Checked ? 32 : 26;

            for (int i = 0; i <= k; i++)
            {
                buf += i.ToString() + ' ' + _cypher.MovedText(IN, i) + Environment.NewLine + Environment.NewLine;
            }

            textBox3.AppendText(buf);
        }

        private void Button3_Click(object sender, EventArgs e) //сдвиг по numericupdown
        {
            textBox2.Text = _cypher.MovedText(_cypher.MakeE(textBox1.Text), Convert.ToInt32(numericUpDown1.Value));
        }

        private void BExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void Button6_Click(object sender, EventArgs e) //взлом
        {
            var hackTime = new Stopwatch();
            hackTime.Start();
            string IN = _cypher.MakeE(textBox1.Text);
            var moves = new[]
            {
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
                new SortedDictionary<char, double>(), new SortedDictionary<char, double>(),
            };
            var difference = new double[32];

            for (int i = 0; i < 32; i++)
            {
                moves[i] = _cypher.MakeDict(_cypher.MovedText(IN, i));
            }

            double sum = 0;
            for (int j = 0; j < 32; j++)
            {
                for (int i = 0; i < 32; i++)
                {
                    sum += moves[j].ElementAt(i).Value;
                }

                difference[j] = sum;
                sum = 0;
            }

            int k = Array.IndexOf(difference, difference.Min());
            textBox2.Text = _cypher.MovedText(IN, k);
            hackTime.Stop();
            label4.Text = "Время последнего подбора: " + hackTime.ElapsedMilliseconds + " мс." + " ( тиков " +
                          hackTime.ElapsedTicks + ')';
            if (int.Parse(textBox4.Text) > hackTime.ElapsedMilliseconds)
            {
                textBox4.Text = hackTime.ElapsedMilliseconds.ToString();
                textBox6.Text = IN.Length.ToString();
            }

            if (int.Parse(textBox5.Text) < hackTime.ElapsedMilliseconds)
            {
                textBox5.Text = hackTime.ElapsedMilliseconds.ToString();
                textBox7.Text = IN.Length.ToString();
            }

        }

        private void Base_MouseDown(object sender, MouseEventArgs e) //перетаскивание формы
        {
            Capture = false;
            var mmm = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref mmm);
        }

        private void Button7_Click(object sender, EventArgs e) //озвучить ввод
        {
            Talk(textBox1.Text);
        }

        private void Button8_Click(object sender, EventArgs e) //озвучить вывод
        {
            Talk(textBox2.Text);
        }

        private void Button9_Click(object sender, EventArgs e) //частоты букв слева
        {
            string s = textBox1.Text;
            var x = s.GroupBy(c => c)
                .Select(g => new {g, count = g.Count()})
                .OrderBy(t => t.g.Key)
                .Select(t => new {Value = t.g.Key, Count = t.count});

            richTextBox4.Clear();
            richTextBox4.Text = "символ: " + "  " + "частота" + "  " + "отн. частота" + "    " + "\n\r";
            foreach (var count in x)
            {
                double a = Convert.ToDouble(count.Count) / s.Length;
                a = Math.Round(a, 5);
                richTextBox4.Text += "    " + count.Value + "                     " + count.Count + "          " + a +
                                     "\n\r";
            }

        }

        private void Button10_Click(object sender, EventArgs e) //частоты справа
        {
            string s = textBox2.Text;
            var x = s.GroupBy(c => c)
                .Select(g => new {g, count = g.Count()})
                .OrderBy(t => t.g.Key)
                .Select(t => new {Value = t.g.Key, Count = t.count});
            richTextBox3.Clear();
            richTextBox3.Text = "символ" + "  " + "частота" + "  " + "отн. частота" + "    " + "\n\r";
            foreach (var count in x)
            {
                double a = Convert.ToDouble(count.Count) / s.Length;
                a = Math.Round(a, 5);
                richTextBox3.Text += "    " + count.Value + "                     " + count.Count + "          " + a +
                                     "\n\r";
            }

        }

        private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream;
                if ((stream = saveFileDialog1.OpenFile()) != null)
                {
                    var fileSave = new StreamWriter(stream);
                    try
                    {
                        fileSave.Write(textBox2.Text + ' ');
                        fileSave.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка записи файла (" + ex.Message + ')');
                    }

                    fileSave.Close();
                }
            }
        }

        private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream;
                if ((stream = openFileDialog1.OpenFile()) != null)
                {
                    var fileRead = new StreamReader(stream);
                    try
                    {
                        textBox1.Text = fileRead.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка чтения файла (" + ex.Message + ')');
                    }

                    fileRead.Close();
                }
            }
        }

        private void ОчиститьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приложениия для шифрования и расшифровки текстов с применением шифра Цезаря",
                "О программе.");
        }
    }
}