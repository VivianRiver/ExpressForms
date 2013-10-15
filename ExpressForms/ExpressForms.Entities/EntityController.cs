using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using ExpressForms.Inputs;

namespace ExpressForms.Entities
{
    public abstract class EntityController<TEntity> : ExpressForms.ExpressFormsController<TEntity, int>
        where TEntity : EntityObject, new()
    {
        public EntityController()
            : base()
        {
            IgnoredPropertyNames.AddRange(new[] { "EntityState", "EntityKey" });
        }

        protected void Initialize(ObjectContext objectContext)
        {            
            EntityExchange<TEntity, int> exchange = EntityExchangeFactory.GetEntityExchange<TEntity, int>(objectContext);
            
            Initialize(exchange);
        }

        
    }
}