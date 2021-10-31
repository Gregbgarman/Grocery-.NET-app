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
using Project4_1.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Project4_1.Items;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Project4_1.ViewCartPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 



    public sealed partial class ShoppingCartPage : Page
    {

        public shoppingcart TheShoppingCart { get; set; }
        
             
        public ShoppingCartPage()
        {
            this.InitializeComponent();
            
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.TheShoppingCart= (shoppingcart)e.Parameter;
            DataContext = new ShoppingCartViewModel(TheShoppingCart);

        }


        

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

            (DataContext as ShoppingCartViewModel).ClearCart();

        }


        private void Return_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(MainPage), (DataContext as ShoppingCartViewModel).Theusercart);


        }

        private void ListBox_SelectionChangedd(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as ShoppingCartViewModel).RemoveFromCart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(CheckoutPage), (DataContext as ShoppingCartViewModel).Theusercart);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            (DataContext as ShoppingCartViewModel).GoPreviousPage();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (DataContext as ShoppingCartViewModel).GoNextPage();
        }
    }
}
