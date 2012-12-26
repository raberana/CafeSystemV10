using Cafe.Business.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class SalesMap : ClassMap<Sales>
    {
        public SalesMap()
        {
            References(x => x.Product);
            References(x => x.Customer);
            References(x => x.Branch);
            Map(x => x.DateTimePurchase);
            Map(x => x.Quantity);
            Map(x => x.Amount);
            Map(x => x.Remarks);
            Table("Sales");
        }
    }
}
