using CommunityToolkit.Mvvm.Messaging.Messages;
using Kizu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Messages
{
    public class AccountDeletedMessage : ValueChangedMessage<Account>
    {
        public AccountDeletedMessage(Account account) : base(account) { }
    }
}
