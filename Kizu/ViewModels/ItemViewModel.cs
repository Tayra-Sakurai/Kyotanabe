using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inari.Options;
using Inari.Services;
using Kizu.Contexts;
using Kizu.Models;
using Kizu.Services;
using Kizu.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.ViewModels
{
    public partial class ItemViewModel : ObservableValidator
    {
        private readonly IDatabaseService<KizuContext> databaseService;
        private readonly IEmbeddingService<TaskType> embeddingService;

        private Item item;

        public ItemViewModel(IDatabaseService<KizuContext> databaseService, IEmbeddingService<TaskType> embeddingService)
        {
            this.databaseService = databaseService;
            this.embeddingService = embeddingService;

            Category category = databaseService.GetEntities<Category>().First();
            PaymentMethod paymentMethod = databaseService.GetEntities(context => context.PaymentMethods).First();

            CategoryList = new(databaseService.GetEntities<Category>());
            PaymentMethodList = new(databaseService.GetEntities<PaymentMethod>());

            item = new()
            {
                Category = category,
                CategoryId = category.Id,
                PaymentMethod = paymentMethod,
                PaymentMethodId = paymentMethod.Id,
            };
        }

        public async Task InitializeForExistingValueAsync(Item item)
        {
            this.item = item;

            OnPropertyChanged(nameof(ItemName));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(Expense));
            OnPropertyChanged(nameof(Income));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
        }

        [ObservableProperty]
        private ObservableCollection<Category> categoryList;

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethodList;

        [Required]
        public string? ItemName
        {
            get => item.ItemName;
            set
            {
                if (SetProperty(item.ItemName, value, item, (m, v) => m.ItemName = v, true))
                { 
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        public Category Category
        {
            get => item.Category;
            set
            {
                if (SetProperty(item.Category, value, item, (m, v) => m.Category = v, true))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        public PaymentMethod PaymentMethod
        {
            get => item.PaymentMethod;
            set
            {
                if (SetProperty(item.PaymentMethod, value, item, (m, v) => m.PaymentMethod = v, true))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        [BalanceValidation(nameof(Income))]
        [DataType(DataType.Currency)]
        public double Expense
        {
            get => item.Expense;
            set
            {
                if (SetProperty(item.Expense, value, item, (m, v) => m.Expense = v, true))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        [BalanceValidation(nameof(Expense))]
        [DataType(DataType.Currency)]
        public double Income
        {
            get => item.Income;
            set
            {
                if (SetProperty(item.Income, value, item, (m, v) => m.Income = v, true))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        [PastDateTime(nameof(Time))]
        public DateTimeOffset Date
        {
            get => item.DateTime.Date;
            set
            {
                if (SetProperty(item.DateTime.Date, value, item, SetDate, true))
                {
                    ValidateProperty(Time, nameof(Time));
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [Required]
        [PastDateTime(nameof(Date))]
        public TimeSpan Time
        {
            get => item.DateTime.TimeOfDay;
            set
            {
                if (SetProperty(item.DateTime.TimeOfDay, value, item, SetTime, true))
                {
                    ValidateProperty(Date, nameof(Date));
                    SaveCommand.NotifyCanExecuteChanged();
                    SuggestCategoryCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private void SetDate(Item model, DateTimeOffset value)
        {
            DateTime dt = value.Date;
            model.DateTime = dt.Add(model.DateTime.TimeOfDay);
        }

        private void SetTime(Item model, TimeSpan value)
        {
            DateTime dt = model.DateTime.Date;
            model.DateTime = dt.Add(value);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            item.Vector = await embeddingService.GetVectorAsync(item.ItemName!, null, TaskType.Clustering);
            await databaseService.UpdateAsync(item);
        }

        private bool CanSave()
        {
            return !(string.IsNullOrWhiteSpace(item.ItemName) || HasErrors);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SuggestCategoryAsync()
        {
            if (HasErrors)
                return;

            if (ItemName == null)
                return;

            List<Category> categories = [];

            await foreach (var (value, _, rank) in
                embeddingService.SearchAsync(
                    ItemName,
                    from Category category in CategoryList
                    select category.Vector,
                    CategoryList))
                categories.Insert(rank, value);

            Category = categories[0];
        }
    }
}
