﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using 夏日;
using 夏日.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 倒计时
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Festival : Page
    {
        public double MinMyNav = MainPage.Current.MyNav.CompactModeThresholdWidth;
        public static Festival Current;
        public string str1, str2, str3;
        public Color str4;
        public FestivalDataViewModel ViewModel = new FestivalDataViewModel();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        //public FestivalData SelectedItem;
        public Festival()
        {
            this.InitializeComponent();
            Current = this;
            SetThemeColor();
        }

        private void SetThemeColor()
        {
            if (localSettings.Values["ThemeColor"] == null)
                localSettings.Values["ThemeColor"] = "CornflowerBlue";
            switch (localSettings.Values["ThemeColor"].ToString())
            {
                case "CornflowerBlue":
                    TC.Color = Colors.CornflowerBlue;
                    break;
                case "DeepSkyBlue":
                    TC.Color = Colors.DeepSkyBlue;
                    break;
                case "Orange":
                    TC.Color = Colors.Orange;
                    break;
                case "Crimson":
                    TC.Color = Colors.Crimson;
                    break;
                case "Gray":
                    TC.Color = Color.FromArgb(255, 73, 92, 105);
                    break;
                default:
                    break;
            }
        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var _item = (e.OriginalSource as FrameworkElement)?.DataContext as FestivalData;
            App.FestivalItem = _item;
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                All.Current.conn.Insert(new DataTemple() { Schedule_name = App.FestivalItem.Str1, CalculatedDate = App.FestivalItem.Str2, Date = App.FestivalItem.Str3, BgColor = App.FestivalItem.Str4.ToString(), TintOpacity = 0.7, IsTop = "0", AddTime = "" });
                All.Current.ViewModel.CustomDatas.Add(new CustomData() { Str1 = App.FestivalItem.Str1, Str2 = App.FestivalItem.Str2, Str3 = App.FestivalItem.Str3, Str4 = All.Current.ColorfulBrush(App.FestivalItem.Str4, 0.8), BackGroundColor = App.FestivalItem.Str4 });
                All.Current.NewTB.Visibility = Visibility.Collapsed;
                All.Current.NewTB2.Visibility = Visibility.Collapsed;
            }
            catch
            {
                MessageDialog AboutDialog = new MessageDialog("此日程已被添加，请勿重复添加~","提示");
                await AboutDialog.ShowAsync();
                return;
            }
            MainPage.Current.MyNav.SelectedItem = MainPage.Current.MyNav.MenuItems[0];
            Frame.Navigate(typeof(All));
            PopupNotice popupNotice = new PopupNotice("添加成功");
            popupNotice.ShowAPopup();
        }

        private string Calculator(string s1)
        {
            string str1 = s1;
            string str2 = DateTime.Now.ToShortDateString().ToString();
            string s2;
            DateTime d1 = Convert.ToDateTime(str1);
            DateTime d2 = Convert.ToDateTime(str2);
            DateTime d3 = Convert.ToDateTime(string.Format("{0}/{1}/{2}", d1.Year, d1.Month, d1.Day));
            DateTime d4 = Convert.ToDateTime(string.Format("{0}/{1}/{2}", d2.Year, d2.Month, d2.Day));
            int days = (d4 - d3).Days;
            if (days < 0)
            {
                days = -days;
                s2 = "还有" + days.ToString() + "天";
            }
            else
                s2 = "已过" + days.ToString() + "天";
            return s2;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var _item = (FestivalData)e.ClickedItem;
            App.FestivalItem = _item;
            str1 = _item.Str1;
            str2 = _item.Str2;
            str3 = _item.Str3;
            str4 = _item.Str4;
            MainPage.Current.SelectedPage = false;
            Frame.Navigate(typeof(Details));
        }

        public Color GetColor(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            return Color.FromArgb(a, r, g, b);
        }
    }
}
