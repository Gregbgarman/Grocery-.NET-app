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
using Project4_1.Items;
using Project4_1.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Project4_1.ViewCartPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckoutPage : Page
    {
        public shoppingcart FinalShoppingCart { get; set; }

        public CheckoutPage()
        {
            this.InitializeComponent();
            FinalShoppingCart = new shoppingcart();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.FinalShoppingCart = (shoppingcart)e.Parameter;
            
            DataContext = new CheckoutViewModel(FinalShoppingCart);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);

        }
    }
}
