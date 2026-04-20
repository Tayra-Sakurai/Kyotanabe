using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inari.Options;
using Inari.Services;
using Kizu.Contexts;
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

        public ItemsViewModel(IDatabaseService<KizuContext> databaseService, IEmbeddingService<TaskType> embeddingService)
        {
            this.databaseService = databaseService;
            this.embeddingService = embeddingService;
            Items = [];
            Categories = [];
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
    }
}
