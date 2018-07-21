using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointOfSaleMVC.DBL;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Manager
{
    public class ReportManager
    {
        ReportGateway reportGateway = new ReportGateway();
        public List<StockIn> GetStocksByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportGateway.GetStocksByBranchId(stockIn);
            return stockIns;
        }
    }
}