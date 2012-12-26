using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Cafe.DbIntermediator
{
    [Serializable]
    public class Criteria : ICriteria
    {
        protected Type _persistentType;
        private readonly IList<Expression> _criterion = new List<Expression>();
        private readonly IList<Order> _orders = new List<Order>();
        private readonly Hashtable _aliases = new Hashtable();
        private IList<SubCriteria> _subCriteria = new List<SubCriteria>();

        #region Criteria constructors...
        public Criteria(Type persistentType)
        {
            _persistentType = persistentType;
            this.Projections = NHibernate.Criterion.Projections.ProjectionList();
        }
        #endregion

        #region Criteria properties...
        public IList<Expression> Criterion
        {
            get { return _criterion; }
        }

        private bool IsDistinct { get; set; }

        private int FirstResult { get; set; }

        private ProjectionList Projections { get; set; }

        private int MaxResult { get; set; }

        private NHibernate.Transform.IResultTransformer Transfomer { get; set; }

        public IList<SubCriteria> SubCriteria
        {
            get { return _subCriteria; }
            set { _subCriteria = value; }
        }

        #endregion

        public ICriteria Between(string propertyName, object lo, object hi)
        {
            _criterion.Add(new Expression(ExpressionType.Between, propertyName, new[] { lo, hi }));
            return this;
        }

        public ICriteria Count(string propertyName)
        {
            this.Projections.Add(NHibernate.Criterion.Projections.Count(propertyName));
            return this;
        }

        public ICriteria CountDistinct(string propertyName)
        {
            this.Projections.Add(NHibernate.Criterion.Projections.CountDistinct(propertyName));
            return this;
        }

        public ICriteria Distinct()
        {
            this.IsDistinct = true;
            return this;
        }

        public ICriteria Transform(NHibernate.Transform.IResultTransformer transformer)
        {
            this.Transfomer = transformer;
            return this;
        }

        private ICriterion Evaluate(Expression expression)
        {
            var expressionContainer = expression as ExpressionContainer;
            switch (expression.ExpressionType)
            {
                case ExpressionType.Between:
                    {
                        var parameters = expression.PropertyValues.Cast<object>().ToList();
                        if (parameters.Count > 1)
                            return NHibernate.Criterion.Restrictions.Between(expression.PropertyName, parameters[0], parameters[1]);
                    }
                    break;

                case ExpressionType.IsNull:
                    return NHibernate.Criterion.Restrictions.IsNull(expression.PropertyName);

                case ExpressionType.Eq:
                    return NHibernate.Criterion.Restrictions.Eq(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Ne:
                    return NHibernate.Criterion.Restrictions.Not(
                        NHibernate.Criterion.Restrictions.Eq(expression.PropertyName, expression.PropertyValue));

                case ExpressionType.Le:
                    return NHibernate.Criterion.Restrictions.Le(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Lt:
                    return NHibernate.Criterion.Restrictions.Lt(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Ge:
                    return NHibernate.Criterion.Restrictions.Ge(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Gt:
                    return NHibernate.Criterion.Restrictions.Gt(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Like:
                    return NHibernate.Criterion.Restrictions.Like(expression.PropertyName, expression.PropertyValue);

                case ExpressionType.Or:
                    if (expressionContainer == null)
                        return null;
                    return expressionContainer.SubExpressions.Aggregate<Expression, ICriterion>(null, (current, contained) =>
                        current != null ? NHibernate.Criterion.Restrictions.Or(current, this.Evaluate(contained)) : this.Evaluate(contained));

                case ExpressionType.And:
                    if (expressionContainer == null)
                        return null;
                    return expressionContainer.SubExpressions.Aggregate<Expression, ICriterion>(null, (current, contained) =>
                        current != null ? NHibernate.Criterion.Restrictions.And(current, this.Evaluate(contained)) : this.Evaluate(contained));

                case ExpressionType.EqOr:
                    Junction junction = null;
                    var list = (IList)expression.PropertyValue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var propertyPair = (PropertyPair)list[i];
                        if (i == 0)
                        {
                            junction =
                                NHibernate.Criterion.Restrictions.Disjunction().Add(
                                    NHibernate.Criterion.Restrictions.Eq(propertyPair.PropertyName, propertyPair.PropertyValue));
                        }
                        else if (junction != null)
                        {
                            junction = junction.Add(NHibernate.Criterion.Restrictions.Eq(
                                propertyPair.PropertyName, propertyPair.PropertyValue));
                        }
                    }
                    return junction;

                case ExpressionType.In:
                    return NHibernate.Criterion.Restrictions.In(expression.PropertyName, expression.PropertyValues);

                case ExpressionType.NotBetween:
                    {
                        var parameters = expression.PropertyValues.Cast<object>().ToList();
                        if (parameters.Count > 1)
                        {
                            return NHibernate.Criterion.Restrictions.Not(
                                NHibernate.Criterion.Restrictions.Between(expression.PropertyName, parameters[0], parameters[1]));
                        }
                    }
                    break;

                case ExpressionType.NotIn:
                    return NHibernate.Criterion.Restrictions.Not(
                        NHibernate.Criterion.Restrictions.In(expression.PropertyName, expression.PropertyValues));
            }
            return null;
        }

        public virtual IList ExecuteUsing(ISession session)
        {
            NHibernate.ICriteria nhibernateCriteria = EvaluateCriteria(session);
            return nhibernateCriteria.List();
        }

        public virtual IList<T> ExecuteUsing<T>(ISession session)
        {
            NHibernate.ICriteria nhibernateCriteria = EvaluateCriteria(session);
            return nhibernateCriteria.List<T>();
        }

        public NHibernate.ICriteria EvaluateCriteria(ISession session)
        {
            NHibernate.ICriteria nhibernateCriteria = session.CreateCriteria(_persistentType);

            if (this.Projections != null && this.Projections.Length > 0)
            {
                IProjection projection = this.Projections;
                if (this.IsDistinct)
                    projection = NHibernate.Criterion.Projections.Distinct(this.Projections);

                nhibernateCriteria.SetProjection(projection);
            }

            foreach (SubCriteria subCriteria in _subCriteria)
            {
                NHibernate.ICriteria crit = nhibernateCriteria.CreateCriteria(subCriteria.EntityProperty);
                foreach (Expression criterion in subCriteria._criterion)
                {
                    crit.Add(Evaluate(criterion));
                }
            }
            foreach (string key in _aliases.Keys)
            {
                nhibernateCriteria.CreateAlias(key, _aliases[key].ToString());
            }

            foreach (Expression criterion in _criterion)
            {
                nhibernateCriteria.Add(Evaluate(criterion));
            }
            foreach (Order order in _orders)
            {
                switch (order.OrderType)
                {
                    case OrderType.Asc:
                        nhibernateCriteria.AddOrder(NHibernate.Criterion.Order.Asc(order.PropertyName));
                        break;
                    case OrderType.Desc:
                        nhibernateCriteria.AddOrder(NHibernate.Criterion.Order.Desc(order.PropertyName));
                        break;
                }
            }

            if (MaxResult > 0)
                nhibernateCriteria.SetMaxResults(MaxResult);

            if (FirstResult >= 0)
                nhibernateCriteria.SetFirstResult(FirstResult);

            if (Transfomer != null)
                nhibernateCriteria.SetResultTransformer(Transfomer);

            return nhibernateCriteria;
        }

        public ICriteria Eq(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Eq, propertyName, val));
            return this;
        }

        public ICriteria GetProperty(string propertyName)
        {
            this.Projections.Add(NHibernate.Criterion.Projections.Property(propertyName));
            return this;
        }

        public ICriteria GroupProperty(string propertyName)
        {
            this.Projections.Add(NHibernate.Criterion.Projections.GroupProperty(propertyName));
            return this;
        }

        public ICriteria Ne(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Ne, propertyName, val));
            return this;
        }

        public ICriteria Le(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Le, propertyName, val));
            return this;
        }

        public ICriteria Lt(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Lt, propertyName, val));
            return this;
        }

        public ICriteria Ge(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Ge, propertyName, val));
            return this;
        }

        public ICriteria Gt(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Gt, propertyName, val));
            return this;
        }

        public ICriteria Like(string propertyName, object val)
        {
            _criterion.Add(new Expression(ExpressionType.Like, propertyName, val));
            return this;
        }

        public ICriteria Or(ICriteria contained)
        {
            var critContained = (Criteria)contained;
            IList<Expression> containedExpressions = critContained._criterion;
            _criterion.Add(new ExpressionContainer(ExpressionType.Or, containedExpressions));
            return this;
        }

        public ICriteria And(ICriteria contained)
        {
            var critContained = (Criteria)contained;
            IList<Expression> containedExpressions = critContained._criterion;
            _criterion.Add(new ExpressionContainer(ExpressionType.And, containedExpressions));
            return this;
        }

        public ICriteria EqOr(IList val)
        {
            _criterion.Add(new Expression(ExpressionType.EqOr, null, val));
            return this;
        }

        public ICriteria IsNull(string propertyName)
        {
            _criterion.Add(new Expression(ExpressionType.IsNull, propertyName, null));
            return this;
        }

        public ICriteria In(string propertyName, ICollection propertyValues)
        {
            _criterion.Add(new Expression(ExpressionType.In, propertyName, propertyValues));
            return this;
        }

        public ICriteria NotBetween(string propertyName, object lo, object hi)
        {
            _criterion.Add(new Expression(ExpressionType.NotBetween, propertyName, new[] { lo, hi }));
            return this;
        }

        public ICriteria NotIn(string propertyName, ICollection propertyValues)
        {
            _criterion.Add(new Expression(ExpressionType.NotIn, propertyName, propertyValues));
            return this;
        }

        public int CriteriaCount
        {
            get { return _criterion.Count; }
        }

        public int AliasCount
        {
            get { return _aliases.Count; }
        }

        public int OrderCount
        {
            get { return _orders.Count; }
        }

        internal bool OrderContains(Order orderToEvaluate)
        {
            return this._orders.Any(ord =>
                ord.OrderType == orderToEvaluate.OrderType &&
                ord.PropertyName == orderToEvaluate.PropertyName);
        }

        public ICriteria Asc(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return this;

            if (!OrderContains(new Order(OrderType.Asc, propertyName)))
                _orders.Add(new Order(OrderType.Asc, propertyName));
            return this;
        }

        public ICriteria Asc(IEnumerable<string> propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                if (!OrderContains(new Order(OrderType.Asc, propertyName)))
                    _orders.Add(new Order(OrderType.Asc, propertyName));
            }
            return this;
        }

        public ICriteria Desc(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return this;

            if (!OrderContains(new Order(OrderType.Desc, propertyName)))
                _orders.Add(new Order(OrderType.Desc, propertyName));
            return this;
        }

        public ICriteria Desc(IEnumerable<string> propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                if (!OrderContains(new Order(OrderType.Desc, propertyName)))
                    _orders.Add(new Order(OrderType.Desc, propertyName));
            }
            return this;
        }

        public string[] GetOrderList()
        {
            return this._orders.Select(order => order.PropertyName).ToArray();
        }

        public Criteria Alias(string aliasName, string typeName)
        {
            if (_aliases.ContainsKey(aliasName)) throw new ArgumentException("Alias already in use");
            _aliases[aliasName] = typeName;
            return this;
        }

        public override string ToString()
        {
            var text = new StringBuilder("");
            text.AppendFormat("Type: {0} [", _persistentType.FullName);
            text.Append("]");
            return text.ToString();
        }

        public Type PersistentType
        {
            get { return _persistentType; }
        }

        public bool Contains(Expression expressionToEvaluate)
        {
            return this._criterion.Any(exp =>
                exp.ExpressionType == expressionToEvaluate.ExpressionType &&
                exp.PropertyName == expressionToEvaluate.PropertyName &&
                exp.PropertyValue.ToString() == expressionToEvaluate.PropertyValue.ToString());
        }

        public ICriteria RowCount()
        {
            this.Projections.Add(NHibernate.Criterion.Projections.RowCount());
            return this;
        }

        public ICriteria SetFirstResult(int firstResult)
        {
            this.FirstResult = firstResult;
            return this;
        }

        public ICriteria SetMaxResult(int maxResult)
        {
            this.MaxResult = maxResult;
            return this;
        }
    }

    [Serializable]
    public class SubCriteria : Criteria
    {
        public string EntityProperty = string.Empty;
        public SubCriteria(string entityPropertyName)
            : base(null)
        {
            EntityProperty = entityPropertyName;
        }
    }

    [Serializable]
    public enum ExpressionType
    {
        Between,
        Eq,
        Ne,
        Le,
        Ge,
        Like,
        Or,
        EqOr,
        And,
        Lt,
        Gt,
        IsNull,
        In,
        NotBetween,
        NotIn
    }

    [Serializable]
    internal enum OrderType
    {
        Asc,
        Desc
    }
}
