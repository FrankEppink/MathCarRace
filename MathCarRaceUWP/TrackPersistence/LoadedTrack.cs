using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class LoadedTrack : TrackPolygon, ITrack
	{
		#region members

		private readonly string mFilepath;

		internal double widthLeftStartingPoint;
		internal double widthRightStartingPoint;

		internal IList<Point> outerPoints;
		internal IList<Point> innerPoints;

		#endregion members

		#region Constructor

		internal LoadedTrack(string filepath)
		{
			mFilepath = filepath;
		}

		#endregion Constructor


		#region ITrack

		public string GetTrackId()
		{
			return "LoadedTrack" + mFilepath;
		}

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			// Add starting line
			Point startingLineLeft = new Point
			{
				X = width / 100 * widthLeftStartingPoint,
				Y = startingLineCoordinate
			};
			Point startingLineRight = new Point
			{
				X = width / 100 * widthRightStartingPoint,
				Y = startingLineCoordinate
			};
			Shape startingLine = base.CreateStartingLine(startingLineLeft, startingLineRight);
			children.Add(startingLine);
			
			// Draw outer track
			outerPolygon = new Polygon();
			for(int index=0; index < outerPoints.Count; index++)
			{
				Point curPoi = outerPoints[index];
				outerPolygon.Points.Add(new Point(width / 100 * curPoi.X, height / 100 * curPoi.Y));
			}
			base.SetOuterBorderProperties(outerPolygon);
			children.Add(outerPolygon);
			
			// Draw inner track
			innerPolygon = new Polygon();
			for (int index = 0; index < innerPoints.Count; index++)
			{
				Point curPoi = innerPoints[index];
				innerPolygon.Points.Add(new Point(width / 100 * curPoi.X, height / 100 * curPoi.Y));
			}
			base.SetInnerBorderProperties(innerPolygon);
			children.Add(innerPolygon);
		}

		#endregion ITrack
	}
}
