using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;

namespace ExpressForms.Entities
{
    /*
     * TODO: The state of this class is entirely unacceptable.  It's a mess and it isn't clear what it does.
     * 
     * This class seemed to be looking good until I tried to implement functionality for views that update two tables simultaneously.
     * Then, the insert and update commands became problematic.
     * 
     * The delete commands are problematic too, but they can probably be fixed by using cascading deletes.
     * 
     * I believe that I can count on JSON serialization hitting circular reference problems.  I think that I will want to somehow limit
     * JSON serialization to the table being queried and tables that are "one association" away (of course not counting join tables).
     * 
     * *************************************************************
     * 
     * For the insert and update operations, I was originally having the calling code pass in an entity stub representing each 
     * row in another table the entity is related to.  This is not working.  I think that I will need to somehow distinguish between
     * new rows to be associated and associations with existing rows.  Also, I think that I will need to pass in seperate objects
     * representing the rows in foreign tables (perhaps a dictionary).
     * 
     * I think that it also may be problematic that the EntityExchange is associated with a single entity type.  Perhaps it needs to be 
     * made generic, or there needs to be some IoC to supply Exchanges for multiple entities at once, and remember that I don't want
     * this to be tightly coupled to Entity Framework.
     * 
     * */

    public class EntityExchange<TEntity, TId> : ExpressForms.IExpressFormsExchange<TEntity, TId>, IDisposable        
        where TEntity : EntityObject, new()
    {        
        public EntityExchange(ObjectContext objectContext)
        {
            db = objectContext;
        }
        ObjectContext db;               

        public void Dispose()
        {
            db.Dispose();
        }

        // Gets an ObjectSet from Entity Framework representing the table this Exchange is typed for.
        //private ObjectSet<T> GetObjectSet()
        //{
        //    Type entitiesType = db.GetType();
        //    PropertyInfo[] properties = entitiesType.GetProperties();
        //    PropertyInfo objectSetProperty = properties.Single(p => p.PropertyType == typeof(ObjectSet<T>));
        //    ObjectSet<T> objectSet = (ObjectSet<T>)(objectSetProperty.GetValue(db, null));
        //    return objectSet;         
        //}

        // Gets an ObjectSet from Entity Framework representing an arbitrary table.
        // Note that with the generic return type, the calling code must cast the result appropriately.
        //private ObjectSet<EntityObject> GetObjectSetForOtherType(Type type)
        //{
        //    Type genericObjectSetType = typeof(ObjectSet<>).GetGenericTypeDefinition();
        //    Type typedObjectSetType = genericObjectSetType.MakeGenericType(new Type[] { type });                            
        //    PropertyInfo objectSetProperty = db.GetType().GetProperties().Single(p => p.PropertyType == typedObjectSetType);
        //    ObjectSet<EntityObject> objectSet = (ObjectSet<EntityObject>)(objectSetProperty.GetValue(db, null));
        //    return objectSet;
        //}
        
        public ObjectSet<T> GetObjectSetForEntityType<T>()
            where T : EntityObject, new()
        {
            Type genericObjectSetType = typeof(ObjectSet<>).GetGenericTypeDefinition();
            Type typedObjectSetType = genericObjectSetType.MakeGenericType(typeof(T));
            PropertyInfo objectSetProperty = db.GetType().GetProperties().Single(p => p.PropertyType == typedObjectSetType);
            ObjectSet<T> objectSet = (ObjectSet<T>)(objectSetProperty.GetValue(db, null));
            return objectSet;
        }

        //public ObjectSet<EntityObject> GetObjectSetForEntityType(Type type)
        public object GetObjectSetForEntityType(Type type)
        {
            Type genericObjectSetType = typeof(ObjectSet<>).GetGenericTypeDefinition();
            Type typedObjectSetType = genericObjectSetType.MakeGenericType(type);
            PropertyInfo objectSetProperty = db.GetType().GetProperties().Single(p => p.PropertyType == typedObjectSetType);            
            //ObjectSet<EntityObject> objectSet = (ObjectSet<EntityObject>)(objectSetProperty.GetValue(db, null));
            object objectSet = objectSetProperty.GetValue(db, null);
            return objectSet;
        }

        private TId GetId(TEntity entity)            
        {
            // This has partially been setup for generic primary keys, but a few steps remain.
                                       
            EntityHelper helper = new EntityHelper();
            PropertyInfo property = helper.GetPrimaryKeyProperty(typeof(TEntity));
            TId value = (TId)( property.GetValue(entity, null));

            return value;
        }

        public TId Insert(TEntity record)
        {            
            Type type = record.GetType();
            // Get the properties of the object passed in that are EntityCollection`1
            // (These properties represent data in other tables that must be updated.)
            PropertyInfo[] entityCollectionProperties = type.GetProperties()
                .Where(p => p.PropertyType.Name == "EntityCollection`1")
                .ToArray();

            //foreach (PropertyInfo property in entityCollectionProperties)
            //{
            //    // Check what entity IDs were passed in and look up the entities.
            //    dynamic collectionPassedIn = property.GetValue(t, null);
            //    List<int> idsPassedIn = new List<int>();
            //    List<EntityObject> entitiesToAdd = new List<EntityObject>();
            //    foreach (dynamic entity in collectionPassedIn)
            //    {
            //        int id = entity.Id;
            //        idsPassedIn.Add(id);
            //    }

            //    // entityCollection will contain rows from another table.
            //    //dynamic entityCollection = property.GetValue(t, null);
            //    dynamic entityCollection = property.GetValue(t, null);
            //    // Look up the rows from this table that the user passed in.
            //    // First, find the ObjectSet that represents this table.
            //    dynamic objectSet = this.GetType().GetMethod("GetObjectSetForOtherType", BindingFlags.NonPublic | BindingFlags.Instance)
            //        .MakeGenericMethod(property.PropertyType.GetGenericArguments().Single())
            //        .Invoke(this, null);

            //    entityCollection.Clear();
            //    foreach (int Id in idsPassedIn)
            //    {
            //        // Should be only one, but it won't work that way.
            //        IEnumerable<EntityObject> entities = objectSet
            //            .Where("it.Id = @Id", new ObjectParameter[] { new ObjectParameter("Id", Id) });
            //        foreach (dynamic entity in entities)
            //        {
            //            entityCollection.Add(entity);
            //        }
            //    }
            //}
            ObjectSet<TEntity> objectSet = GetObjectSetForEntityType<TEntity>();            
            objectSet.AddObject(record);
            db.SaveChanges();

            return GetId(record);
        }

        public IEnumerable<TEntity> Get()
        {            
            // The great thing about this is that the calling code can use LinQ expressions and Entity Framework will interpret them
            // and pass the appropriate query to SQL.

            ObjectSet<TEntity> objectSet = GetObjectSetForEntityType<TEntity>();            
            return objectSet.AsEnumerable();
        }

        public TEntity Get(TId Id)
        {
            // This has partially been setup for generic primary keys, but a few steps remain.

            string whereClauseFormat = ("it.{0} = @Id");
            string idPropertyName = new EntityHelper().GetPrimaryKeyProperty(typeof(TEntity)).Name;
            string whereClause = string.Format(whereClauseFormat, idPropertyName);

            ObjectSet<TEntity> objectSet = GetObjectSetForEntityType<TEntity>();
            TEntity record = objectSet
                .Where(whereClause, new ObjectParameter[] { new ObjectParameter(idPropertyName, Id) })
                .Single<TEntity>();

            return record;            
        }

        // THIS FUNCTION DOESN'T WORK YET.
        // SORRY, FOLKS!
        // TODO: FIX THIS!
        public void Update(TEntity entity)
        {
            // This first part should update the primitive fields.
            ObjectSet<TEntity> objectSet = GetObjectSetForEntityType<TEntity>();
            objectSet.Attach(entity);
            db.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Modified);            

            ////T entityFromServer = Get(GetId(t));            

            //// This next part enumerates all the "EntityCollection`1" properties (which represent references to other tables)
            //Type type = typeof(T);
            //// Get the properties of the object passed in that are EntityCollection`1
            //// (These properties represent data in other tables that must be updated.)
            //PropertyInfo[] entityCollectionProperties = type.GetProperties()
            //    .Where(p => p.PropertyType.Name == "EntityCollection`1")
            //    .ToArray();            

            //foreach (PropertyInfo property in entityCollectionProperties)
            //{
            //    // Check what entity IDs were passed in and look up the entities.
            //    dynamic collectionPassedIn = property.GetValue(t, null);
            //    List<int> idsPassedIn = new List<int>();
            //    List<EntityObject> entitiesToAdd = new List<EntityObject>();
            //    foreach(dynamic entity in collectionPassedIn){
            //        int id = entity.Id;
            //        idsPassedIn.Add(id);
            //    }                                                

            //    // entityCollection will contain rows from another table.
            //    //dynamic entityCollection = property.GetValue(t, null);
            //    dynamic entityCollection = property.GetValue(t, null);
            //    // Clear the existing rows in preparation for adding what the user passed in.
            //    //entityCollection.Clear();                                                
                
            //    // Look up the rows from this table that the user passed in.
            //    // First, find the ObjectSet that represents this table.
            //    dynamic objectSet = this.GetType().GetMethod("GetObjectSetForOtherType", BindingFlags.NonPublic | BindingFlags.Instance)
            //        .MakeGenericMethod(property.PropertyType.GetGenericArguments().Single())
            //        .Invoke(this, null);
            //    foreach (int Id in idsPassedIn)
            //    {
            //        // Should be only one, but it won't work that way.
            //        IEnumerable<EntityObject> entities = objectSet
            //            .Where("it.Id = @Id", new ObjectParameter[] { new ObjectParameter("Id", Id) });
            //        foreach (dynamic entity in entities)
            //        {
            //            entityCollection.Add(entity);
            //        }
            //    }                                    
            //}

            db.SaveChanges();
        }

