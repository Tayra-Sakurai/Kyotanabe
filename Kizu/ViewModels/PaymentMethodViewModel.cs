using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inari.Options;
using Inari.Services;
using Kizu.Contexts;
using Kizu.Messages;
using Kizu.Models;
using Kizu.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class PaymentMethodViewModel : ObservableValidator
    {
        private readonly IDatabaseService<KizuContext> databaseService;

        private PaymentMethod paymentMethod;

        public PaymentMethodViewModel(IDatabaseService<KizuContext> databaseService)
        {
            this.databaseService = databaseService;

            paymentMethod = new();
            Accounts = new(this.databaseService.GetEntities<Account>());
        }

        [ObservableProperty]
        private ObservableCollection<Account> accounts;

        public void InitializeForExistingValue(PaymentMethod paymentMethod)
        {
            this.paymentMethod = paymentMethod;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Balance));
            OnPropertyChanged(nameof(Account));

            SaveCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
        }

        [Required]
        public string Name
        {
            get => paymentMethod.Name;
            set
            {
                if (SetProperty(paymentMethod.Name, value, paymentMethod, (m, v) => m.Name = v, true))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public double Balance => paymentMethod.Items.Sum(i => i.Income - i.Expense);

        public Account? Account
        {
            get => paymentMethod.Account;
            set
            {
                if (SetProperty(paymentMethod.AccountId, value?.Id, paymentMethod, (m, v) => m.AccountId = v))
                    paymentMethod.Account = value;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            await databaseService.UpdateAsync(paymentMethod);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task AddAsync()
        {
            ValidateAllProperties();
            if (HasErrors) return;

            await databaseService.AddAsync(paymentMethod);

            WeakReferenceMessenger.Default.Send(new PaymentMethodAddedMessage(paymentMethod));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(Exists))]
        public async Task RemoveAsync()
        {
            await databaseService.RemoveAsync(paymentMethod);
            WeakReferenceMessenger.Default.Send(new PaymentMethodDeletedMessage(paymentMethod));
        }

        private bool CanSave()
        {
            return !HasErrors;
        }

        private bool Exists()
        {
            return databaseService.Exists(paymentMethod);
        }
    }
}
