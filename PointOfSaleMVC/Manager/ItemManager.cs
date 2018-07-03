using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class ItemManager
    {
        ItemGateway itemGateway = new ItemGateway();
        public string SaveItem(Item item)
        {
            return itemGateway.SaveItem(item);
        }

        public List<Item> GetAllItems()
        {
            return itemGateway.GetAllItems();
        }
    }
}