using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Map(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.EmailAddress);
            Map(x => x.TelNumber);
            Map(x => x.MobileNumber);
            Table("Customer");
        }
    }
}
