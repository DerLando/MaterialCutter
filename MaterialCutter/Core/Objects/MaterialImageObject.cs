using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialCutter.Core.Data;
using MaterialCutter.Core.Extensions;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;

namespace MaterialCutter.Core.Objects
{
    public class MaterialImageObject : CurveObjectBase<MaterialImageData>
    {
        public MaterialImageObject() { }

        public MaterialImageObject(MaterialImageData data, Curve curve) : base(data, curve) { }

        protected override void OnDeleteFromDocument(RhinoDoc doc)
        {
            RhinoApp.WriteLine($"{Attributes.ObjectId} OnDelete called!");
            // Add data to temporary dict on PlugIn class
            if (MaterialCutterPlugIn.Instance.ImageDataTemps.ContainsKey(Id))
            {
                MaterialCutterPlugIn.Instance.ImageDataTemps[Id] = Data;
            }
            else
            {
                MaterialCutterPlugIn.Instance.ImageDataTemps.Add(Id, Data);
            }

            base.OnDeleteFromDocument(doc);
        }

        protected override void OnAddToDocument(RhinoDoc doc)
        {
            // TODO: Load data properties back from dictionary
            if (MaterialCutterPlugIn.Instance.ImageDataTemps.ContainsKey(Id))
            {
                var original = MaterialCutterPlugIn.Instance.ImageDataTemps[Id];
                var current = Data;
                // TODO: Write a function to get width and height from curve object (hint: use this.CurveGeometry)
                current.CopyPropertiesFrom(original);
                current.CalculateWidthHeightFromCurve(CurveGeometry as PolylineCurve);
            }
            RhinoApp.WriteLine($"{Attributes.ObjectId} OnAdd called!");
            base.OnAddToDocument(doc);
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var data = Data;

            if (data is null) return;

            // hacky way of checking top view
            if (e.Viewport.IsParallelProjection && e.Viewport.Name == "Top")
            {
                // get rectangle defining points in screen space
                var screenOrigin = e.Viewport.WorldToClient(data.Plane.Origin);
                var topRight = e.Viewport.WorldToClient(data.Boundary.Corner(2));
                var topLeft = e.Viewport.WorldToClient(data.Boundary.Corner(3));

                // scale bitmap
                var width = Math.Abs(topRight.X - screenOrigin.X);
                var height = Math.Abs(topRight.Y - screenOrigin.Y);
                if (width < 0.01 | height < 0.01) return;
                var displayBitmap =
                    new DisplayBitmap(ImageExtensions.ResizeImage(data.Image, Convert.ToInt32(width),
                        Convert.ToInt32(height)));

                // display scaled bitmap
                e.Display.DrawBitmap(displayBitmap, Convert.ToInt32(topLeft.X), Convert.ToInt32(topLeft.Y));
            }
            base.OnDraw(e);
        }
    }
}
