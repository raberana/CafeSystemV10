using Cafe.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Entities
{
    public class History
    {
        private string _userName;
        private string _transaction;
        private HistoryType _category;
        private DateTime _actionDateTime;

        public History(IUser user, string message, HistoryType type, DateTime actionDateTime)
        {
            _userName = user.Username;
            _transaction = message;
            _category = type;
            _actionDateTime = actionDateTime;
        }

        public virtual string Username
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public virtual HistoryType Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public virtual string TransactionMessage
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        public virtual DateTime ActionDateTime
        {
            get { return _actionDateTime; }
            set { _actionDateTime = value; }
        }

    }
}
