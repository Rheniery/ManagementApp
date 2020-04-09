using System;
using System.Collections.Generic;
using System.Text;

namespace Management.Services.StandardService
{
    public abstract class StandardService<T>
    {
        /// <summary>
        /// Save T Object
        /// </summary>
        /// <param name="obj">T Object</param>
        /// <returns>boolean</returns>
        public abstract bool Save(T obj);

        /// <summary>
        /// Delete T Object
        /// </summary>
        /// <param name="obj">T Object</param>
        /// <returns>boolean</returns>
        public abstract bool Delete(T obj);

        /// <summary>
        /// List All T Objects
        /// </summary>
        /// <returns>List<T></returns>
        public abstract List<T> Consult();

        /// <summary>
        /// Find T object by Id
        /// </summary>
        /// <param name="id">Object Id</param>
        /// <returns>T Object</returns>
        public abstract T Consult(decimal id);

        /// <summary>
        /// Find T Object by Name
        /// </summary>
        /// <param name="name">Object Name</param>
        /// <returns>List<T> with that name</returns>
        public abstract List<T> Consult(string name);
    }
}
