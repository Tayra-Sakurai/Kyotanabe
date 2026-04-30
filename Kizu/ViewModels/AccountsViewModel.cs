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
    public partial class AccountsViewModel : ObservableObject
    {
        private readonly IDatabaseService<KizuContext> databaseService;

        [ObservableProperty]
        private ObservableCollection<Account> accounts;

        public AccountsViewModel(IDatabaseService<KizuContext> databaseService)
        {
            this.databaseService = databaseService;

            Accounts = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Accounts.Clear();

            foreach (var account in await databaseService.GetEntitiesAsync(context => context.Accounts))
                Accounts.Add(account);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            Account account = new();
            await databaseService.AddAsync(account);

            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRemove))]
        public async Task RemoveAsync(Account? account)
        {
            ArgumentNullException.ThrowIfNull(account);

            await databaseService.RemoveAsync(account);
            await LoadAsync();
        }

        private bool CanRemove(Account? account)
        {
            return account is not null && databaseService.Exists(account);
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        public static void Invoke(Account? account)
        {
            if (account is null)
                return;

            WeakReferenceMessenger.Default.Send(new AccountInvokedMessage(account));
        }

        private static bool CanInvoke(Account? account)
        {
            return account is not null;
        }
    }
}
