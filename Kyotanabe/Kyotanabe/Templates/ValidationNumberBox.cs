using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe.Templates
{
    [TemplatePart(Name = "Value", Type = typeof(NumberBox))]
    [TemplatePart(Name = "Message", Type = typeof(TextBlock))]
    public sealed partial class ValidationNumberBox : Control
    {
        private NumberBox? numberBox;
        private TextBlock? itemsRepeater;

        private INotifyDataErrorInfo? oldDataContext;

        public ValidationNumberBox()
        {
            DefaultStyleKey = typeof(ValidationNumberBox);

            DataContextChanged += ValidationNumberBox_DataContextChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            numberBox = (NumberBox)GetTemplateChild("Value");
            itemsRepeater = (TextBlock)GetTemplateChild("Message");

            numberBox.ValueChanged += NumberBox_ValueChanged;

            RefreshErrors();
        }

        private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            Value = sender.Value;

            RefreshErrors();
        }

        private void ValidationNumberBox_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (oldDataContext != null)
                oldDataContext.ErrorsChanged -= OldDataContext_ErrorsChanged;

            if (DataContext is INotifyDataErrorInfo notifyDataError)
            {
                oldDataContext = notifyDataError;
                oldDataContext.ErrorsChanged += OldDataContext_ErrorsChanged;
            }
        }

        private void OldDataContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(ValidationNumberBox),
            new(0));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        private static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidationNumberBox),
            new(default(string)));

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        private static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            nameof(PropertyName),
            typeof(string),
            typeof(ValidationNumberBox),
            new(PropertyNameProperty, OnPropertyNamePropertyChanged));

        private static void OnPropertyNamePropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is not string v || string.IsNullOrWhiteSpace(v))
                return;

            ((ValidationNumberBox)sender).RefreshErrors();
        }

        private void RefreshErrors()
        {
            if (numberBox is null ||
                itemsRepeater is null ||
                DataContext is not INotifyDataErrorInfo dataContext ||
                string.IsNullOrWhiteSpace(PropertyName))
                return;

            ValidationResult? result = dataContext.GetErrors(PropertyName).OfType<ValidationResult>().FirstOrDefault(e => e.ErrorMessage is not null);
            if (result == null)
            {
                itemsRepeater.Text = string.Empty;
                return;
            }

            if (result.ErrorMessage is not null)
                itemsRepeater.Text = result.ErrorMessage;
        }
    }
}
