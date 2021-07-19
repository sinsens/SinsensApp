using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets
{
    public interface IMustHasUser<T>
    {
        public T UserId { get; set; }
    }
}