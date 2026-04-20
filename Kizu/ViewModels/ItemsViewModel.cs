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
    public partial class ItemsViewModel : ObservableObject
    {
        private readonly IDatabaseService<KizuContext> databaseService;
        private readonly IEmbeddingService<TaskType> embeddingService;


        [ObservableProperty]
        private ObservableCollection<Item> items;

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        private string query;

        [ObservableProperty]
        private Category? selectedCategory;

        public ItemsViewModel(IDatabaseService<KizuContext> databaseService, IEmbeddingService<TaskType> embeddingService)
        {
            this.databaseService = databaseService;
            this.embeddingService = embeddingService;
            Items = [];
            Categories = [];
            Query = string.Empty;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Items.Clear();

            IEnumerable<Item> items = await databaseService.GetEntitiesAsync(context => context.Items);

            foreach (Item item in items)
                Items.Add(item);

            Categories.Clear();

            IEnumerable<Category> categories = await databaseService.GetEntitiesAsync(context => context.Categories);

            foreach (Category category in categories)
                Categories.Add(category);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            Item item = new()
            {
                Category = Categories.First(),
                CategoryId = Categories.First().Id,
            };

            await databaseService.AddAsync(item);
            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(Exists))]
        public async Task RemoveAsync(Item item)
        {
            await databaseService.RemoveAsync(item);
            await LoadAsync();
        }

        private bool Exists(Item item)
        {
            return databaseService.Exists(item);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        public async Task SearchAsync()
        {
            List<Item> wholeItems =
                SelectedCategory?.Items.ToList() ??
                [.. await databaseService.GetEntitiesAsync(context => context.Items)];

            Items.Clear();

            await foreach (var (value, rate, rank) in
                embeddingService.SearchAsync(Query, wholeItems.Select(e => e.Vector).ToList(), wholeItems))
                Items.Insert(rank, value);
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(Query);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(IsSelected))]
        public async Task EditAsync(Item? item)
        {
            if (item is null) return;

            WeakReferenceMessenger.Default.Send(new ItemInvokedMessage(item));
        }

        private static bool IsSelected(Item? item)
        {
            return item is not null;
        }
    }
}
