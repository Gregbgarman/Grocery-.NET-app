using System;
using Project4_1.Items;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Windows.Storage;
using System.IO;

namespace Project4_1.ViewModels
{
    
    public class MainViewModel: INotifyPropertyChanged
    {
       
        public ObservableCollection<Product> Inventory { get; set; }
        public ObservableCollection<Product> InventoryCopy { get; set; }
        

        public ObservableCollection<Product> SubInventory { get; set; }

        public Product SelectedProduct { get; set; }
        public shoppingcart usercart { get; set; }


        public double ItemCount { get; set; }

        public double Total { get; set; }

        public int PageNumber { get; set; }
        
        public String FinalTotal
        {
            get
            {
                
                return String.Format("{0:C}",Total);
               
            }
        }


        public MainViewModel(ObservableCollection<Product> Inventory,shoppingcart usercart)     
        {
            this.usercart = usercart;
            this.Inventory = Inventory;
            InventoryCopy = new ObservableCollection<Product>(Inventory);

            SubInventory = new ObservableCollection<Product>();
            LoadSubInventory();
           
        }

        
        public MainViewModel(ObservableCollection<Product> Inventory)
        {
            usercart = new shoppingcart();
            this.Inventory = Inventory;
            InventoryCopy = new ObservableCollection<Product>(Inventory);
            SubInventory = new ObservableCollection<Product>();           
            LoadSubInventory();
            
        }

        public void LoadSubInventory()          
        {
           SubInventory.Clear();
            ItemCount = 0;
            Total = 0;
            PageNumber = 0;

            if (Inventory.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Inventory[i].isByWeight())
                        SubInventory.Add(Inventory[i]);     
                    else
                        SubInventory.Add(Inventory[i]);

                }

            }
            else
            {

                for (int i = 0; i < Inventory.Count; i++)
                {
                    if (Inventory[i].isByWeight())
                        SubInventory.Add(Inventory[i]);
                    else
                        SubInventory.Add(Inventory[i]);

                }

            }

