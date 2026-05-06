using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
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
    public sealed partial class BasePage : Page
    {
        public BasePage()
        {
            InitializeComponent();

            MainNavigation.ItemInvoked += MainNavigation_ItemInvoked;
            MainNavigation.BackRequested += MainNavigation_BackRequested;

            SuperFrame.Navigated += SuperFrame_Navigated;
        }

        private void MainNavigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (SuperFrame.CanGoBack)
                SuperFrame.GoBack();
            else
                sender.IsBackEnabled = false;
        }

        private void SuperFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Type pageType = ((Frame)sender).SourcePageType;

            ResourceLoader resourceLoader = new();

            if (pageType == typeof(ItemsViewPage))
                MainNavigation.Header = resourceLoader.GetString("Items");
            else if (pageType == typeof(ItemEditPage))
                MainNavigation.Header = resourceLoader.GetString("Item");
            else if (pageType == typeof(CategoriesViewPage))
                MainNavigation.Header = resourceLoader.GetString("Categories");
            else if (pageType == typeof(CategoryEditPage))
                MainNavigation.Header = resourceLoader.GetString("Category");
            else if (pageType == typeof(AccountsViewPage))
                MainNavigation.Header = resourceLoader.GetString("Accounts");
            else if (pageType == typeof(AccountEditPage))
                MainNavigation.Header = resourceLoader.GetString("Account");
            else if (pageType == typeof(MethodsViewPage))
                MainNavigation.Header = resourceLoader.GetString("Methods");
            else if (pageType == typeof(MethodEditPage))
                MainNavigation.Header = resourceLoader.GetString("Method");
            else
                MainNavigation.Header = "";

            if (((Frame)sender).CanGoBack)
                MainNavigation.IsBackEnabled = true;
            else
                MainNavigation.IsBackEnabled = false;
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            object invokedItem = args.InvokedItem;

            ResourceLoader resourceLoader = new();

            Type pageType;
            string newHeader;

            if (invokedItem == CategoriesItem.Content)
            {
                pageType = typeof(CategoriesViewPage);
                newHeader = resourceLoader.GetString("Categories");
            }
            else if (invokedItem == AccountsItem.Content)
            {
                pageType = typeof(AccountsViewPage);
                newHeader = resourceLoader.GetString("Accounts");
            }
            else if (invokedItem == MethodsItem.Content)
            {
                pageType = typeof(MethodsViewPage);
                newHeader = resourceLoader.GetString("Methods");
            }
            else
            {
                // Assumed as invokedItem == ItemsItem.Content

                pageType = typeof(ItemsViewPage);
                newHeader = resourceLoader.GetString("Items");
            }

            SuperFrame.Navigate(pageType);
            sender.Header = newHeader;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResourceLoader resourceLoader = new();

            SuperFrame.Navigate(typeof(ItemsViewPage));
            MainNavigation.Header = resourceLoader.GetString("Items");
        }
    }
}
