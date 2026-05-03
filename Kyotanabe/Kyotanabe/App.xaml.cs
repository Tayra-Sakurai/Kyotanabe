using CommunityToolkit.Mvvm.DependencyInjection;
using Google.GenAI;
using Inari.Options;
using Inari.Services;
using Kizu.Contexts;
using Kizu.Services;
using Kizu.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            ApplicationDataContainer settings = ApplicationData.GetDefault().LocalSettings;

            settings.Values["Version"] = "1.0.0";

            if (settings.Values["IsSetupCompleted"] is not true)
                settings.Values["IsSetupCompleted"] = false;

            Ioc.Default.ConfigureServices(
                GetService());

            _window = new MainWindow();
            _window.Activate();
        }

        private static IServiceProvider GetService()
        {
            ServiceCollection services = new();
            services.AddEmbeddingGenerator(new Client().AsIEmbeddingGenerator("gemini-embedding-2-preview"));

            string source = System.IO.Path.Combine(ApplicationData.GetDefault().LocalFolder.Path, "Kizu.db");
            services.AddDbContextFactory<KizuContext>(
                builder =>
                builder.UseLazyLoadingProxies()
                .UseSqlite($"Data Source={source}"));

            services.AddSingleton<IDatabaseService<KizuContext>, KizuDatabaseService>();
            services.AddSingleton<IEmbeddingService<TaskType>, GoogleEmbeddingService>();

            services.AddTransient<AccountsViewModel>();
            services.AddTransient<AccountViewModel>();
            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<ItemsViewModel>();
            services.AddTransient<ItemViewModel>();
            services.AddTransient<PaymentMethodsViewModel>();
            services.AddTransient<PaymentMethodViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
