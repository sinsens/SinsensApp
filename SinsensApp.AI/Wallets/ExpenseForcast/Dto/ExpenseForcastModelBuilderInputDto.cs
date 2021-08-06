using SinsensApp.AI.Event.Eto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;

namespace SinsensApp.AI.Event.Eto
{
    [EventName(AIConst.ModelBuilderConst.Start)]
    public class ExpenseForcastModelBuilderInputDto : ModelBuilderBaseInput
    {
        public string DataFilePath { get; set; }

        public bool HasHeader { get; set; }

        public char SeparatorChar { get; set; }

        public bool AllowSparse { get; set; }
    }
}