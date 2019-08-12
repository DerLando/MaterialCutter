using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialCutter.Core.Extensions;
using MaterialCutter.Core.Objects;
using Rhino.Display;
using Rhino.DocObjects.Custom;
using Rhino.Geometry;

namespace MaterialCutter.Core.Data
{
    public class MaterialImageData : DataBase
    {
        public Image Image { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        //public DisplayBitmap DisplayBitmap { get; set; }

        public Rectangle3d Boundary => new Rectangle3d(Plane, Width, Height);
        public int WidthInt => Convert.ToInt32(Math.Round(Width));
        public int HeightInt => Convert.ToInt32(Math.Round(Height));


        public MaterialImageData() { }

        public MaterialImageData(Plane plane, Image image, double width, double height) : base(plane)
        {
            Image = image;
            Width = width;
            Height = height;

            //CreateDisplayBitmap();
        }

        //private void CreateDisplayBitmap()
        //{
        //    DisplayBitmap = new DisplayBitmap(ImageExtensions.ResizeImage(Image, WidthInt, HeightInt));
        //}

        public MaterialImageObject GetCustomObject()
        {
            return new MaterialImageObject(this, Boundary.ToPolyline().ToPolylineCurve());
        }

        protected override void OnDuplicate(UserData source)
        {
            base.OnDuplicate(source);
        }

        public void CopyPropertiesFrom(MaterialImageData other)
        {
            Plane = other.Plane;

            Image = other.Image;
            Width = other.Width;
            Height = other.Height;
        }

        public void CalculateWidthHeightFromCurve(PolylineCurve curve)
        {
            var rect = Rectangle3d.CreateFromPolyline(curve.ToPolyline());
            Width = rect.Width;
            Height = rect.Height;

            var plane = this.Plane;
            plane.Origin = rect.Corner(0);

            Plane = plane;
        }
    }
}
