using System;
using System.Collections.Generic;

namespace Cafe.DbIntermediator
{
    [Serializable]
    internal class ExpressionContainer : Expression
    {
        private IList<Expression> _subExpressions;

        public IList<Expression> SubExpressions
        {
            get { return _subExpressions; }
        }

        public ExpressionContainer(ExpressionType ExpressionType, IList<Expression> exps)
            : base(ExpressionType)
        {
            _subExpressions = exps;
        }
    }
}
