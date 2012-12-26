using Cafe.Business.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class ReferenceDataMap : ClassMap<ReferenceData>
    {
        public ReferenceDataMap()
        {
            Map(x => x.Name).Column("ReferenceName");
            Map(x => x.Code).Column("ReferenceCode");
            Map(x => x.Value1);
            Map(x => x.Value2);
            Map(x => x.Value3);
            Map(x => x.Value4);
            Map(x => x.Value5);
            Map(x => x.Value6);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Table("ReferenceData");
        }
    }
}
