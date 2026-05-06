using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Kizu.Messages;
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
    public sealed partial class MethodsViewPage : Page, IRecipient<PaymentMethodAddingMessage>, IRecipient<PaymentMethodInvokedMessage>
    {
        private PaymentMethodsViewModel? viewModel;

        public MethodsViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel = Ioc.Default.GetService<PaymentMethodsViewModel>();

            if (viewModel is not null)
                await viewModel.LoadAsync();

            WeakReferenceMessenger.Default.Register<PaymentMethodAddingMessage>(this);
            WeakReferenceMessenger.Default.Register<PaymentMethodInvokedMessage>(this);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        public void Receive(PaymentMethodAddingMessage message)
        {
            Frame.Navigate(typeof(MethodEditPage));
        }

        public void Receive(PaymentMethodInvokedMessage message)
        {
            Frame.Navigate(typeof(MethodEditPage), message.Value);
        }
    }
}
