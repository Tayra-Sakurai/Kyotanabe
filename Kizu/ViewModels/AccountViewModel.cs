using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kizu.Contexts;
using Kizu.Models;
using Kizu.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class AccountViewModel : ObservableValidator
    {
        private readonly IDatabaseService<KizuContext> databaseService;

        private Account account;

        public AccountViewModel(IDatabaseService<KizuContext> databaseService)
        {
            this.databaseService = databaseService;

            account = new();
        }

        public void InitializeForExistingValue(Account account)
        {
            this.account = account;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Invoice));
        }

        [Required]
        public string Name
        {
            get => account.Name;
            set
            {
                if (SetProperty(account.Name, value, account, (m, v) => m.Name = v, true))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public double Invoice => account.Invoice();

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            await databaseService.UpdateAsync(account);
        }

        private bool CanSave()
        {
            ValidateAllProperties();

            return !HasErrors;
        }
    }
}
