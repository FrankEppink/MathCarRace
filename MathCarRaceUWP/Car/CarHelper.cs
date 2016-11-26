using System;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	internal static class CarHelper
	{
		/// <summary>
		/// calculate distance i.e. neighborhood
		/// </summary>
		internal static uint CalculateDistance(Point p1, Point p2)
		{
			uint xDiff = (uint) Math.Abs( (int) (p1.X - p2.X));
			uint yDiff = (uint) Math.Abs( (int) (p1.Y - p2.Y));

			return Math.Max(xDiff, yDiff);
		}

		/// <summary>
		/// check the second point, i.e. first point is on starting line, now the second point was clicked
		/// </summary>
		internal static bool CheckSecondPoint(Point startingLinePoint, Point secondPoint)
		{
			// first point is on starting grid, now second point chosen, must have distance 1
			uint d = CalculateDistance(startingLinePoint, secondPoint);
			if (d != 1)
			{
				return false;
			}

			// and must go "up"/"northwards"
			if (startingLinePoint.Y <= secondPoint.Y)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// calculated the next point, i.e. based on the previous movement vector the next "extrapolated" point
		/// </summary>
		internal static Point CalculateNextMiddlePoint(Point secondLastPoint, Point lastPoint)
		{
			return VectorMath.ExtrapolateMovement(secondLastPoint, lastPoint);
		}

		/// <summary>
		/// check if the next point is allowed, based on the preceding two points (and their vector) and the allowed distance
		/// </summary>
		internal static bool CheckNextPoint(Point secondLastPoint, Point lastPoint, Point nextPoint, uint allowedDistance)
		{
			Point nextMiddlePoint = CalculateNextMiddlePoint(secondLastPoint, lastPoint);
			uint dist = CalculateDistance(nextMiddlePoint, nextPoint);

			return (dist <= allowedDistance);
		}
	}
}
