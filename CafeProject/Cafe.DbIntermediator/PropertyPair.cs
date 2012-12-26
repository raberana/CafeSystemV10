using System;

namespace Cafe.DbIntermediator
{
    [Serializable]
    public class PropertyPair
    {
        private string _propertyName;
        private object _propertyValue;

        public string PropertyName
        {
            get { return this._propertyName; }
            set { this._propertyName = value; }
        }

        public object PropertyValue
        {
            get { return this._propertyValue; }
            set { this._propertyValue = value; }
        }

        public PropertyPair(string propertyName, object propertyValue)
        {
            this._propertyName = propertyName;
            this._propertyValue = propertyValue;
        }
    }
}
