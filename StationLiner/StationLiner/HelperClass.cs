using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationLinerMVC
{
    public class HelperClass
    {
        public List<CurrentProduct> currentProductList = new List<CurrentProduct>();

        public List<CurrentProduct> getCurrentProduct()
        {
            return currentProductList;
        }
        public void setCurrentProduct(CurrentProduct currentProduct)
        {
            currentProductList.Add(currentProduct);
        }
    }
    public class CurrentProduct //Not being used maintained for future reference
    {
        public string stationName { get; set; }
        public string productName { get; set; }
        public double productPrice { get; set; }
    }
}