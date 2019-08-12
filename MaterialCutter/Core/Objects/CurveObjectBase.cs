using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.DocObjects.Custom;
using Rhino.Geometry;

namespace MaterialCutter.Core.Objects
{
    public abstract class CurveObjectBase<T> : CustomCurveObject where T : UserData
    {
        public T Data => Attributes.UserData.Find(typeof(T)) as T;

        protected CurveObjectBase() { }

        protected CurveObjectBase(T data, Curve curve) : base(curve)
        {
            Attributes.UserData.Add(data);
        }
    }
}
