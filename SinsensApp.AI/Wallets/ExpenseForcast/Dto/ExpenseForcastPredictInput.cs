using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.AI.Wallets.ExpenseForcast.Dto
{
    public class ExpenseForcastPredictInput
    {
        [ColumnName("col0"), LoadColumn(0)]
        public string Day { get; set; }
    }
}