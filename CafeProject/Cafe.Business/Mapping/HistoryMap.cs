using Cafe.Business.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class HistoryMap : ClassMap<History>
    {
        public HistoryMap()
        {
            Map(x => x.Username);
            Map(x => x.Category);
            Map(x => x.TransactionMessage);
            Map(x => x.ActionDateTime);
            Table("History");
        }
    }
}
