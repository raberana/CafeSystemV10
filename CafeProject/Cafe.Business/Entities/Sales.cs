using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Entities
{
    public class Sales
    {
        private Branch _branch;
        private Customer _customer;
        private Product _product;
        private DateTime _dateOfPurchase;
        private string _remarks;
        private double _quantity;
        private double _amount;

        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }

        public virtual Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public virtual Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public virtual DateTime DateTimePurchase
        {
            get { return _dateOfPurchase; }
            set { _dateOfPurchase = value; }
        }

        public virtual string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public virtual double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public virtual double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
