using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Domain
{
    public class ResponseMessage
    {
        public bool Result { get; set; }

        public string Detail { get; set; }

        public ResponseMessage()
        {
        }

        public static ResponseMessage ResponseError(string message)
        {
            return new ResponseMessage { Detail = message };
        }

        public static ResponseMessage ResponseOk(string message = null)
        {
            return new ResponseMessage { Detail = message, Result = true };
        }
    }
}