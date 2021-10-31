using System;
using Project4_1.Items;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Project4_1.Items
{
    public class shoppingcart : INotifyPropertyChanged
    {
        public shoppingcart()
        {
            Cart = new ObservableCollection<Product>();
        }
        public ObservableCollection<Product> Cart { get; set; }        //cart stores a List of Products as its container


        public bool Contains(int id)                //method used to determine if an item is already in the cart by looking for the ID
        {
            for (int i = 0; i < Cart.Count; i++)
            {
                if (Cart[i].ID == id)
                    return true;

            }
            return false;

        }

        public int GetIndex(int id)             //method runs if above method returns true and returns the index of the item already in cart
        {
            for (int i = 0; i < Cart.Count; i++)
            {
                if (Cart[i].ID == id)
                    return i;

            }

            return 0;       //will never be returned-program required something here
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
