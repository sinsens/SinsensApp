using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets.Event
{
    public class TransactionUpdatedEventEto
    {
        public TransactionUpdatedEventEto(Transaction transaction)
        {
            Entity = transaction;
        }

        public Transaction Entity { get; }
    }
}