using CommunityToolkit.Mvvm.Messaging.Messages;
using Kizu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Messages
{
    public class AccountAddedMessage : ValueChangedMessage<Account>
    {
        public AccountAddedMessage(Account account)
            : base(account)
        { }
    }
}
