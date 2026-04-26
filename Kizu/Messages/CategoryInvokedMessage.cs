using CommunityToolkit.Mvvm.Messaging.Messages;
using Kizu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Messages
{
    public class CategoryInvokedMessage : ValueChangedMessage<Category>
    {
        public CategoryInvokedMessage(Category category)
            : base(category) { }
    }
}
