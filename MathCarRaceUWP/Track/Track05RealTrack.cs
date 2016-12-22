using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class Track05RealTrack : TrackPolygon, ITrack
	{
		#region ITrack

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			// Draw outer track
			outerPolygon = new Polygon();
			// left part start
			outerPolygon.Points.Add(new Point(width / 100 * 5, startingLineCoordinate));
			outerPolygon.Points.Add(new Point(width / 100 * 5, height / 100 * 40));
			outerPolygon.Points.Add(new Point(width / 100 * 8, height / 100 * 20));
			outerPolygon.Points.Add(new Point(width / 100 * 12, height / 100 * 10));
			outerPolygon.Points.Add(new Point(width / 100 * 13, height / 100 * 8));

			// upper part
			outerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 35, height / 100 * 10));
			outerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 8));
			outerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 12));
			outerPolygon.Points.Add(new Point(width / 100 * 65, height / 100 * 14));
			outerPolygon.Points.Add(new Point(width / 100 * 67, height / 100 * 16));
			outerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 20));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 22));

			// right part
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 33));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 40));
			outerPolygon.Points.Add(new Point(width / 100 * 85, height / 100 * 60));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 70));
			outerPolygon.Points.Add(new Point(width / 100 * 85, height / 100 * 80));

			// lower part
			outerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 78, height / 100 * 91));
			outerPolygon.Points.Add(new Point(width / 100 * 76, height / 100 * 92));
			outerPolygon.Points.Add(new Point(width / 100 * 74, height / 100 * 93));
			outerPolygon.Points.Add(new Point(width / 100 * 72, height / 100 * 94));
			outerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 95));
			outerPolygon.Points.Add(new Point(width / 100 * 50, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 80));
			outerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 85));

			// left part end
			outerPolygon.Points.Add(new Point(width / 100 * 15, height / 100 * 70));
			outerPolygon.Points.Add(new Point(width / 100 * 10, height / 100 * 60));

			base.SetOuterBorderProperties(outerPolygon);
			children.Add(outerPolygon);

			// Add starting line
			Point startingLineLeft = new Point
			{
				X = width / 100 * 5,
				Y = startingLineCoordinate
			};
			Point startingLineRight = new Point
			{
				X = width / 100 * 15,
				Y = startingLineCoordinate
			};
			Shape startingLine = base.CreateStartingLine(startingLineLeft, startingLineRight);
			children.Add(startingLine);

			// Draw inner track
			innerPolygon = new Polygon();
			// left part start
			innerPolygon.Points.Add(new Point(width / 100 * 15, startingLineCoordinate));
			innerPolygon.Points.Add(new Point(width / 100 * 15, height / 100 * 40));
			innerPolygon.Points.Add(new Point(width / 100 * 18, height / 100 * 20));
						
			// upper part
			innerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 15));
			innerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 15));
			innerPolygon.Points.Add(new Point(width / 100 * 35, height / 100 * 20));
			innerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 18));
			innerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 22));
			innerPolygon.Points.Add(new Point(width / 100 * 65, height / 100 * 24));
			innerPolygon.Points.Add(new Point(width / 100 * 67, height / 100 * 26));
			innerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 30));
			
			// right part
			innerPolygon.Points.Add(new Point(width / 100 * 86, height / 100 * 33));
			innerPolygon.Points.Add(new Point(width / 100 * 85, height / 100 * 40));
			innerPolygon.Points.Add(new Point(width / 100 * 75, height / 100 * 60));
			innerPolygon.Points.Add(new Point(width / 100 * 85, height / 100 * 70));
			innerPolygon.Points.Add(new Point(width / 100 * 75, height / 100 * 80));

			// lower part
			innerPolygon.Points.Add(new Point(width / 100 * 78, height / 100 * 81));
			innerPolygon.Points.Add(new Point(width / 100 * 76, height / 100 * 82));
			innerPolygon.Points.Add(new Point(width / 100 * 74, height / 100 * 83));
			innerPolygon.Points.Add(new Point(width / 100 * 72, height / 100 * 84));
			innerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 85));
			innerPolygon.Points.Add(new Point(width / 100 * 50, height / 100 * 80));
			
			// left part end
			innerPolygon.Points.Add(new Point(width / 100 * 25, height / 100 * 70));
			innerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 60));

			base.SetInnerBorderProperties(innerPolygon);
			children.Add(innerPolygon);
		}

		#endregion ITrack
	}
}
