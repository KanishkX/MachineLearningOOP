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
            //get new data
            var sampleData = new SentimentModel.ModelInput()
            {
                Col0 = textBox1.Text,
            };

            //Load model and predict output
            var result = SentimentModel.Predict(sampleData);

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
    }
}
