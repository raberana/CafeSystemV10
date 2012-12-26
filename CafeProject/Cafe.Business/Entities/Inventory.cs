using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Entities
{
    public class Inventory
    {
        private Branch _branch;
        private Employee _employee;
        private string _itemCode;
        private string _itemName;
        private DateTime _dateOfPurchase;
        private string _remarks;
        private double _quantity;
        private double _amount;

        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }

        public virtual Employee Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        public virtual string ItemCode
        {
            get { return _itemCode; }
            set { _itemCode = value; }
        }

        public virtual string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
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
