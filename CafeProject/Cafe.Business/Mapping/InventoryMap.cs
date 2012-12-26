using Cafe.Business.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class InventoryMap : ClassMap<Inventory>
    {
        public InventoryMap()
        {
            References(x => x.Branch).Column("BranchCode");
            References(x => x.Employee).Column("EmployeeId");
            Map(x => x.ItemCode);
            Map(x => x.ItemName);
            Map(x => x.DateTimePurchase);
            Map(x => x.Remarks);
            Map(x => x.Quantity);
            Map(x => x.Amount);
            Table("Inventory");
        }
    }
}
