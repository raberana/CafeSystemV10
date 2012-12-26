using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Map(x => x.Name);
            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.PasswordSalt);
            Map(x => x.EmployeeId);
            Map(x => x.Role);
            References(x => x.Branch).Column("BranchCode");
            Table("Employee");
        }
    }
}
