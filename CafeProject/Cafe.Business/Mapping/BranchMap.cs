using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class BranchMap:ClassMap<Branch>
    {
        public BranchMap()
        {
            Map(x => x.Address);
            Map(x => x.Name);
            Map(x => x.Code);
            Table("Branch");
        }
    }
}
