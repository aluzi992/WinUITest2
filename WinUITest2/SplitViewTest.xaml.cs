// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using WinRT.Interop;
using static PInvoke.User32;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITest2
{
    public class NegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double v)
            {
                return -v;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IconAndTextLabel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string icon = "";
        public string Icon
        {
            get
            {
                Debug.WriteLine("getting Icon" + icon);
                return icon;
            }
            set
            {
                if (value != icon)
                {
                    icon = value;
                    FirePropertyChanged("Icon");
                }
            }
        }
        public string Text { get; set; }

        public IconAndTextLabel()
        {
            Icon = "";
            Text = "";
        }

        private void FirePropertyChanged(string propertyName)
        {
            Debug.WriteLine("FirePropertyChanged running............."+propertyName);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplitViewTest : Window
    {
        IntPtr m_hwnd;
        private AppWindow? ThisWindow;
        public ObservableCollection<IconAndTextLabel> Items { get; set; }
        public string TestImageUrl { get; set; }= "https://images.pexels.com/photos/347926/pexels-photo-347926.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
        public string TestImageUrl2 { get; set; } = "https://images.pexels.com/photos/672101/pexels-photo-672101.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
        public SplitViewTest()
        {
            this.InitializeComponent();
            Items = new ObservableCollection<IconAndTextLabel> {
                new IconAndTextLabel { Icon = "\ue669", Text = "待办事宜" },
                new IconAndTextLabel { Icon = "\ue60c", Text = "经办事项" },
                new IconAndTextLabel { Icon = "\ue61b", Text = "我起草的" },
                new IconAndTextLabel { Icon = "\ue608", Text = "收藏夹" }};
            m_hwnd = WindowNative.GetWindowHandle(this);
            WindowId id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(m_hwnd);
            ThisWindow = AppWindow.GetFromWindowId(id);

            int size = 3072;
            Debug.WriteLine(String.Format("{0}K", size / 1024));
            //窗口大小
            int w = 1088;
            int h = 640;
            PInvoke.User32.SetWindowPos(m_hwnd, SpecialWindowHandles.HWND_TOP, 0, 0, w, h, SetWindowPosFlags.SWP_NOMOVE);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Msgbox($"X:{ThisWindow.Position.X} Y:{ThisWindow.Position.Y} Width:{ThisWindow.Size.Width} Height:{ThisWindow.Size.Height}");
            List<IconAndTextLabel> list = new List<IconAndTextLabel>
            {
                new IconAndTextLabel { Icon = "A", Text = "AAA" },
                new IconAndTextLabel { Icon = "B", Text = "BBB" },
                new IconAndTextLabel { Icon = "C", Text = "CCC" }
            };
            var result = list.FirstOrDefault(g => g.Icon == "DS");
            Debug.WriteLine(result);
        }

        async void Msgbox(string msg)
        {
            var messageDialog = new MessageDialog(msg, "提示");
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;
            var hwnd = WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(messageDialog, hwnd);
            await messageDialog.ShowAsync();
        }

        async void Confirm(string message, string title)
        {
            var messageDialog = new MessageDialog(message, title);
            messageDialog.Commands.Add(new UICommand("确定",null,"ok"));
            messageDialog.Commands.Add(new UICommand("取消",null,"cancel"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;
            var hwnd = WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(messageDialog, hwnd);
            var result = await messageDialog.ShowAsync();
            Debug.WriteLine(result);
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(listView1.SelectedItem);
        }
    }
}
