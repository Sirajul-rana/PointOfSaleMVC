using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class SalesManager
    {
        SalesGateway salesGateway = new SalesGateway();
        public List<Item> GetItems(string itemName)
        {
            return salesGateway.GetItems(itemName);
        }

        public string SaveStockOut(StockOut stockOut)
        {
            if (salesGateway.SaveStockOut(stockOut) > 0)
            {
                return "Stock out successfull";
            }
            else
            {
                return "Stock out failed";
            }
        }
    }
}