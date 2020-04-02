﻿using StockChecker.UWP.Helpers;
using StockChecker.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StockChecker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = DataContext as MainPageViewModel;
            viewModel.UserRole = e.Parameter.ToString();
        }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainPageViewModel(
                new HttpClientHelper(new Uri("https://localhost:44371")));
            DataContext = ViewModel;
        }

        public MainPageViewModel ViewModel { get; set; }
    }
}