        /*
         * private void UpdateClassRoom(ClassRoomDto classRoomDto)
{
    using (var ctx = new TrainingContext())
    {
        ClassRoom classRoomEntity = ctx.
                                    ClassRooms.
                                    Include("Students").
                                    Single(c => c.ClassID == classRoomDto.ClassId);
        classRoomEntity.ClassName = classRoomDto.ClassName;

        foreach (var studentDto in classRoomDto.Students)
        {
            if (studentDto.StudentId == 0)
            {
                // it's a new student add it to the classroom
                Student student = new Student
                                      {
                                          StudentID = studentDto.StudentId,
                                          StudentName = studentDto.StudentName
                                      };
                classRoomEntity.Students.Add(student);
            }
            else
            {
                // Student exists in the DB, but you don't know whether it's 
                // already part of the student collection for the classroom
                Student student = classRoomEntity.
                                  Students.
                                  FirstOrDefault(s => s.StudentID == studentDto.StudentId);

                if (student == null)
                {
                    // this student is not in the class, fetch it from the DB
                    // and add to the classroom
                    student = ctx.
                              Students.
                              SingleOrDefault(s => s.StudentID == studentDto.StudentId)

                    classRoomEntity.Students.Add(student);
                }

                // Update name
                student.StudentName = studentDto.StudentName;
                // Since student is now part of the classroom student collection
                // and classroom IS attached => student is also attached
            }
        }

        ctx.SaveChanges();
    }
}*/

        public void Delete(TId Id)
        {
            TEntity record = Get(Id);
            db.DeleteObject(record);
            db.SaveChanges();
        }

        #region helper methods
        


        #endregion
    }
}
