using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Kizu.Messages;
using Kizu.Models;
using Kizu.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryEditPage : Page, IRecipient<CategoryDeletedMessage>
    {
        public CategoryEditPage()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<CategoryViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Category category &&
                DataContext is CategoryViewModel dataContext)
            {
                dataContext.InitializeForExistingValue(category);
            }
        }

        public void Receive(CategoryDeletedMessage message)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
