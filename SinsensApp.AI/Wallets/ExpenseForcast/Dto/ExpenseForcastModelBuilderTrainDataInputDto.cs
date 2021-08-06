using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.ai
{
    public class ExpenseForcastModelBuilderTrainDataInputDto
    {
        [ColumnName("col0"), LoadColumn(0)]
        public string Day { get; set; }

        [ColumnName("col1"), LoadColumn(1)]
        public float Amount
        {
            get; set;
        }
    }
}