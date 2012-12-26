using Cafe.Business.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(x => x.Name);
            Map(x => x.Code);
            Map(x => x.Category);
            Map(x => x.Price);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Table("Product");
        }

    }
}
