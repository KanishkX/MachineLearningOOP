using System.Xml.Linq;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using SentimentModel_ConsoleApp;
using System.Windows.Forms;
using static Microsoft.ML.DataOperationsCatalog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
namespace MLForm
{
    public partial class Form1 : Form
    {
        private string filePath = @"C:\Users\W0478410\Source\Repos\MachineLearningOOP\MLForm\message.txt";
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

        ITransformer TrainModel()
        {
            // 1. Load data
            // 1.1 Load data from file
            var mlContext = new MLContext();
            var data = LoadIDataViewFromFile(mlContext, filePath, ' ', true);

            // 1.2 Split data
            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            IDataView trainingDataView = dataSplit.TrainSet;
            IDataView testDataView = dataSplit.TestSet;

            // 2. Build training pipeline
            // 2.1 Create trainer
            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(
                new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 0.03125F, L2Regularization = 0.3624528F, LabelColumnName = @"col1", FeatureColumnName = @"Features" });

            // 2.2 Build data prep pipeline
            var dataPrepPipeline = BuildDataPrepPipeline(mlContext);
            // 2.3 Build full pipeline
            var pipeline = BuildPipeline(mlContext, trainer);

            // 2.4 Fit data prep pipeline
            var preppedData = dataPrepPipeline.Fit(trainingDataView);

            // 3. Train full model
            var model = pipeline.Fit(trainingDataView);

            // 4 Save model
            
            // 4.1 Transform data prep pipeline
            var transformedData = preppedData.Transform(trainingDataView);
            var transformedTrainer = trainer.Fit(transformedData);
            
            // 4.2 Save full model
            mlContext.Model.Save(model, data.Schema, modelPath);
            // 4.3 Save data prep
            mlContext.Model.Save(preppedData, data.Schema, dataPrepPath);
            // 4.4 Save trainer
            mlContext.Model.Save(transformedTrainer, transformedData.Schema, trainerFilePath);
            return model;
        }

        ITransformer RetrainModel()
        {
            // 1. Load models
            var mlContext = new MLContext();
            
            var trainerModel = mlContext.Model.Load(trainerFilePath, out var trainerSchema);
            var pipelineModel = mlContext.Model.Load(dataPrepPath, out var pipelineSchema);

            // 1.1 Load data from file
            var newData = LoadIDataViewFromFile(mlContext, filePath, ' ', true);

            // 1.2 Split data
            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(newData, testFraction: 0.2);
            IDataView trainingDataView = dataSplit.TrainSet;
            IDataView testDataView = dataSplit.TestSet;


            // 2. Build training pipeline

            // 2.1 Pull original model parameters
            var originalModelParameters =
                ((ISingleFeaturePredictionTransformer<MaximumEntropyModelParameters>)trainerModel).Model;
            
            // 2.2 Transform pipeline with new data
            var newTransformedData = pipelineModel.Transform(trainingDataView);

            // 2.3 Retrain model
            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(
                new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 0.03125F, L2Regularization = 0.3624528F, LabelColumnName = @"col1", FeatureColumnName = @"Features" });

            //var retrainedModel = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy().Fit(newTransformedData, originalModelParameters);
            var retrainedModel = trainer.Fit(newTransformedData, originalModelParameters);

            // 2.5 Pull new model parameters
            var newModelParameters =
                retrainedModel.Model;

            // 3. Comapre models

            // 3.1 Inspect Change in Weights
            VBuffer<float>[] originalWeights = default;
            VBuffer<float>[] newWeights = default;
            originalModelParameters.GetWeights(ref originalWeights, out int numClasses);
            newModelParameters.GetWeights(ref newWeights, out numClasses);

            var actualWeight =
                originalWeights.Select(original =>
                {
                    var originalValues = original.GetValues();
                    float[] actualWeightArray = new float[originalValues.Length];
                    for (int i = 0; i < originalValues.Length; i++)
                    {   
                        actualWeightArray[i] = originalValues[i];
                    }
                    return actualWeightArray;
                }).ToArray();

            
            var newWeight =
                originalWeights.Zip(newWeights, (original, retrained) =>
                {
                    var newValues = retrained.GetValues();
                    float[] Actualweight = new float[newValues.Length];
                    for (int i = 0; i < newValues.Length; i++)
                    {
                        Actualweight[i] = newValues[i];
                    }
                    return Actualweight;
                }).ToArray();
            var weightDiffs =
                originalWeights.Zip(newWeights, (original, retrained) =>
                {
                    var originalValues = original.GetValues();
                    var newValues = retrained.GetValues();
                    float[] actualWeightArray = new float[originalValues.Length];
                    float[] Actualweight = new float[newValues.Length];
                    float[] difference = new float[originalValues.Length];
                    for (int i = 0; i < originalValues.Length; i++)
                    {
                        difference[i] = originalValues[i] - newValues[i];
                    }
                    return difference;
                }).ToArray();
            double actualWeightAverage = actualWeight.SelectMany(array => array).Average();
            double newWeightAverage = newWeight.SelectMany(array => array).Average();
            double weightDiffsAverage = weightDiffs.SelectMany(array => array).Average();

            originalBox.Text = actualWeightAverage.ToString();
            reweightBox.Text = newWeightAverage.ToString();
            diffBox.Text = weightDiffsAverage.ToString();
            ////// Show weights in form
            //for (int i = 0; i < weightDiffs.Count(); i++)
            //{
            //    Console.WriteLine($"{originalWeights.ToArray()[i]} | {newWeights.ToArray()[i]} | {weightDiffs[i]}");
            //}


            return retrainedModel;
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
