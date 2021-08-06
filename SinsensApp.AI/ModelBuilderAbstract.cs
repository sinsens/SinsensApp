using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Volo.Abp.BackgroundJobs;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using SinsensApp.AI.Event.Eto;

namespace SinsensApp.AI
{
    public abstract class ModelBuilderAbstract<TArgs> : BackgroundJob<TArgs>, IModelBuilder<TArgs>
    {
        protected readonly IDistributedEventBus _eventBus;

        public ModelBuilderAbstract(IDistributedEventBus eventBus) => _eventBus = eventBus;

        public abstract void CreateModel(TArgs args);

        public IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText("col0_tf", "col0")
                                      .Append(mlContext.Transforms.CopyColumns("Features", "col0_tf"))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);
            // Set the training algorithm
            var trainer = mlContext.Regression.Trainers.LbfgsPoissonRegression(labelColumnName: "col1", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            Console.WriteLine("=============== Training  model ===============");

            ITransformer model = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("=============== End of training process ===============");
            return model;
        }

        protected static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            Console.WriteLine("The model is saved to {0}", GetAbsolutePath(modelRelativePath));
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(ModelBuilderAbstract<TArgs>).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}