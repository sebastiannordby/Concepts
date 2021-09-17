using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ExtendedPurchaseOrderDto 
{    
    public class ExtendedPurchaseOrderDto
    {
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }

    public class PurchaseOrderDto 
    {
        public int Number { get; set ;}
        public string Name { get; set; }
    }
}