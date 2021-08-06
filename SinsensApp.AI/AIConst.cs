using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.AI
{
    public class AIConst
    {
        public class EventConst
        {
            public const string Group = "SinsensApp.AI";
        }

        public class ModelBuilderConst
        {
            public const string Group = EventConst.Group + ".ModelBuilder";

            public const string Finish = Group + ".Finish";

            public const string Start = Group + ".Start";
        }
    }
}