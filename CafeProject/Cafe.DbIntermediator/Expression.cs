using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cafe.DbIntermediator
{
    [Serializable]
    public class Expression
    {
        private ExpressionType expressionType;
        private string propertyName;
        private object propertyValue;
        private ICollection values;

        public Expression(ExpressionType ExpressionType, string PropertyName, object PropertyValue)
        {
            expressionType = ExpressionType;
            propertyName = PropertyName;
            propertyValue = PropertyValue;
        }

        public Expression(ExpressionType ExpressionType, string PropertyName, ICollection PropertyValues)
        {
            expressionType = ExpressionType;
            propertyName = PropertyName;
            values = PropertyValues;
        }

        public Expression(ExpressionType ExpressionType)
        {
            expressionType = ExpressionType;
            propertyName = string.Empty;
            propertyValue = null;
        }

        public ExpressionType ExpressionType
        {
            get { return expressionType; }
        }

        public string PropertyName
        {
            get { return propertyName; }
        }

        public object PropertyValue
        {
            get { return propertyValue; }
        }

        public ICollection PropertyValues
        {
            get { return values; }
        }
    }
}
