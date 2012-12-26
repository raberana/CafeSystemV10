using Cafe.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Entities
{
    public class Product
    {
        private ProductCategory _category;
        private string _code;
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private double _price;

        public virtual ProductCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public virtual double Price
        {
            get { return _price; }
            set { _price = value; }
        }

    }
}
