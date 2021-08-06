using SinsensApp.AI.Event.Eto;

namespace SinsensApp.AI.Wallets
{
    public interface IExpenseForcastAppService
    {
        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        ExpenseForcastOutputDto Predict(ExpenseForcastInputDto input);
    }
}