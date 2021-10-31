using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Windows.Storage;

namespace Project4_1.Items
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Product
        {

            public String DisplayInfo
            {
            
               get
               {

                   return $"{Name}     \"{Description}\"" + "    " +string.Format("{0:C}", getUnitPrice()) +" Unit/Ounce";

               }
            
            }   
           
            public virtual double Price { get; set; }

            [JsonProperty]
            public string Name { get; set; }

             [JsonProperty]
            public string Description { get; set; }

             [JsonProperty]
            public int ID { get; set; }

            public abstract double getUnitPrice();          //accessor for private data in derived classes
            public abstract double getUnits();              //accessor for private data in derived classes
            public abstract bool isByWeight();              //used to determine what kind of product by looking at how it's measured

            public abstract void ModifyQuantity(double amount);     //used to modifty unit/ounce amount in derived classes

        }

    [JsonObject(MemberSerialization.OptIn)]
    public class ProductByQuantity : Product,INotifyPropertyChanged
        {

        public String NameCost
        {
            get
            {
                return Name + "     " + string.Format("{0:C}", Price);
                
            }

        }


        public String NameQuantity
        {

            get
            {
                return $"{Name} X {getUnits()} Units/Ounces";
            }

        }

        public double Count
        {
            get
            {
                return getUnits();
            }
           
            

        }

        public override bool isByWeight()
        {
                return false;
        }

        public override double Price
        {

                get
                {
                    return UnitPrice * Units;
                }

        }

        [JsonProperty]
        public double UnitPrice { get; set; }

        [JsonProperty]
        public int Units { get; set; }

            public override double getUnits()
            {
            
                return Units;
            }
            public override double getUnitPrice()
            {
                return UnitPrice;
            }

            public override void ModifyQuantity(double amount)
            {

                Units = Units + (int)amount;
                if (Units <= 0)
                     Units = 0;

            NotifyPropertyChanged();
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("NameQuantity");
            NotifyPropertyChanged("Price");

        }


            public ProductByQuantity(string name, string description, int id, double unitprice, int units)
            {
                Name = name;
                Description = description;
                ID = id;
                UnitPrice = unitprice;
                Units = units;
            }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ProductByWeight : Product,INotifyPropertyChanged
        {

        public String NameCost
        {
            get
            {
                return Name + "     " + string.Format("{0:C}", Price*10);
                
            }
            set
            {
                NameCost = value;
            }

        }


        public String NameQuantity
        {

            get
            {
                return $"{Name} X {getUnits()} Units/Ounces";
            }

        }


        public double Count
        {
            get
            {
                return getUnits();
            }

        }

        public override bool isByWeight()
            {
                return true;
            }

        public override double Price
        {

            get
            {
                return PricePerOunce * Ounces;

            }

        }

        [JsonProperty]
        public double PricePerOunce { get; set; }

        [JsonProperty]
        public double Ounces { get; set; }
            

       public override double getUnits()
       {
            return Ounces;
            
       }

       public override double getUnitPrice()
       {   return PricePerOunce;

       }

       public override void ModifyQuantity(double amount)
       {

           Ounces = Ounces + amount;
             if (Ounces <= 0)
               Ounces = 0;
            
            NotifyPropertyChanged();
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("NameQuantity");
            NotifyPropertyChanged("Price");

        }

        public ProductByWeight(string name, string description, int id, double priceperounce, double ounces)
        {
                Name = name;
                Description = description;
                ID = id;
                PricePerOunce = priceperounce;
                Ounces = ounces;
            
         }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
