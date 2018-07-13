using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class PurchaseManager
    {
        PurchaseGateway purchaseGateway = new PurchaseGateway();
        public List<Employee> GetEmployees(int branchId)
        {
            return purchaseGateway.GetEmployees(branchId);
        }

        public string SaveStockIn(StockIn stockIn)
        {
            if (purchaseGateway.SaveStockIn(stockIn) > 0)
            {
                return "Stock in successfull";
            }
            else
            {
                return "Stock in failed";
            }
        }
    }
}