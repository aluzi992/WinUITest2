// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NotificationTest
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            SendNotification("窗口标题", "标题", "2023-03-12 14:53:22", "我的 意见再次", "oauser", "http://asdasdasd");
        }

        void SendNotification(string windowTitle, string title, string date, string idea, string sFrom, string url)
        {
            var builder = new AppNotificationBuilder()
                .AddArgument("Action", "NewToast")
                .AddArgument("ActionData", url);

            if (sFrom != "")
            {
                builder.SetAppLogoOverride(new Uri("d:\\photo\\228-01.jpg"), AppNotificationImageCrop.Circle);

            }

            builder.SetScenario(AppNotificationScenario.IncomingCall);
            builder.AddText(sFrom + " " + date.Substring(11, 5) + " " + windowTitle)
            .AddText(title)
            .AddText(idea);
            //var notify = builder.BuildNotification();
            //notify.ExpiresOnReboot = true;
            AppNotificationManager.Default.Show(builder.BuildNotification());
        }

    }
}
