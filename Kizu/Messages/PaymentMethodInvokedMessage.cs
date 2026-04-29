using CommunityToolkit.Mvvm.Messaging.Messages;
using Kizu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Messages
{
    public class PaymentMethodInvokedMessage : ValueChangedMessage<PaymentMethod>
    {
        public PaymentMethodInvokedMessage(PaymentMethod paymentMethod) : base(paymentMethod) { }
    }
}
