using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe.Templates;

[ContentProperty(Name = "Content")]
public sealed partial class EditControl : Control
{
    public EditControl()
    {
        DefaultStyleKey = typeof(EditControl);
    }

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(object),
        typeof(EditControl),
        new(default));

    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(ContentProperty, value);
    }

    private static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
        nameof(SaveCommand),
        typeof(ICommand),
        typeof(EditControl),
        new(default(ICommand)));

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    private static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
        nameof(RemoveCommand),
        typeof(ICommand),
        typeof(EditControl),
        new(default(ICommand)));
}
