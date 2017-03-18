using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;

namespace VoiceArduino
{
    public partial class Form1 : Form
    {
        SerialPort port;
        public Form1()
        {
            InitializeComponent();
            initilization();

        }

        SpeechSynthesizer myspeech = new SpeechSynthesizer();
        SpeechRecognitionEngine myengine = new SpeechRecognitionEngine();
        PromptBuilder builder = new PromptBuilder();
        

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void initilization()
        {
            button1.Enabled = false;
            trackBar1.Enabled = false;
            textBox1.Enabled = false;
            textBox1.ReadOnly = true; 
            port = new SerialPort();
            port.BaudRate = 9600;
            MessageBox.Show("Make Sure Your Laptop Bluetooth Is Open And Carefully See The Bluetooth COM Port", "Important Message", MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
            for(int a=0;a<=100;a++)
            {
                comboBox1.Items.Add(a.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Choices myvoicecommand = new Choices();
            myvoicecommand.Add(new String[] { "rishabh forward","rishabh left","rishabh right","rishabh backward","exit" });
            Grammar commandgrammer = new Grammar(new GrammarBuilder(myvoicecommand));

            try
            {
                myspeech.SpeakAsync("your project has been started");
                myengine.RequestRecognizerUpdate();
                myengine.LoadGrammar(commandgrammer);
                myengine.SpeechRecognized += Myengine_SpeechRecognized;
                myengine.SetInputToDefaultAudioDevice();
                myengine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                MessageBox.Show("Make Sure You selected a Correct COM Port or Bluetooth must be ON  ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Myengine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Text=="rishabh forward")
            {
                textBox1.Clear();
                textBox1.Text = "Rishabh Move Forward";
                myspeech.Speak(textBox1.Text.ToString());
                port.Write("F");
            }
           else if(e.Result.Text=="rishabh backward")
            {
                textBox1.Clear();
                textBox1.Text = "Rishabh Move Backward";
                myspeech.Speak(textBox1.Text.ToString());
                port.Write("B");
            }
            else if (e.Result.Text == "rishabh left")
            {
                textBox1.Clear();
                textBox1.Text = "Rishabh Move Left";
                myspeech.Speak(textBox1.Text.ToString());
                port.Write("L");
            }
            else if (e.Result.Text == "rishabh right")
            {
                textBox1.Clear();
                textBox1.Text = "Rishabh Move Right";
                myspeech.Speak(textBox1.Text.ToString());
                port.Write("R");
            }
            else  if (e.Result.Text == "exit")
            {
                textBox1.Text = "Closing the Application";
                myspeech.SpeakAsync(textBox1.Text.ToString());
                System.Threading.Thread.Sleep(2000);
                this.Close();

            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            myspeech.SelectVoiceByHints(VoiceGender.Male);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            myspeech.SelectVoiceByHints(VoiceGender.Female);
        }

        private void projectInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://rishicentury.wix.com/rira-corp?fb_ref=Default");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                port.PortName = "COM" + comboBox1.Text.ToString();
                port.Open();
                button1.Enabled = true;
                trackBar1.Enabled = true;
                textBox1.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Make Sure You selected a Correct COM Port or Bluetooth must be ON ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void aboutProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("First Step Select COM port then click start button after that speak a command"
                
                
                , "Information", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
    }
}
