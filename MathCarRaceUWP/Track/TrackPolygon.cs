using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class TrackPolygon : TrackBase
	{
		#region member

		protected Polygon outerPolygon;
		protected Polygon innerPolygon;

		#endregion member

		#region ITrack

		public bool ValidateVector(IList<Point> routeGridPoints, Point gridPointClicked)
		{
			bool? baseValidationResult;
			BaseValidateVector(routeGridPoints, gridPointClicked, out baseValidationResult);
			if (baseValidationResult.HasValue)
			{
				// we are done
				return baseValidationResult.Value;
			}

			// further checks
			Point point = GridBackgroundHelper.ConvertGridPoint2Point(gridPointClicked);
			bool innerPolygonIncluded = Contains(innerPolygon, point);
			if (innerPolygonIncluded)
			{
				// point is in the inner polygon so it is NOT on the track
				return false;
			}
			// if it is NOT in the inner polygon then simply check if it is included in the outer polygon
			return Contains(outerPolygon, point);
		}

		#endregion ITrack

		#region private helper methods

		/// <summary>
		/// ray casting algorithm
		/// https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm
		/// </summary>
		private static bool Contains(Polygon polygon, Point location)
		{
			// nr of intersections
			uint nrIntersects = 0;
			Point horizontalLineLeft = new Point(0, location.Y);
			Point horizontalLineRight = new Point(location.X, location.Y);

			Point? oldPoint = null;
			for (int index = 0; index < polygon.Points.Count; index++)
			{
				if (oldPoint.HasValue)
				{
					// construct Line and do intersection check
					if (VectorMath.CheckIf2LinesIntersect(horizontalLineLeft, horizontalLineRight, oldPoint.Value, polygon.Points[index]))
					{
						nrIntersects++;
					}
				}

				// save for next line
				oldPoint = polygon.Points[index];
			}

			// first check if the first point of the polygon lies on the starting line
			// which is the case for random generated polygon tracks
			// we must not count this point twice...
			if (polygon.Points[0].Y != location.Y)
			{
				// now check the last closing polygon line
				if (VectorMath.CheckIf2LinesIntersect(horizontalLineLeft, horizontalLineRight, oldPoint.Value, polygon.Points[0]))
				{
					nrIntersects++;
				}
			}
			// odd number means inside
			return (nrIntersects % 2 == 1);
		}

		#endregion private helper methods
	}
}
