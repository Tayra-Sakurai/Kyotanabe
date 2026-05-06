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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private readonly IDatabaseService<KizuContext> _databaseService;
        private readonly IEmbeddingService<TaskType> _embeddingService;

        public CategoriesViewModel(IDatabaseService<KizuContext> databaseService, IEmbeddingService<TaskType> embeddingService)
        {
            _databaseService = databaseService;
            _embeddingService = embeddingService;
            Categories = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Categories.Clear();

            IEnumerable<Category> categories = await _databaseService.GetEntitiesAsync(context => context.Categories);
            
            foreach (Category category in categories)
                Categories.Add(category);

            RemoveCommand.NotifyCanExecuteChanged();
            InvokeCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        public static void Invoke(Category? category)
        {
            if (category is not null)
                WeakReferenceMessenger.Default.Send(new CategoryInvokedMessage(category));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRemove))]
        public async Task RemoveAsync(Category? category)
        {
            if (category is not null)
            {
                await _databaseService.RemoveAsync(category);
                await LoadAsync();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            Category newCategory = new();
            await _databaseService.AddAsync(newCategory);
            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        public async Task SearchAsync(string? searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            List<Category> categories = [.. Categories];
            Categories.Clear();

            await foreach (
                (Category value, float _, int rank) in
                _embeddingService.SearchAsync(
                    searchText,
                    from c in categories
                    select c.Vector,
                    categories))
                Categories.Insert(rank, value);
        }

        private static bool CanSearch(string? searchText)
        {
            return string.IsNullOrWhiteSpace(searchText);
        }

        private bool CanRemove(Category? category)
        {
            return (category is not null) && _databaseService.Exists(category);
        }

        private static bool CanInvoke(Category? category)
        {
            return category is not null;
        }

        [ObservableProperty]
        private ObservableCollection<Category> categories;
    }
}
