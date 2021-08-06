using Microsoft.ML;
using SinsensApp.AI.Event.Eto;
using SinsensApp.AI.Wallets.ExpenseForcast.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.AI.Wallets
{
    public class ExpenseForcastAppService : SinsensAppAIAppService, IExpenseForcastAppService
    {
        public ExpenseForcastOutputDto Predict(ExpenseForcastInputDto input)
        {
            ExpenseForcastOutputDto result = CreatePredictionEngine(input).Predict(new ExpenseForcastPredictInput { Day = input.Day });
            return result;
        }

        private PredictionEngine<ExpenseForcastPredictInput, ExpenseForcastOutputDto> CreatePredictionEngine(ExpenseForcastInputDto input)
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();

            string modelPath = GetAbsolutePath(input);
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ExpenseForcastPredictInput, ExpenseForcastOutputDto>(mlModel);

            return predEngine;
        }

        private string GetAbsolutePath(ExpenseForcastInputDto input)
        {
            var tenantId = input.TenantId.HasValue ? input.TenantId.Value.ToString("n") : "Default";
            var userId = input.UserId.Value.ToString("n");

            var dataPath = $"{AppDomain.CurrentDomain.BaseDirectory}/TrainData/{tenantId}/{userId}_day.zip";
            var fileInfo = new FileInfo(dataPath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("请先构建模型！");
            }
            return dataPath;
        }
    }
}