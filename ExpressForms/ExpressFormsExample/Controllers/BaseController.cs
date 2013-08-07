using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressForms;
using ExpressForms.Entities;
using System.Data.Objects.DataClasses;

namespace ExpressFormsExample.Controllers
{
    /// <summary>
    /// This abstract class inherits the EntityController class and provides the code to initialize the Controller
    /// with the appropriate Entity Framework reference.    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseController<T> : ExpressForms.Entities.EntityController<T>
        where T : EntityObject, new()  
    {       
        protected ExpressFormsExampleEntities db;
        
        public BaseController()
        {
            db = new ExpressFormsExampleEntities();
            Initialize(db);
        }

        ~BaseController()
        {
            db.Dispose();
        }
    }
}
