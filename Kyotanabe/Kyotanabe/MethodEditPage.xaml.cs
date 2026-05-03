using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kizu.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Kizu.Messages;
using Kizu.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MethodEditPage : Page, IRecipient<PaymentMethodDeletedMessage>
{
    public MethodEditPage()
    {
        InitializeComponent();

        DataContext = Ioc.Default.GetRequiredService<PaymentMethodViewModel>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (DataContext is PaymentMethodViewModel dataContext &&
            e.Parameter is PaymentMethod paymentMethod)
            dataContext.InitializeForExistingValue(paymentMethod);
    }

    public void Receive(PaymentMethodDeletedMessage message)
    {
        if (Frame.CanGoBack)
            Frame.GoBack();
    }
}
