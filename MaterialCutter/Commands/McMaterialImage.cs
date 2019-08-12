using System;
using System.Drawing;
using MaterialCutter.Core.Data;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;

namespace MaterialCutter.Commands
{
    public class McMaterialImage : Command
    {
        static McMaterialImage _instance;
        public McMaterialImage()
        {
            _instance = this;
        }

        ///<summary>The only instance of the McMaterialImage command.</summary>
        public static McMaterialImage Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "McMaterialImage"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            // get image file
            Image image;
            var fileDialog = new Rhino.UI.OpenFileDialog
                {Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"};

            if (!fileDialog.ShowOpenDialog()) return Result.Cancel;

            image = Image.FromFile(fileDialog.FileName);

            // get Dimensions
            var rc = RhinoGet.GetRectangle("Specify bounding rectangle", out var corners);
            double width = corners[1].DistanceTo(corners[0]);
            double height = corners[3].DistanceTo(corners[0]);

            // create material image data
            var plane = Plane.WorldXY;
            plane.Origin = corners[0];
            var data = new MaterialImageData(plane, image, width, height);

            // create custom object
            var mObject = data.GetCustomObject();

            // add custom object to doc
            doc.Objects.AddRhinoObject(mObject, mObject.CurveGeometry);

            // redraw the view
            doc.Views.Redraw();

            // return success
            return Result.Success;
        }
    }
}