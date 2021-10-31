using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Project4_1.Items;
using Newtonsoft.Json;

namespace Project4_1.ViewModels
{
    public class ShoppingCartViewModel : INotifyPropertyChanged
    {
        public String FinalTotal
        {
            get
            {

                return "Cart Total  " + String.Format("{0:C}", Total);

            }
        }

        public double Total { get; set; }

        public shoppingcart SubCart { get; set; }

        public Product TheSelectedProduct { get; set; }
        public shoppingcart Theusercart { get; set; }

        public int PageNumber { get; set; }

        public ShoppingCartViewModel(shoppingcart thecart)
        {

            Theusercart = new shoppingcart();
            SubCart = new shoppingcart();                                                        
            PageNumber = 0;

            Theusercart.Cart = thecart.Cart;
           
            LoadSubCart();
            CalculateTotal();
        }

       

        public async void ClearCart()
        {
            var handler = new WebRequestHandler();
            var tempitem = JsonConvert.DeserializeObject<int>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/ClearCart", 0));

            SubCart.Cart.Clear();
            Theusercart.Cart.Clear();
            CalculateTotal();
            NotifyPropertyChanged("Theusercart.Cart");

        }


        public void LoadSubCart()
        {
            SubCart.Cart.Clear();

            if (Theusercart.Cart.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Theusercart.Cart[i].isByWeight())                        
                        SubCart.Cart.Add(new ProductByWeight(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), Theusercart.Cart[i].getUnits()));
                    else
                        SubCart.Cart.Add(new ProductByQuantity(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), (int)Theusercart.Cart[i].getUnits()));
                        
                }

            }
            else
            {

                for (int i = 0; i < Theusercart.Cart.Count; i++)
                {
                    if (Theusercart.Cart[i].isByWeight())
                        SubCart.Cart.Add(new ProductByWeight(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), Theusercart.Cart[i].getUnits()));
                        
                    else
                        SubCart.Cart.Add(new ProductByQuantity(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), (int)Theusercart.Cart[i].getUnits()));
                        

                }

            }
            

        }


        public void GoNextPage()
        {
            if (PageNumber==0 && Theusercart.Cart.Count>10)
            {

                SubCart.Cart.Clear();
                PageNumber++;

                for (int i = 10; i < Theusercart.Cart.Count; i++)
                {
                    if (Theusercart.Cart[i].isByWeight())
                        SubCart.Cart.Add(new ProductByWeight(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), Theusercart.Cart[i].getUnits()));
                        

                    else
                        SubCart.Cart.Add(new ProductByQuantity(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), (int)Theusercart.Cart[i].getUnits()));
                        

                }

            }
        }

        public void GoPreviousPage()
        {
            if (PageNumber==1)
            {

                SubCart.Cart.Clear();
                PageNumber--;

                for (int i = 0; i < 10; i++)
                {
                    if (Theusercart.Cart[i].isByWeight())
                        SubCart.Cart.Add(new ProductByWeight(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), Theusercart.Cart[i].getUnits()));
                       

                    else
                        SubCart.Cart.Add(new ProductByQuantity(Theusercart.Cart[i].Name, Theusercart.Cart[i].Description, Theusercart.Cart[i].ID, Theusercart.Cart[i].getUnitPrice(), (int)Theusercart.Cart[i].getUnits()));
                       

                }

            }
        }

        public async void RemoveFromCart()
        {

            if (TheSelectedProduct == null)
            {
                return;
            }

            if (SubCart.Contains(TheSelectedProduct.ID))
            {
                var handler = new WebRequestHandler();

                if (TheSelectedProduct.isByWeight())
                {
                    var ItemToDec = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/DecrementItem", Theusercart.GetIndex(TheSelectedProduct.ID)));
                    Theusercart.Cart[Theusercart.GetIndex(ItemToDec.ID)].ModifyQuantity(-0.1);
                    SubCart.Cart[SubCart.GetIndex(ItemToDec.ID)].ModifyQuantity(-0.1);
                    

                    if (SubCart.Cart[SubCart.GetIndex(TheSelectedProduct.ID)].getUnits() < 0.1)
                    {
                        var ItemToRemove = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/RemoveItem", Theusercart.GetIndex(TheSelectedProduct.ID)));
                        Theusercart.Cart.RemoveAt(Theusercart.GetIndex(ItemToRemove.ID));                       
                        

                        LoadSubCart();
                        if (PageNumber == 1)
                        {
                            PageNumber = 0;
                            GoNextPage();
                        }

                    }

                }

                else if (!TheSelectedProduct.isByWeight())
                {
                    var ItemToDec = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/DecrementItem", Theusercart.GetIndex(TheSelectedProduct.ID)));
                    Theusercart.Cart[Theusercart.GetIndex(ItemToDec.ID)].ModifyQuantity((double)-1);
                    SubCart.Cart[SubCart.GetIndex(ItemToDec.ID)].ModifyQuantity((double)-1);
                    

                    if (SubCart.Cart[SubCart.GetIndex(TheSelectedProduct.ID)].getUnits() == 0)
                    {
                        var ItemToRemove = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/RemoveItem", Theusercart.GetIndex(TheSelectedProduct.ID)));
                        Theusercart.Cart.RemoveAt(Theusercart.GetIndex(ItemToRemove.ID));                      
                        

                        LoadSubCart();
                        if (PageNumber == 1)
                        {
                            PageNumber = 0;
                            GoNextPage();
                        }
                    }

                   
                }

            }
            TheSelectedProduct = null;

            NotifyPropertyChanged("TheSelectedProduct");
            NotifyPropertyChanged("NameQuantity");
           
            CalculateTotal();

        }

        public void CalculateTotal()
        {

            double subtotal = 0;
            for (int i = 0; i < Theusercart.Cart.Count; i++)
            {
                double price = Theusercart.Cart[i].Price;

                if (Theusercart.Cart[i].Price < 1)
                    price *= 10;

                subtotal = subtotal + price;
            }

            Total = subtotal;
            NotifyPropertyChanged();
            NotifyPropertyChanged("FinalTotal");
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
