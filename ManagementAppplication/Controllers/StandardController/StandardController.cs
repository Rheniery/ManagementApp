using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAppplication.Controllers.StandardController
{
    /// <summary>
    /// Generic Controller with some importants methods
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    public abstract class StandardController<T> : Controller
    {
        public abstract bool Save(T obj);

        public abstract bool Delete(T obj);

        public abstract List<T> Consult();

        public abstract T Consult(decimal id);

        public abstract List<T> Consult(string name);
    }
}