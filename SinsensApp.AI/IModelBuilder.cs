using Microsoft.ML;

namespace SinsensApp.AI
{
    public interface IModelBuilder<in TArgs>
    {
        IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext);

        void CreateModel(TArgs args);

        ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline);
    }
}