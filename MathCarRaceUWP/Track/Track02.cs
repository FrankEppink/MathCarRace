using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Track02 is a 8 corner polygon
	/// </summary>
	internal class Track02 : TrackPolygon, ITrack
	{
		#region ITrack

		public string GetTrackId()
		{
			return "Track02";
		}

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			// Draw outer track
			outerPolygon = new Polygon();
			outerPolygon.Points.Add(new Point(width / 100 * 10, height / 100 * 30));
			outerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 10));
			outerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 10));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 30));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 70));
			outerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 10, height / 100 * 70));
			base.SetOuterBorderProperties(outerPolygon);
			children.Add(outerPolygon);

			// Add starting line
			Point startingLineLeft = new Point
			{
				X = width / 100 * 10,
				Y = startingLineCoordinate
			};
			Point startingLineRight = new Point
			{
				X = width / 100 * 20,
				Y = startingLineCoordinate
			};
			Shape startingLine = base.CreateStartingLine(startingLineLeft, startingLineRight);
			children.Add(startingLine);			

			// Draw inner track
			innerPolygon = new Polygon();			
			innerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 40));
			innerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 20));
			innerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 20));
			innerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 40));
			innerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 60));
			innerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 80));
			innerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 80));
			innerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 60));
			base.SetInnerBorderProperties(innerPolygon);
			children.Add(innerPolygon);
		}

		#endregion ITrack
	}
}
