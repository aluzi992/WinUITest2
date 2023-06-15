// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITest2
{
    public class People
    {
        public string Name { get; set; } = "";
        public string Sex { get; set; } = "";
        public ObservableCollection<SubPeople> Items { get; set; } = new ObservableCollection<SubPeople>();
    }
    public class SubPeople
    {
        public string Detail { get; set; } = "";
        public string Remark { get; set; } = "";
    }
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public List<People> Peoples { get; set; } = new List<People> { new People { Name = "����", Sex = "��" }, new People { Name = "����", Sex = "Ů" },
           new People { Name = "Lkja����ɽ�Ŀռ䰮���˴򿪰���˿�Ŀ������ֻ�����ɽ�Ŀռ䰡�յ�������",
               Sex = "Ů",
               Items=new ObservableCollection<SubPeople>{new SubPeople{Detail="xxxxxxxxxxx",Remark="yyyyyyyy" },
               new SubPeople{Detail="����ɽ�Ŀռ�",Remark="ǡΪuŶ���"}} } };
        //public ObservableCollection<People> Items = new ObservableCollection<People>();
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
