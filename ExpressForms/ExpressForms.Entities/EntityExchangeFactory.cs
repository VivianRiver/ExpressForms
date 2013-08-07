using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace ExpressForms.Entities
{
    public static class EntityExchangeFactory
    {
        //ObjectContext db;

        // No public parameterless constructor
        //private EntityExchangeFactory() { }

        //public EntityExchangeFactory(ObjectContext db)
        //{
        //    this.db = db;
        //}

        public static EntityExchange<TEntity, TId> GetEntityExchange<TEntity, TId>(ObjectContext db)
            where TEntity : EntityObject, new()
        {
            EntityExchange<TEntity, TId> exchange = new EntityExchange<TEntity, TId>(db);
            return exchange;
        }

    }
}
