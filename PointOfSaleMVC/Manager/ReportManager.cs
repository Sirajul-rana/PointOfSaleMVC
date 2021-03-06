﻿using System;
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

        /*
         *  Purchase report code starts here 
         */
        public List<StockIn> GetPurchaseByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportGateway.GetPurchaseByBranchId(stockIn);
            return stockIns;
        }
        /*
         *  Purchase report code ends here 
         */

        /*
         *  Stock report code starts here 
         */
        public List<StockIn> GetStocksByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportGateway.GetStocksByBranchId(stockIn);
            return stockIns;
        }
        /*
         *  Stock report code ends here 
         */

    }
}