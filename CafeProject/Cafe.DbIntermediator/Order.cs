using System;

namespace Cafe.DbIntermediator
{
    [Serializable]
    internal class Order
    {
        private readonly OrderType orderType;
        private readonly string propertyName;

        public Order(OrderType OrderType, string PropertyName)
        {
            orderType = OrderType;
            propertyName = PropertyName;
        }

        public OrderType OrderType
        {
            get { return orderType; }
        }

        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}
