using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Palindrome_Detector
{
    public partial class Form1 : Form
    {
        private const int MAXLENGTH = 60;
        PalindromeChecker palindromeChecker;
        List<string> listOfPalindromes;
        Stopwatch stopWatch;

        public Form1()
        {
            InitializeComponent();
            stopWatch = new Stopwatch();
        }
        /// <summary>
        /// This method is called when the text in the textBox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            filePath.Text = "(No file selected)";
        }

        /// <summary>
        /// Displays a given list in the resultBox starting with the count and elapsed time  and followed by one item per line
        /// </summary>
        /// <param name="listOfPalindromes">A list of strings to be displayed.</param>
        private void Display(List<string> listOfPalindromes)
        {
            string displayText = "Number of Palindromes: " + listOfPalindromes.Count.ToString() + 
                "      (Computation time: " + stopWatch.ElapsedMilliseconds +
                " ms)\r\n--------------------------------------------------------------------------------------------\r\n";
            foreach (string palindrome in listOfPalindromes)
            {
                displayText += palindrome + " (" + palindrome.Length + ")\r\n";
            }
            resultBox.Text = displayText;
        }

        /// <summary>
        /// Gets called when the 'browse' button is clicked.
        /// It opens a file dialog box for selecting an input file and stores the path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                filePath.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Gets called when the 'detect' button is clicked.
        /// It initialises a PalindromeChecker object with specified parameters and calls its FindAll method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detectButton_Click(object sender, EventArgs e)
        {
            UpdateDetectButton(false);
            palindromeChecker = new PalindromeChecker((int)minUpDown.Value, MAXLENGTH, includeSpace.Checked);
            listOfPalindromes = new List<string>();
            stopWatch.Restart();
            listOfPalindromes = palindromeChecker.FindAll(textBox.Text.ToLower());
            stopWatch.Stop();
            Display(listOfPalindromes);
            UpdateDetectButton(true);

        }

        /// <summary>
        /// Updates (enables/disables and changes the text of) 'detect' button based on the given parameter.
        /// </summary>
        /// <param name="isEnabled">The boolean value to determine if the button is enable.</param>
        private void UpdateDetectButton(bool isEnable)
        {
            if (isEnable)
                detectButton.Text = "Detect";
            else
                detectButton.Text = "Calculating ....";
            detectButton.Enabled = isEnable;
        }
    }
}
