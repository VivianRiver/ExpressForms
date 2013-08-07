using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Reflection;

namespace ExpressForms.Entities
{
    /// <summary>
    /// This class reads the attributes in the generated entity classes as described in MSDN: http://msdn.microsoft.com/en-us/library/71s1zwct.aspx
    /// </summary>
    public class EntityHelper
    {
        /// <summary>
        ///  Gets information about the property of an EntityObject that defines its primary key.
        ///  Note that this method does not support composite primary keys. (will throw exception)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public PropertyInfo GetPrimaryKeyProperty(Type type)
        {
            if (!type.IsSubclassOf(typeof(EntityObject)))
                throw new ArgumentOutOfRangeException("It appears that an object was passed to GetPrimaryKeyProperty that does not inherit from EntityObject.");

            PropertyInfo[] properties = type.GetProperties();
         
            bool primaryKeyFound = false;
            PropertyInfo primaryKeyProperty = null;

            foreach (PropertyInfo property in properties)
            {
                // The EdmScalarPropertyAttribute declared in the auto-generated code tells what properties are part of the primary key.
                // Again, this code does not support composite keys.
                EdmScalarPropertyAttribute attribute =
                    (EdmScalarPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(EdmScalarPropertyAttribute));

                if (attribute != null && attribute.EntityKeyProperty)
                {
                    if (primaryKeyFound)
                        throw new ArgumentOutOfRangeException("It appears that an entity with a composite primary key was passed to GetPrimaryKeyProperty, which is not supported.");
                    primaryKeyProperty = property;
                    primaryKeyFound = true;
                }
            }

            return primaryKeyProperty;
        }

        public ForeignKeyInfo[] GetForeignKeys(EntityObject entity)
        {
            throw new NotImplementedException();
        }

        public class ForeignKeyInfo { }        
    }
}
