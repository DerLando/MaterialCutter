using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.DocObjects.Custom;
using Rhino.Geometry;

namespace MaterialCutter.Core.Data
{
    public abstract class DataBase : UserData
    {
        public Plane Plane { get; set; }

        protected DataBase() { }

        protected DataBase(Plane plane)
        {
            Plane = plane;
        }
    }
}
