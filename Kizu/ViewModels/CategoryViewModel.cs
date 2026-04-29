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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class CategoryViewModel : ObservableValidator
    {
        private readonly IDatabaseService<KizuContext> _databaseService;
        private readonly IEmbeddingService<TaskType> _embeddingService;
        private Category category;

        public CategoryViewModel(IDatabaseService<KizuContext> databaseService, IEmbeddingService<TaskType> embeddingService)
        {
            _databaseService = databaseService;
            _embeddingService = embeddingService;
            category = new();
        }

        public void InitializeForExistingValue(Category category)
        {
            this.category = category;
            OnPropertyChanged(nameof(CategoryName));
            OnPropertyChanged(nameof(Budget));
            SaveCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
        }

        [Required]
        public string CategoryName
        {
            get => category.Name;
            set
            {
                if (SetProperty(category.Name, value, category, (m, v) => m.Name = v, true))
                    SaveCommand.NotifyCanExecuteChanged();
            }
        }

        [Required]
        [DataType(DataType.Currency)]
        public double Budget
        {
            get => category.Budget;
            set
            {
                if (SetProperty(category.Budget, value, category, (m, v) => m.Budget = v))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    RemoveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            ValidateAllProperties();
            if (HasErrors)
                return;

            category.Vector = await _embeddingService.GetVectorAsync(CategoryName, null, TaskType.Classification);
            await _databaseService.UpdateAsync(category);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRemove))]
        public async Task RemoveAsync()
        {
            if (_databaseService.Exists(category))
            {
                await _databaseService.RemoveAsync(category);
                WeakReferenceMessenger.Default.Send(new CategoryDeletedMessage(category));
            }
        }

        private bool CanSave()
        {
            return !HasErrors;
        }

        private bool CanRemove()
        {
            return _databaseService.Exists(category);
        }
    }
}
