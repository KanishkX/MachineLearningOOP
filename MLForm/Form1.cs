using System.Xml.Linq;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using SentimentModel_ConsoleApp;
namespace MLForm
{
    public partial class Form1 : Form
    {
        private string filePath = @"C:\\Users\\Kanis\\source\\repos\\MachineLearningOOP\\MLForm\\message.txt";
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

            // Add the Text to the file
            AddToText();

            // loading pre-trained model and re-training it
            TrainModel();

            //Comparing the Model


            


        }

        public void AddToText() {

            //Adding text to file to re-train new model
            string textToAdd = textBox1.Text + " " + GuessInputBox.Text;



            try
            {
                // Open the file for appending text
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    // Write the text to the file
                    writer.WriteLine(textToAdd);
                    writer.Close();
                }


                Console.WriteLine("Text added to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void TrainModel() {
            char RetrainSeparatorChar = '	';
            bool RetrainHasHeader = false;

            var mlContext = new MLContext();

            var data = LoadIDataViewFromFile(mlContext, filePath, RetrainSeparatorChar, RetrainHasHeader);
            var model = RetrainModel(mlContext, data);
            // Save the retrained model
            mlContext.Model.Save(model, data.Schema, "retrainedModel.zip");



            

        }
        public static IDataView LoadIDataViewFromFile(MLContext mlContext, string inputDataFilePath, char separatorChar, bool hasHeader)
        {
            return mlContext.Data.LoadFromTextFile<MLModel1.ModelInput>(inputDataFilePath, separatorChar, hasHeader);
        }
        public static ITransformer RetrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"col0", outputColumnName: @"col0")
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"col0" }))
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"col1", inputColumnName: @"col1", addKeyValueAnnotationsAsText: false))
                                    .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 0.03125F, L2Regularization = 0.3624528F, LabelColumnName = @"col1", FeatureColumnName = @"Features" }))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

            return pipeline;
        }




    }
}
