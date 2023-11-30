using System.Xml.Linq;
using SentimentModel_ConsoleApp;
namespace MLForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sampleData = new MLModel1.ModelInput()
            {
                Col0 = textBox1.Text,
            };

            //Load model and predict output
            var result = MLModel1.Predict(sampleData);

            guessBox.Text = result.PredictedLabel.ToString();
            confidenceBox.Text = (result.Score[0] * 100).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void reTrainButton_Click(object sender, EventArgs e)
        {   
            //Adding text to file to re-train new model
            string textToAdd = textBox1.Text + " " + GuessInputBox.Text;

            string filePath = "message.txt";

            try
            {
                // Open the file for appending text
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    // Write the text to the file
                    writer.WriteLine(textToAdd);
                }

                Console.WriteLine("Text added to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
