using Microsoft.ML;
using Microsoft.ML.Data;
using SinsensApp.AI.Event.Eto;
using SinsensApp.Wallets.Dtos.ai;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.Trainers;

namespace SinsensApp.AI.Wallets.ExpenseForcast
{
    public class ExpenseForcastModelBuilder : ModelBuilderAbstract<ExpenseForcastModelBuilderInputDto>, ITransientDependency
    {
        // Create MLContext to be shared across the model creation workflow objects Set a random
        // seed for repeatable/deterministic results across multiple trainings.
        private MLContext mlContext;

        public ExpenseForcastModelBuilder(IDistributedEventBus eventBus) : base(eventBus)
        {
        }

        public override void CreateModel(ExpenseForcastModelBuilderInputDto input)
        {
            if (input.DataFilePath.IsNullOrWhiteSpace())
            {
                throw new Exception("数据文件路径不能为空！");
            }
            var fileInfo = new FileInfo(input.DataFilePath);

            if (!fileInfo.Exists)
            {
                throw new Exception("指定的数据文件不存在！");
            }

            mlContext = new MLContext(seed: 1);

            var saveModelFileInfo = new FileInfo($"{fileInfo.DirectoryName}/{fileInfo.Name.ReplaceFirst(fileInfo.Extension, ".zip")}");

            if (saveModelFileInfo.Exists)
            {
                if (saveModelFileInfo.CreationTime > fileInfo.CreationTime.AddDays(1))
                {
                    return;
                }
                // 重新训练
                DataViewSchema modelSchema;
                var trainedModel = mlContext.Model.Load(saveModelFileInfo.FullName, out modelSchema);

                // Extract trained model parameters
                LinearRegressionModelParameters originalModelParameters =
                    ((ISingleFeaturePredictionTransformer<object>)trainedModel).Model as LinearRegressionModelParameters;

                // Load Data
                IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ExpenseForcastModelBuilderTrainDataInputDto>(path: fileInfo.FullName,
                                                    hasHeader: false,
                                                    separatorChar: ',',
                                                    allowQuoting: true,
                                                    allowSparse: false);

                // Retrain model
                RegressionPredictionTransformer<LinearRegressionModelParameters> retrainedModel =
                    mlContext.Regression.Trainers.OnlineGradientDescent()
                        .Fit(trainingDataView, originalModelParameters);
                // Save model
                SaveModel(mlContext, retrainedModel, saveModelFileInfo.FullName, trainingDataView.Schema);
            }
            else
            {
                // Load Data
                IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ExpenseForcastModelBuilderTrainDataInputDto>(path: fileInfo.FullName,
                                                    hasHeader: false,
                                                    separatorChar: ',',
                                                    allowQuoting: true,
                                                    allowSparse: false);

                // Build training pipeline
                IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);

                // Train Model
                ITransformer mlModel = TrainModel(mlContext, trainingDataView, trainingPipeline);
                // Save model
                SaveModel(mlContext, mlModel, saveModelFileInfo.FullName, trainingDataView.Schema);
            }
        }

        public override void Execute(ExpenseForcastModelBuilderInputDto args)
        {
            var output = new ModelBuilderBaseOutput() { TenantID = args.TenantID, UserID = args.UserID, TaskName = "单日消费金额预测模型生成" };
            try
            {
                CreateModel(args);
                output.Result = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            finally
            {
                output.End = DateTime.Now;
                Task.Run(() =>
                {
                    _eventBus.PublishAsync(output);
                });
            }
        }
    }
}