            CalculateTotal();
        }

        public void GoNextPage()
        {
            
            if (PageNumber == 0 && Inventory.Count > 10)
            {
                ItemCount = 0;
                NotifyPropertyChanged("ItemCount");
                SubInventory.Clear();
                PageNumber++;

                for (int i = 10; i < Inventory.Count; i++)
                {
                    if (Inventory[i].isByWeight())
                        SubInventory.Add(Inventory[i]);

                    else
                        SubInventory.Add(Inventory[i]);

                }

            }
        }

        public void GoPreviousPage()
        {
            
            if (PageNumber == 1)
            {
                ItemCount = 0;
                NotifyPropertyChanged("ItemCount");
                SubInventory.Clear();
                PageNumber--;

                for (int i = 0; i < 10; i++)
                {
                    if (Inventory[i].isByWeight())
                        SubInventory.Add(Inventory[i]);

                    else
                        SubInventory.Add(Inventory[i]);

                }

            }
        }      

        public void CalculateTotal()
        {
    
            double subtotal = 0;
            double price;
            for (int i = 0; i < usercart.Cart.Count; i++)
            {
                 price= usercart.Cart[i].Price;

                if (usercart.Cart[i].Price < 1)
                    price *= 10;

                subtotal = subtotal + price;
            }

            Total = subtotal;
            NotifyPropertyChanged();
            NotifyPropertyChanged("FinalTotal");
        }


        public void ChangeItemCount()       //called when user clicks on an inventory item
        {
            if (SelectedProduct != null)
            {
                                                                 
                if (usercart.Contains(SelectedProduct.ID))          
                    ItemCount = usercart.Cart[usercart.GetIndex(SelectedProduct.ID)].getUnits();

                else
                    ItemCount = 0;                          //ItemCount is shown in UI

                NotifyPropertyChanged("ItemCount");

            }

        }
                           

        public async void Add()
        {

          if (SelectedProduct == null)
            {
                return;
            }

            if  (usercart.Contains(SelectedProduct.ID))
                {

                var handler = new WebRequestHandler();
                ItemCount = usercart.Cart[usercart.GetIndex(SelectedProduct.ID)].getUnits();

                if (SelectedProduct.isByWeight())           
                {
                    var tempitem = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/IncrementItem", usercart.GetIndex(SelectedProduct.ID)));
                    usercart.Cart[usercart.GetIndex(SelectedProduct.ID)] = tempitem;   
                    ItemCount += 0.1;
                }


                else
                {
                                                   
                    var tempitem = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/IncrementItem", usercart.GetIndex(SelectedProduct.ID)));
                    usercart.Cart[usercart.GetIndex(SelectedProduct.ID)] = tempitem;                    
                    ItemCount += 1;
                }
                
            }
            else
            {
                var handler = new WebRequestHandler();

                if (SelectedProduct.isByWeight())
                {
                                                      
                    var tempitem = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/AddItemByWeight", SelectedProduct));                    
                    usercart.Cart.Add(tempitem);                   
                    ItemCount = 0.1;
                }


                else
                {
                   
                    var tempitem = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/AddItemByQuantity", SelectedProduct));
                    usercart.Cart.Add(tempitem);                    
                    ItemCount = 1;
                }
            }


            NotifyPropertyChanged("ItemCount");
            CalculateTotal();         
        }

        public async void RemoveFromCart()
        {
            

            if (SelectedProduct == null)
            {
                return;
            }

            if (SelectedProduct.isByWeight() && usercart.Contains(SelectedProduct.ID))
            {
                var handler = new WebRequestHandler();
                double itemcount = usercart.Cart[usercart.GetIndex(SelectedProduct.ID)].getUnits();   
                    

                if (itemcount-0.1 < 0.1)
                {
                    var tempitem = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/RemoveItem", usercart.GetIndex(SelectedProduct.ID)));
                    usercart.Cart.RemoveAt(usercart.GetIndex(tempitem.ID));    
                    ItemCount = 0;
                }

                else
                {
                    var tempitem = JsonConvert.DeserializeObject<ProductByWeight>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/DecrementItem", usercart.GetIndex(SelectedProduct.ID)));                    
                    usercart.Cart[usercart.GetIndex(SelectedProduct.ID)]=tempitem;
                    ItemCount = tempitem.getUnits();
                }
            }

            else if (!SelectedProduct.isByWeight() && usercart.Contains(SelectedProduct.ID))        
            {
                var handler = new WebRequestHandler();                
                double itemcount = usercart.Cart[usercart.GetIndex(SelectedProduct.ID)].getUnits();

                if (itemcount-1 < 1)
                {
                    var tempitem = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/RemoveItem", usercart.GetIndex(SelectedProduct.ID)));
                    usercart.Cart.RemoveAt(usercart.GetIndex(tempitem.ID));         
                    ItemCount = 0;
                }

                else
                {
                    var tempitem = JsonConvert.DeserializeObject<ProductByQuantity>(await handler.Post("http://localhost/MyProjectAPI/shoppingcart/DecrementItem", usercart.GetIndex(SelectedProduct.ID)));
                    usercart.Cart[usercart.GetIndex(SelectedProduct.ID)] = tempitem;
                    ItemCount = tempitem.getUnits();
                }
            }

            NotifyPropertyChanged("ItemCount");
            CalculateTotal();            

        }

        public async void SearchItems(String Text)
        {
            ItemCount = 0;
            NotifyPropertyChanged("ItemCount");

            if (Text == "")                 //how to get inventory list to show all again as normal
            {
                Inventory.Clear();
                Inventory = InventoryCopy;
                LoadSubInventory();
                
            }
            else
            {
                
                var handler = new WebRequestHandler();
                var productsbyweight = JsonConvert.DeserializeObject<ObservableCollection<ProductByWeight>>(await handler.Post("http://localhost/MyProjectAPI/Search/SearchWeightedItems", Text));
                var productsbyquantity = JsonConvert.DeserializeObject<ObservableCollection<ProductByQuantity>>(await handler.Post("http://localhost/MyProjectAPI/Search/SearchQuantityItems", Text));

                if (productsbyweight.Count > 0 || productsbyquantity.Count > 0)
                {

                    Inventory.Clear();
                    foreach (Product p in productsbyquantity)
                    {
                        Inventory.Add(p);
                    }

                    foreach (Product p in productsbyweight)
                    {
                        Inventory.Add(p);
                    }
                    LoadSubInventory();
                }
                else                    //user performs search that doesn't match anything
                {
                    SubInventory.Clear();
                    ItemCount = 0;
                    NotifyPropertyChanged("ItemCount");
                    
                }

            }
            

        }
    

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
