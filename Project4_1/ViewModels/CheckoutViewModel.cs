using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project4_1.Items;
using Project4_1.ViewModels;
using Project4_1.ViewCartPage;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;

namespace Project4_1.ViewModels
{
   
    class CheckoutViewModel : INotifyPropertyChanged
    {
        public shoppingcart FinalUserCart { get; set; }
        
        public CheckoutViewModel(shoppingcart thecart)
        {
            FinalUserCart = thecart;           
            GetReceipt();
            
        }
               
        public void GetReceipt()
        {
            var handler = new WebRequestHandler();          
            string TheReceipt = handler.Get("http://localhost/MyProjectAPI/shoppingcart/GetReceipt").Result;

            
            using (StreamWriter sw = new StreamWriter(@"C:\Users\grego\AppData\Local\Packages\e3e112c4-31d0-43dc-be85-fd1c463cb332_c8n7c6grcnbs0\LocalState\myreceipt.txt"))
            {
                sw.WriteLine(TheReceipt);
                sw.Close();

           }

        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}
