using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressForms
{
    public interface IExpressFormsExchange<T, TId>
    {
        TId Insert(T record);
        IEnumerable<T> Get();
        T Get(TId id);
        void Update(T record);
        void Delete(TId id);
    }
}
