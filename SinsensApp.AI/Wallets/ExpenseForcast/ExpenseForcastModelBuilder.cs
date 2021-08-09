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
using Microsoft.Extensions.Logging;

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
                throw new ArgumentException("数据文件路径不能为空！");
            }
            var fileInfo = new FileInfo(input.DataFilePath);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"指定的数据文件不存在！路径：{input.DataFilePath}");
            }

            mlContext = new MLContext(seed: 1);

            var saveModelFileInfo = new FileInfo($"{fileInfo.DirectoryName}/{fileInfo.Name.ReplaceFirst(fileInfo.Extension, ".zip")}");

            if (saveModelFileInfo.Exists)
            {
                if (saveModelFileInfo.CreationTime > fileInfo.CreationTime.AddDays(1))
                {
                    Logger.LogInformation("该数据集 {0} 近期已训练过", fileInfo.Name);
                    return;
                }
                Logger.LogInformation("训练数据集 {0}", fileInfo.Name);
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

                Logger.LogInformation("训练数据集 {0} 完成", fileInfo.Name);
                // Save model
                SaveModel(mlContext, mlModel, saveModelFileInfo.FullName, trainingDataView.Schema);

                Logger.LogInformation("训练数据集 {0} 保存完成", fileInfo.Name);
            }
        }

        public override void Execute(ExpenseForcastModelBuilderInputDto args)
        {
            var output = new ModelBuilderBaseOutput() { TenantID = args.TenantID, UserID = args.UserID, TaskName = "单日消费金额预测模型生成" };
            try
            {
                CreateModel(args);
                output.Result = true;
                output.End = DateTime.Now;
                Task.Run(() =>
                {
                    _eventBus.PublishAsync(output);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "训练数据集 {0} 时发生错误", args.DataFilePath);
                throw;
            }
        }
    }
}