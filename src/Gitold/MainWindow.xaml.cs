﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Igorary.Xaml;

namespace Gitold
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            cbTheme.ItemsSource = (Application.Current as App).GetResourceNames("Theme");
            cbAccent.ItemsSource = (Application.Current as App).GetResourceNames("Accent");
            (Application.Current as App).SwitchResources("Theme", Properties.Settings.Default.Theme);
            (Application.Current as App).SwitchResources("Accent", Properties.Settings.Default.Accent);
            cbTheme.SelectedValue = Properties.Settings.Default.Theme;
            cbAccent.SelectedValue = Properties.Settings.Default.Accent;
            updateButtons();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Normal;
        }

        private void updateButtons() {
            btnRestore.Visibility = WindowState == WindowState.Maximized ? Visibility.Visible : Visibility.Collapsed;
            btnMaximize.Visibility = WindowState == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
        }

        private void btnSetTheme_Click(object sender, RoutedEventArgs e) {
            (Application.Current as App).SwitchResources("Theme", Properties.Settings.Default.Theme = cbTheme.SelectedValue as string);
            (Application.Current as App).SwitchResources("Accent", Properties.Settings.Default.Accent = cbAccent.SelectedValue as string);
            Properties.Settings.Default.Save();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            updateButtons();
        }

        private void borderTitle_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
