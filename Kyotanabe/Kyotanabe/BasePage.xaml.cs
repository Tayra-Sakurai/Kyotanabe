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
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem invokedItem = (NavigationViewItem)args.InvokedItem;

            ResourceLoader resourceLoader = new("HeaderResources");

            switch (invokedItem.Tag as string)
            {
                case "Items":
                    SuperFrame.Navigate(typeof(ItemsViewPage));
                    sender.Header = resourceLoader.GetString(invokedItem.Tag as string);
                    break;

                case "Accounts":
                    SuperFrame.Navigate(typeof(AccountsViewPage));
                    sender.Header = resourceLoader.GetString(invokedItem.Tag as string);
                    break;

                case "Categories":
                    SuperFrame.Navigate(typeof(CategoriesViewPage));
                    sender.Header = resourceLoader.GetString((string)invokedItem.Tag);
                    break;

                case "Methods":
                    SuperFrame.Navigate(typeof(MethodsViewPage));
                    sender.Header = resourceLoader.GetString((string)invokedItem.Tag);
                    break;

                default:
                    SuperFrame.Navigate(typeof(ItemsViewPage));
                    sender.Header = resourceLoader.GetString("Items");
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResourceLoader resourceLoader = new("HeaderResources");

            SuperFrame.Navigate(typeof(ItemsViewPage));
            MainNavigation.Header = resourceLoader.GetString("Items");
        }
    }
}
