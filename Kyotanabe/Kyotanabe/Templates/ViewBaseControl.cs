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

[TemplatePart(Name = "PART_ListView", Type = typeof(ListView))]
[ContentProperty(Name = "AdditionalContent")]
public sealed partial class ViewBaseControl : Control
{
    private ListView? listView;

    public ViewBaseControl()
    {
        DefaultStyleKey = typeof(ViewBaseControl);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        listView = (ListView)GetTemplateChild("PART_ListView");

        listView.SelectionChanged += ListView_SelectionChanged;
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RemoveCommandParameter = ((ListView)sender).SelectedItem;
    }

    public object TableHeader
    {
        get => GetValue(TableHeaderProperty);
        set => SetValue(TableHeaderProperty, value);
    }

    private static readonly DependencyProperty TableHeaderProperty = DependencyProperty.Register(
        nameof(TableHeader),
        typeof(object),
        typeof(ViewBaseControl),
        new PropertyMetadata(default));

    public DataTemplate TableRowTemplate
    {
        get => (DataTemplate)GetValue(TableRowTemplateProperty);
        set => SetValue(TableRowTemplateProperty, value);
    }

    private static readonly DependencyProperty TableRowTemplateProperty = DependencyProperty.Register(
        nameof(TableRowTemplate),
        typeof(DataTemplate),
        typeof(ViewBaseControl),
        new(default(DataTemplate)));

    public object AdditionalContent
    {
        get => GetValue(AdditionalContentProperty);
        set => SetValue(AdditionalContentProperty, value);
    }

    private static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
        nameof(AdditionalContent),
        typeof(object),
        typeof(ViewBaseControl),
        new(default));

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    private static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(
        nameof(AddCommand),
        typeof(ICommand),
        typeof(ViewBaseControl),
        new PropertyMetadata(default(ICommand)));

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    private static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
        nameof(RemoveCommand),
        typeof(ICommand),
        typeof(ViewBaseControl),
        new PropertyMetadata(default(ICommand)));

    public ICommand LoadCommand
    {
        get => (ICommand)GetValue(LoadCommandProperty);
        set => SetValue(LoadCommandProperty, value);
    }

    private static readonly DependencyProperty LoadCommandProperty = DependencyProperty.Register(
        nameof(LoadCommand),
        typeof(ICommand),
        typeof(ViewBaseControl),
        new PropertyMetadata(default(ICommand)));

    public object RemoveCommandParameter
    {
        get => GetValue(RemoveCommandParameterProperty);
        set => SetValue(RemoveCommandParameterProperty, value);
    }

    private static readonly DependencyProperty RemoveCommandParameterProperty = DependencyProperty.Register(
        nameof(RemoveCommandParameter),
        typeof(object),
        typeof(ViewBaseControl),
        new PropertyMetadata(default));

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(object),
        typeof(ViewBaseControl),
        new(default));
}
