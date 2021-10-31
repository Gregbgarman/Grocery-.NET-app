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
using Project4_1.ViewCartPage;
using Newtonsoft.Json;
using Windows.Storage;
using Project4_1.Items;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Project4_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public shoppingcart TheShoppingCart { get; set; }       
        public ObservableCollection<Product> Inventory { get; set; }

        public MainPage()
        {
            Initialize(0);
                       
        }


        public void Initialize(int flag)
        {
            this.InitializeComponent();
            Inventory = new ObservableCollection<Product>();

            TheShoppingCart = new shoppingcart();               //Product is an abstract type so using child classes

            var handler = new WebRequestHandler();
            var productsbyquantity = JsonConvert.DeserializeObject<List<ProductByQuantity>>(handler.Get("http://localhost/MyProjectAPI/Product/getproductsbyquantity").Result);
            var productsbyweight = JsonConvert.DeserializeObject<List<ProductByWeight>>(handler.Get("http://localhost/MyProjectAPI/Product/getproductsbyweight").Result);

            
            var cartweightitems = JsonConvert.DeserializeObject<ObservableCollection<ProductByWeight>>(handler.Get("http://localhost/MyProjectAPI/shoppingcart/GetCartItemsByWeight").Result);
            var cartquantityitems = JsonConvert.DeserializeObject<ObservableCollection<ProductByQuantity>>(handler.Get("http://localhost/MyProjectAPI/shoppingcart/GetCartItemsByQuantity").Result);

            foreach (ProductByQuantity p in cartquantityitems)
            {
                
                TheShoppingCart.Cart.Add(p);
            }

            foreach (ProductByWeight p in cartweightitems)
            {
                TheShoppingCart.Cart.Add(p);
            }


            foreach (ProductByQuantity p in productsbyquantity)
            {
                Inventory.Add(p);
            }

            foreach (ProductByWeight p in productsbyweight)
            {
                Inventory.Add(p);
            }

     
            if (flag == 0 && TheShoppingCart.Cart.Count==0)
                DataContext = new MainViewModel(Inventory);
            else if (flag == 0 && TheShoppingCart.Cart.Count != 0)
                DataContext = new MainViewModel(Inventory,TheShoppingCart);
            else
                return;

        }




        protected override void OnNavigatedTo(NavigationEventArgs e)        //so user can come back from cart page and add items
        {
            
            if (e.Parameter.ToString().Equals( "Project4_1.Items.shoppingcart"))           
            {
                
                Initialize(1);
                base.OnNavigatedTo(e);
                this.TheShoppingCart = (shoppingcart)e.Parameter;
                DataContext = new MainViewModel(Inventory, TheShoppingCart);

            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MainViewModel).ChangeItemCount();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            (DataContext as MainViewModel).Add();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).RemoveFromCart();
        }
                                                                        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(ShoppingCartPage), (DataContext as MainViewModel).usercart);

        }
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).GoNextPage();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).GoPreviousPage();
        }

        private void Get_Text_Click(object sender, RoutedEventArgs e)
        {
            String Text = usersearchbox.Text.ToString();
            (DataContext as MainViewModel).SearchItems(Text);
        }


    }
}
