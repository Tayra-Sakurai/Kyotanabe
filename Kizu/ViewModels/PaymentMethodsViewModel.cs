using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kizu.Contexts;
using Kizu.Messages;
using Kizu.Models;
using Kizu.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class PaymentMethodsViewModel : ObservableObject
    {
        private readonly IDatabaseService<KizuContext> databaseService;

        public PaymentMethodsViewModel(IDatabaseService<KizuContext> databaseService)
        {
            this.databaseService = databaseService;

            PaymentMethods = [];
        }

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethods;

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            PaymentMethods.Clear();

            foreach (PaymentMethod paymentMethod in
                await databaseService.GetEntitiesAsync(context => context.PaymentMethods))
                PaymentMethods.Add(paymentMethod);
        }

        [RelayCommand]
        public static void Add()
        {
            WeakReferenceMessenger.Default.Send(new PaymentMethodAddingMessage(new()));
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        public static void InvokeItem(PaymentMethod? paymentMethod)
        {
            if (paymentMethod is not null)
                WeakReferenceMessenger.Default.Send(new PaymentMethodInvokedMessage(paymentMethod));
        }

        private static bool CanInvoke(PaymentMethod? paymentMethod)
        {
            return paymentMethod is not null;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRemove))]
        public async Task RemoveAsync(PaymentMethod? paymentMethod)
        {
            if (paymentMethod == null)
                return;

            await databaseService.RemoveAsync(paymentMethod);
            await LoadAsync();
        }

        private bool CanRemove(PaymentMethod? paymentMethod)
        {
            return paymentMethod != null && databaseService.Exists(paymentMethod);
        }
    }
}
