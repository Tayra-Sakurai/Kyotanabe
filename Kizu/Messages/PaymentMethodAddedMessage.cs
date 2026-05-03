using CommunityToolkit.Mvvm.Messaging.Messages;
using Kizu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Messages
{
    public class PaymentMethodAddedMessage : ValueChangedMessage<PaymentMethod>
    {
        public PaymentMethodAddedMessage(PaymentMethod value)
            : base(value) { }
    }
}
