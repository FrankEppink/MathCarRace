using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class Track06RealTrack : TrackPolygon, ITrack
	{
		#region ITrack

		public string GetTrackId()
		{
			return "Track06RealTrack";
		}

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			// the given numbers are percentages, i.e.		width / 100 * 5			means 
			// from the full width take 5 %, so		width / 100 * 5		is really close to the left border

			// Draw outer track
			outerPolygon = new Polygon();
			// left part start
			outerPolygon.Points.Add(new Point(width / 100 * 5, startingLineCoordinate));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 45));
			outerPolygon.Points.Add(new Point(width / 100 * 9, height / 100 * 40));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 35));
			outerPolygon.Points.Add(new Point(width / 100 * 5, height / 100 * 30));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 25));
			outerPolygon.Points.Add(new Point(width / 100 * 9, height / 100 * 20));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 15));
			outerPolygon.Points.Add(new Point(width / 100 * 5, height / 100 * 10));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 9, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 11, height / 100 * 6));

			// upper part
			outerPolygon.Points.Add(new Point(width / 100 * 15, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 17, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 19, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 21, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 23, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 25, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 27, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 29, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 31, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 33, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 35, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 37, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 39, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 41, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 43, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 45, height / 100 * 11));
			outerPolygon.Points.Add(new Point(width / 100 * 47, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 49, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 51, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 53, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 55, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 57, height / 100 * 11));
			outerPolygon.Points.Add(new Point(width / 100 * 59, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 61, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 63, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 65, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 67, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 69, height / 100 * 11));
			outerPolygon.Points.Add(new Point(width / 100 * 71, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 73, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 75, height / 100 * 5));
			outerPolygon.Points.Add(new Point(width / 100 * 77, height / 100 * 7));
			outerPolygon.Points.Add(new Point(width / 100 * 79, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 81, height / 100 * 11));
			outerPolygon.Points.Add(new Point(width / 100 * 83, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 85, height / 100 * 8));
			outerPolygon.Points.Add(new Point(width / 100 * 87, height / 100 * 9));
			outerPolygon.Points.Add(new Point(width / 100 * 89, height / 100 * 11));
			outerPolygon.Points.Add(new Point(width / 100 * 91, height / 100 * 13));

			// right part
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 14));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 16));
			outerPolygon.Points.Add(new Point(width / 100 * 93, height / 100 * 18));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 20));
			outerPolygon.Points.Add(new Point(width / 100 * 91, height / 100 * 22));
			outerPolygon.Points.Add(new Point(width / 100 * 93, height / 100 * 24));
			outerPolygon.Points.Add(new Point(width / 100 * 94, height / 100 * 26));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 28));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 30));
			outerPolygon.Points.Add(new Point(width / 100 * 94, height / 100 * 32));
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 34));
			outerPolygon.Points.Add(new Point(width / 100 * 98, height / 100 * 36));
			outerPolygon.Points.Add(new Point(width / 100 * 97, height / 100 * 38));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 40));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 42));
			outerPolygon.Points.Add(new Point(width / 100 * 88, height / 100 * 44));
			outerPolygon.Points.Add(new Point(width / 100 * 89, height / 100 * 46));
			outerPolygon.Points.Add(new Point(width / 100 * 89, height / 100 * 48));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 50));
			outerPolygon.Points.Add(new Point(width / 100 * 89, height / 100 * 52));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 54));
			outerPolygon.Points.Add(new Point(width / 100 * 91, height / 100 * 56));
			outerPolygon.Points.Add(new Point(width / 100 * 93, height / 100 * 58));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 60));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 62));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 64));
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 66));
			outerPolygon.Points.Add(new Point(width / 100 * 97, height / 100 * 68));
			outerPolygon.Points.Add(new Point(width / 100 * 93, height / 100 * 70));
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 72));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 74));
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 76));
			outerPolygon.Points.Add(new Point(width / 100 * 91, height / 100 * 78));
			outerPolygon.Points.Add(new Point(width / 100 * 94, height / 100 * 80));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 82));
			outerPolygon.Points.Add(new Point(width / 100 * 96, height / 100 * 84));
			outerPolygon.Points.Add(new Point(width / 100 * 95, height / 100 * 86));
			outerPolygon.Points.Add(new Point(width / 100 * 94, height / 100 * 88));
			outerPolygon.Points.Add(new Point(width / 100 * 93, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 92, height / 100 * 92));
			outerPolygon.Points.Add(new Point(width / 100 * 91, height / 100 * 94));
			outerPolygon.Points.Add(new Point(width / 100 * 90, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 89, height / 100 * 98));

			// lower part
			outerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 78, height / 100 * 95));
			outerPolygon.Points.Add(new Point(width / 100 * 76, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 74, height / 100 * 97));
			outerPolygon.Points.Add(new Point(width / 100 * 72, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 99));
			outerPolygon.Points.Add(new Point(width / 100 * 68, height / 100 * 95));
			outerPolygon.Points.Add(new Point(width / 100 * 66, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 64, height / 100 * 97));
			outerPolygon.Points.Add(new Point(width / 100 * 62, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 99));

			outerPolygon.Points.Add(new Point(width / 100 * 58, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 56, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 54, height / 100 * 93));
			outerPolygon.Points.Add(new Point(width / 100 * 52, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 50, height / 100 * 98));

			outerPolygon.Points.Add(new Point(width / 100 * 48, height / 100 * 99));
			outerPolygon.Points.Add(new Point(width / 100 * 46, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 44, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 42, height / 100 * 93));
			outerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 96));

			outerPolygon.Points.Add(new Point(width / 100 * 38, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 36, height / 100 * 99));
			outerPolygon.Points.Add(new Point(width / 100 * 34, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 32, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 93));

			outerPolygon.Points.Add(new Point(width / 100 * 28, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 26, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 24, height / 100 * 99));
			outerPolygon.Points.Add(new Point(width / 100 * 22, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 20, height / 100 * 96));

			outerPolygon.Points.Add(new Point(width / 100 * 18, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 16, height / 100 * 93));
			outerPolygon.Points.Add(new Point(width / 100 * 14, height / 100 * 96));
			outerPolygon.Points.Add(new Point(width / 100 * 12, height / 100 * 98));
			outerPolygon.Points.Add(new Point(width / 100 * 10, height / 100 * 99));

			// left part end
			outerPolygon.Points.Add(new Point(width / 100 * 9, height / 100 * 90));
			outerPolygon.Points.Add(new Point(width / 100 * 8, height / 100 * 85));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 80));
			outerPolygon.Points.Add(new Point(width / 100 * 6, height / 100 * 75));
			outerPolygon.Points.Add(new Point(width / 100 * 7, height / 100 * 70));
			outerPolygon.Points.Add(new Point(width / 100 * 6, height / 100 * 65));
			outerPolygon.Points.Add(new Point(width / 100 * 5, height / 100 * 60));
			outerPolygon.Points.Add(new Point(width / 100 * 8, height / 100 * 55));

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
			innerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 18));
			innerPolygon.Points.Add(new Point(width / 100 * 35, height / 100 * 25));
			innerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 18));
			innerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 25));
			innerPolygon.Points.Add(new Point(width / 100 * 65, height / 100 * 22));
			innerPolygon.Points.Add(new Point(width / 100 * 67, height / 100 * 20));
			innerPolygon.Points.Add(new Point(width / 100 * 80, height / 100 * 25));
			
			// right part
			innerPolygon.Points.Add(new Point(width / 100 * 76, height / 100 * 33));
			innerPolygon.Points.Add(new Point(width / 100 * 65, height / 100 * 40));
			innerPolygon.Points.Add(new Point(width / 100 * 66, height / 100 * 60));
			innerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 70));
			innerPolygon.Points.Add(new Point(width / 100 * 68, height / 100 * 80));

			// lower part
			innerPolygon.Points.Add(new Point(width / 100 * 78, height / 100 * 81));
			innerPolygon.Points.Add(new Point(width / 100 * 76, height / 100 * 82));
			innerPolygon.Points.Add(new Point(width / 100 * 74, height / 100 * 83));
			innerPolygon.Points.Add(new Point(width / 100 * 72, height / 100 * 84));
			innerPolygon.Points.Add(new Point(width / 100 * 70, height / 100 * 85));
			innerPolygon.Points.Add(new Point(width / 100 * 60, height / 100 * 83));
			innerPolygon.Points.Add(new Point(width / 100 * 50, height / 100 * 83));
			innerPolygon.Points.Add(new Point(width / 100 * 40, height / 100 * 81));
			innerPolygon.Points.Add(new Point(width / 100 * 30, height / 100 * 81));

			// left part end
			innerPolygon.Points.Add(new Point(width / 100 * 25, height / 100 * 70));
			innerPolygon.Points.Add(new Point(width / 100 * 23, height / 100 * 65));
			innerPolygon.Points.Add(new Point(width / 100 * 22, height / 100 * 60));
			innerPolygon.Points.Add(new Point(width / 100 * 21, height / 100 * 55));

			base.SetInnerBorderProperties(innerPolygon);
			children.Add(innerPolygon);
		}

		#endregion ITrack
	}
}


