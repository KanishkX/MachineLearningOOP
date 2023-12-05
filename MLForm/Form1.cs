using System.Xml.Linq;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using SentimentModel_ConsoleApp;
using System.Windows.Forms;
using static Microsoft.ML.DataOperationsCatalog;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MLForm
{
    public partial class Form1 : Form
    {
        private string filePath = @"C:\Users\domin\OneDrive - Nova Scotia Community College\YEAR 2\SEMESTER 1\Advanced-OOP\myMLApp\MLForm\message.txt";
        private string trainerFilePath = "trainer.zip";
        private string modelPath = "retrainedModel.zip";
        private string dataPrepPath = "data_preparation.zip";
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
            // Extract trained model parameters

            // Load the original model

            MLContext mLContext = new MLContext();
            ITransformer originalModel;

            try
            {
                originalModel = mLContext.Model.Load(modelPath, out var modelSchema);
                MaximumEntropyModelParameters? originalModelParameters =
                    ((ISingleFeaturePredictionTransformer<object>)originalModel).Model as MaximumEntropyModelParameters;

            } catch
            {
                Console.WriteLine($"Unable to get original model parameters");
            }


            // Add the Text to the file
            AddToText();

            // loading pre-trained model and re-training it


            RetrainModel();
            


            //Comparing the Model

            // Extract Model Parameters of re-trained model

            //MaximumEntropyModelParameters retrainedModelParameters = retrainedModel.Model as MaximumEntropyModelParameters;

            //// Inspect Change in Weights
            //var weightDiffs =
            //    originalModelParameters.Weights.Zip(
            //        retrainedModelParameters.Weights, (original, retrained) => original - retrained).ToArray();

            //Console.WriteLine("Original | Retrained | Difference");
            //for (int i = 0; i < weightDiffs.Count(); i++)
            //{
            //    Console.WriteLine($"{originalModelParameters.Weights[i]} | {retrainedModelParameters.Weights[i]} | {weightDiffs[i]}");
            //}

        }

        void AddToText() {

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

        static IDataView LoadIDataViewFromFile(MLContext mlContext, string inputDataFilePath, char separatorChar, bool hasHeader)
        {
            return mlContext.Data.LoadFromTextFile<MLModel1.ModelInput>(inputDataFilePath, separatorChar, hasHeader);
        }
        ITransformer RetrainModel()
        {

            // 1. Load data
            var mlContext = new MLContext();
            char SeparatorChar = '	';
            bool HasHeader = true;
            var data = LoadIDataViewFromFile(mlContext, filePath, SeparatorChar, HasHeader);
            
            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            IDataView trainingDataView = dataSplit.TrainSet;
            IDataView testDataView = dataSplit.TestSet;

            // 2. Build training pipeline
            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(
                new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 0.03125F, L2Regularization = 0.3624528F, LabelColumnName = @"col1", FeatureColumnName = @"Features" });
            
            var dataPrepPipeline = BuildDataPrepPipeline(mlContext);
            var pipeline = BuildPipeline(mlContext, trainer);

            var preppedData = dataPrepPipeline.Fit(data);

            var transformedData = preppedData.Transform(data);
            var transformedTrainer = trainer.Fit(transformedData);
            // 3. Train model
            var model = pipeline.Fit(trainingDataView);

            // 4.1 Load original model
            var trainerModel = mlContext.Model.Load(trainerFilePath, out DataViewSchema modelSchema);
            var pipelineModel = mlContext.Model.Load(dataPrepPath, out DataViewSchema modelSchema2);
            var originalModelParameters =
                ((ISingleFeaturePredictionTransformer<object>)trainerModel).Model as MaximumEntropyModelParameters;
            var retrainedModel = trainer.Fit(transformedData, originalModelParameters);
            var newModelParameters =
                ((ISingleFeaturePredictionTransformer<object>)retrainedModel).Model as MaximumEntropyModelParameters;



            // Inspect Change in Weights
            VBuffer<float>[] originalWeights = default;
            VBuffer<float>[] newWeights = default;
            originalModelParameters.GetWeights(ref originalWeights, out int numClasses);
            newModelParameters.GetWeights(ref newWeights, out numClasses);

            var weightDiffs =
                originalWeights.Zip(newWeights, (original, retrained) =>
                {   
                    var originalValues = original.GetValues();
                    var newValues = retrained.GetValues();
                    float[] difference = new float[originalValues.Length];
                    for (int i = 0; i < originalValues.Length; i++)
                    {
                        difference[i] = originalValues[i] - newValues[i];
                    }
                    return difference;
                }).ToArray();

            // Show weights in form
            for (int i = 0; i < weightDiffs.Count(); i++)
            {
                Console.WriteLine($"{originalWeights.ToArray()[i]} | {newWeights.ToArray()[i]} | {weightDiffs[i]}");
            }

            // 4.2 Save model

            // Save full model
            mlContext.Model.Save(model, data.Schema, modelPath);
            // Save data prep
            mlContext.Model.Save(preppedData, data.Schema, dataPrepPath);
            // Save trainer
            mlContext.Model.Save(transformedTrainer, transformedData.Schema, trainerFilePath);

            return model;
        }

        private static IEstimator<ITransformer> BuildDataPrepPipeline(MLContext mlContext)
        {
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: "col0", outputColumnName: "col0")
                .Append(mlContext.Transforms.Concatenate("Features", new[] { "col0" }))
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "col1", inputColumnName: "col1", addKeyValueAnnotationsAsText: false));
            return pipeline;
        }

        static IEstimator<ITransformer> BuildPipeline(MLContext mlContext, LbfgsMaximumEntropyMulticlassTrainer trainer)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = BuildDataPrepPipeline(mlContext)
                                    .Append(trainer)
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));
            return pipeline;
        }
    }
}
