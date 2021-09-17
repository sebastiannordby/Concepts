using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using ChangeDetection.Dtos;
using ChangeDetection.Models;

namespace ChangeDetection.Models
{   
    public class EditPurchaseOrder : CMDEntityEditBase<ExtendedPurchaseOrderDto, EditPurchaseOrder>
    {
        public EditProperty<string> Name { get; private set; }
        public EditProperty<int> Number { get; private set; }

        public EditPurchaseOrder(Guid companyId) : base(companyId)
        {
        }

        public EditPurchaseOrder(ExtendedPurchaseOrderDto existingModel) : base(existingModel)
        {
        }

        protected override void SetupProperties()
        {
            Name = AssignProp(x => x.PurchaseOrder.Name);
            Number = AssignProp(x => x.PurchaseOrder.Number);
        }
    }
}