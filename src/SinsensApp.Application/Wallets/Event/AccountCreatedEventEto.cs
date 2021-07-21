using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets.Event
{
    public class AccountCreatedEventEto
    {
        public AccountCreatedEventEto(Account account)
        {
            Entity = account;
        }

        public Account Entity { get; }
    }
}