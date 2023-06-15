// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITest2
{
    public sealed class BINPersonPicture2 : Control
    {
        bool errorPlaceholderSetted = false;
        public ImageSource PortraitUrl
        {
            get => (ImageSource)GetValue(PortraitUrlProperty);
            set => SetValue(PortraitUrlProperty, value);
        }

        public string PersonName
        {
            get => (string)GetValue(PersonNameProperty);
            set => SetValue(PersonNameProperty, value);
        }

        public ImageSource ErrorPlaceholder
        {
            get => (ImageSource)GetValue(ErrorPlaceholderProperty);
            set => SetValue(ErrorPlaceholderProperty, value);
        }
        public static DependencyProperty PortraitUrlProperty = DependencyProperty.Register(nameof(PortraitUrl), typeof(ImageSource), typeof(BINPersonPicture2), new PropertyMetadata(default(ImageSource), new PropertyChangedCallback(OnPortraitUrlChanged)));
        public static DependencyProperty PersonNameProperty = DependencyProperty.Register(nameof(PersonName), typeof(string), typeof(BINPersonPicture2), new PropertyMetadata(default(string), new PropertyChangedCallback(OnPersonNameChanged)));
        public static DependencyProperty ErrorPlaceholderProperty = DependencyProperty.Register(nameof(ErrorPlaceholder), typeof(ImageSource), typeof(BINPersonPicture2), new PropertyMetadata(default(ImageSource), new PropertyChangedCallback(OnErrorPlaceholderChanged)));

        public BINPersonPicture2()
        {
            this.DefaultStyleKey = typeof(BINPersonPicture2);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Debug.WriteLine("OnApplyTemplate running..........");            
            if (GetTemplateChild("portraitBrush") is ImageBrush ThisBrush)
            {
                ThisBrush.ImageFailed += (s, e) =>
                {
                    if (!errorPlaceholderSetted)
                    {
                        errorPlaceholderSetted = true;
                        if (ErrorPlaceholder != null)
                            ThisBrush.ImageSource = ErrorPlaceholder;
                    }
                };
            }
        }

        private static void OnPortraitUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("OnPortraitUrlChanged running....................");
        }

        private static void OnPersonNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnErrorPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }

}
