using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Web.Mvc;
using ExpressForms.Inputs;

namespace ExpressForms.Entities
{
    /// <summary>
    /// This class will use reflection to make a quick and dirty form for a given class composed of basic types.
    /// It will not recognize enumerations or complex types.
    /// </summary>
    public class EntityForm : ExpressForms.ExpressFormsForm
    {
        private string[] propertiesToIgnore = new[] { "EntityState", "EntityKey" };

        // private Dictionary<string, IEnumerable<SelectListItem>> selectListItemDictionary = new Dictionary<string, IEnumerable<SelectListItem>>();

        private Func<Type, IEnumerable<object>> ChoiceGetter { get; set; }

        public void Initialize<T>(T t, Func<Type, IEnumerable<object>> choiceGetter )
        {
            if (!t.GetType().IsSubclassOf(typeof(EntityObject)))
                throw new ArgumentOutOfRangeException("EntityForm can only be initialized with an object that inherits EntityObject.");

            ChoiceGetter = choiceGetter;

            PropertyInfo primaryKeyProperty = new EntityHelper().GetPrimaryKeyProperty(t.GetType());

            // t.GetType() is used here rather than typeof(T) in order to get the most specific type implemented by the object passed in.
            PropertyInfo[] properties = t.GetType().GetProperties();

            Func<PropertyInfo, ExpressFormsInput> InputSelector = p =>
            {
                switch (p.PropertyType.Name)
                {
                    case "Boolean":
                        return new ExpressFormsCheckBox()
                        {
                            FormName = t.GetType().Name,
                            InputName = p.Name,
                            Value = Convert.ToString(p.GetValue(t, null)),
                            IsVisible = true
                        };
                    case "EntityCollection`1":
                        return new ExpressFormsListBox()
                        {
                            FormName = t.GetType().Name,
                            InputName = p.Name,
                            Value = Convert.ToString(p.GetValue(t, null)),
                            IsVisible = true,
                            SelectListItems = getSelectListItems(p),
                        };
                    default:
                        return new ExpressFormsTextBox()
                        {
                            FormName = t.GetType().Name,
                            InputName = p.Name,
                            Value = Convert.ToString(p.GetValue(t, null)),
                            IsVisible = (p.Name != primaryKeyProperty.Name),
                        };
                }
            };

            Inputs = properties
                .Where(p=> !propertiesToIgnore.Contains(p.Name))
                .ToDictionary(p=>p.Name, InputSelector);                

            isInitialized = true;
        }        

        // For now, this will just get the entire contents of a foreign table referenced by the object.
        private IEnumerable<SelectListItem> getSelectListItems(PropertyInfo property)
        {                
            if (property.PropertyType.Name == "EntityCollection`1")
            {
                Type entityType = property.PropertyType.GetGenericArguments().Single();

                PropertyInfo stringProperty = entityType.GetProperties()
                    .Where(p => p.PropertyType.Name == "String")
                    .First();
                Func<object, string> genericTextGetter = o => Convert.ToString(stringProperty.GetValue(o, null));

                PropertyInfo primaryKeyProperty = new EntityHelper().GetPrimaryKeyProperty(entityType);
                Func<object, string> primaryKeyGetter = o => Convert.ToString(primaryKeyProperty.GetValue(o, null));

                Type parameterType = property.PropertyType.GetGenericArguments().Single();
                string parameterTypeName = parameterType.Name;

                var choices = ChoiceGetter(entityType);

                return choices.Select(c => new SelectListItem()
                {
                    Text = genericTextGetter(c),
                    Value = primaryKeyGetter(c)
                });

                //if (selectListItemDictionary.Keys.Contains(parameterTypeName))
                //{
                //    return selectListItemDictionary[parameterTypeName];
                //}
                //else
                //    return new SelectListItem[] { };
            }
            else
                return null;
        }

        // TODO: Properly implement this
        private bool getSelectListMultiple(PropertyInfo property)
        {
            return property.PropertyType.Name == "EntityCollection`1";            
        }
    }
}
