using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kyotanabe.Templates
{
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "Message", Type = typeof(TextBlock))]
    public sealed partial class ValidationTextBox : Control
    {
        private INotifyDataErrorInfo? oldDataContext;

        private TextBox? textBox;
        private TextBlock? textBlock;

        public ValidationTextBox()
        {
            DefaultStyleKey = typeof(ValidationTextBox);

            DataContextChanged += ValidationTextBox_DataContextChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            textBox = (TextBox)GetTemplateChild("TextBox");
            textBlock = (TextBlock)GetTemplateChild("Message");

            textBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = ((TextBox)sender).Text;
        }

        private void ValidationTextBox_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (oldDataContext != null)
                oldDataContext.ErrorsChanged -= OldDataContext_ErrorsChanged;

            if (DataContext is INotifyDataErrorInfo notifyDataError)
            {
                oldDataContext = notifyDataError;
                oldDataContext.ErrorsChanged += OldDataContext_ErrorsChanged;
            }

            RefreshErrors();
        }

        private void OldDataContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(ValidationTextBox),
            new(default(string)));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        private static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            nameof(Message),
            typeof(string),
            typeof(ValidationTextBox),
            new(default(string)));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        private static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidationTextBox),
            new(default(string)));

        public string Property
        {
            get => (string)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        private static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
            nameof(Property),
            typeof(string),
            typeof(ValidationTextBox),
            new(PropertyProperty, OnPropertyPropertyChanged));

        private static void OnPropertyPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not string str || string.IsNullOrWhiteSpace(str))
                return;

            ((ValidationTextBox)sender).RefreshErrors();
        }

        private void RefreshErrors()
        {
            if (Property is not string property ||
                DataContext is not INotifyDataErrorInfo dataErrorInfo ||
                textBox is not TextBox tBox ||
                textBlock is null)
                return;

            ValidationResult? result = dataErrorInfo.GetErrors(property).OfType<ValidationResult>().FirstOrDefault();

            if (result is null || result == ValidationResult.Success)
            {
                Message = string.Empty;
                Brush? brush = Application.Current.Resources["TextBoxBorderThemeBrush"] as Brush;
                if (brush != null)
                    textBox.BorderBrush = brush;
                return;
            }

            Message = result.ErrorMessage ?? "An unspecified error occurred.";

            Brush? themeDics = Application.Current.Resources["SystemErrorTextColor"] as Brush;
            if (themeDics != null)
                tBox.BorderBrush = themeDics;
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        private static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            nameof(PlaceholderText),
            typeof(string),
            typeof(ValidationTextBox),
            new(default(string)));
    }
}
