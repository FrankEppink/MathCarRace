using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Static class for vector mathematic methods
	/// </summary>
	internal static class VectorMath
	{
		#region internal methods

		/// <summary>
		/// Determine if the two given lines intersect
		/// http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/fa0cfeb6-70b7-4181-bc9b-fe625cd5e159
		/// </summary>
		internal static bool CheckIf2LinesIntersect(Point lineA1, Point lineA2, Point lineB1, Point lineB2)
		{
			// first line A
			double diffAY = lineA2.Y - lineA1.Y;
			double diffAX = lineA1.X - lineA2.X;
			double A = diffAY * lineA1.X + diffAX * lineA1.Y;

			// second line
			double diffBY = lineB2.Y - lineB1.Y;
			double diffBX = lineB1.X - lineB2.X;
			double B = diffBY * lineB1.X + diffBX * lineB1.Y;

			// now assume that we do not have line segments but infinite lines
			double det = diffAY * diffBX - diffBY * diffAX;
			if (det == 0)
			{
				// Lines are parallel
				return CheckIf2LinesIntersect_ParallelScenario(lineA1, lineA2, lineB1, lineB2);
			}
			else
			{
				// now calculate the candidate point for intersection
				Point pointC = new Point();
				pointC.X = (diffBX * A - diffAX * B) / det;
				pointC.Y = (diffAY * B - diffBY * A) / det;

				// now we need to check if the found intersection point lies on both lines
				return ((CheckIfPointIsOnLine(lineA1, lineA2, pointC)) && (CheckIfPointIsOnLine(lineB1, lineB2, pointC)));
			}
		}

		/// <summary>
		/// Calculate the line endpoint that
		/// - starts in x2, y2
		/// - has the given length
		/// - has the given angle to line (x1, y1) -> (x2, y2)
		/// </summary>
		internal static Point CalculateLineEndpointSatisfyingAngle(Point p1, Point p2, double length, double angle)
		{
			// Finish point if the original line would be the horizontal line taken from here
			// http://forums.codeguru.com/showthread.php?120582-Draw-line-knowing-angle
			double angle2HorizontalLine = CalculateAngle(p1, p2, new Point(1.0, 0.0), new Point(2.0, 0.0));

			double x = p2.X + length * Math.Cos(angle + angle2HorizontalLine + Math.PI);
			double y = p2.Y + length * Math.Sin(angle + angle2HorizontalLine + Math.PI);

			return new Point(x, y);
		}

		/// <summary>
		/// Given the movement from p1 to p2, what would be p3 applying the same vector
		/// </summary>
		/// <param name="p1">start</param>
		/// <param name="p2">end or new start</param>
		/// <returns>new endpoint</returns>
		internal static Point ExtrapolateMovement(Point p1, Point p2)
		{
			Point p3 = new Point();
			p3.X = p2.X + (p2.X - p1.X);
			p3.Y = p2.Y + (p2.Y - p1.Y);
			return p3;
		}

		/// <summary>
		/// Find those points in pointList that have the same minimum difference to fix point
		/// </summary>
		/// <param name="fixPoint"></param>
		/// <param name="pointList"></param>
		/// <returns></returns>
		internal static IList<Point> FindPointsInTheListClosest2FixPoint(Point fixPoint, IList<Point> pointList)
		{
			if (pointList == null) { return new List<Point>(); };
			if (pointList.Count == 0) { return new List<Point>(); };

			// first find the closestDiff value
			Point closestPoint = pointList[0];
			double closestDiff = CalculateLength(fixPoint, closestPoint);
			for (int index= 1; index < pointList.Count; index++)
			{
				double newDiff = CalculateLength(fixPoint, pointList[index]);
				if (newDiff < closestDiff)
				{
					closestPoint = pointList[index];
					closestDiff = newDiff;
				}
			}

			// now find all in the list that have this difference
			IList<Point> closestPoints = new List<Point>();
			for (int index = 0; index < pointList.Count; index++)
			{
				double newDiff = CalculateLength(fixPoint, pointList[index]);
				if (newDiff == closestDiff)
				{
					closestPoints.Add(pointList[index]);
				}
			}

			return closestPoints;
		}

		/// <summary>
		/// Find those points in pointList that have the same maximum difference to fix point
		/// </summary>
		/// <param name="fixPoint"></param>
		/// <param name="pointList"></param>
		/// <returns></returns>
		internal static IList<Point> FindPointsInTheListFarthest2FixPoint(Point fixPoint, IList<Point> pointList)
		{
			if (pointList == null) { return new List<Point>(); };
			if (pointList.Count == 0) { return new List<Point>(); };

			// first find the closestDiff value
			Point farthestPoint = pointList[0];
			double farthestDiff = CalculateLength(fixPoint, farthestPoint);
			for (int index = 1; index < pointList.Count; index++)
			{
				double newDiff = CalculateLength(fixPoint, pointList[index]);
				if (newDiff > farthestDiff)
				{
					farthestPoint = pointList[index];
					farthestDiff = newDiff;
				}
			}

			// now find all in the list that have this difference
			IList<Point> farthestPoints = new List<Point>();
			for (int index = 0; index < pointList.Count; index++)
			{
				double newDiff = CalculateLength(fixPoint, pointList[index]);
				if (newDiff == farthestDiff)
				{
					farthestPoints.Add(pointList[index]);
				}
			}

			return farthestPoints;
		}

		/// <summary>
		/// Calculate the max difference as absolute value in x or y coordinate
		/// </summary>
		internal static double CalculateMaxXorYDiffAbsolute(Point p1, Point p2)
		{
			double xDiff = Math.Abs(p1.X - p2.X);
			double yDiff = Math.Abs(p1.Y - p2.Y);
			if (xDiff > yDiff)
			{
				return xDiff;
			}
			else
			{
				return yDiff;
			}
		}

		/// <summary>
		/// Calculate the angle between the 2 given lines
		/// http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/fa0cfeb6-70b7-4181-bc9b-fe625cd5e159
		/// </summary>
		internal static double CalculateAngle(Point lineA1, Point lineA2, Point lineB1, Point lineB2)
		{
			Double valueAngle = Math.Atan2(lineA2.Y - lineA1.Y, lineA2.X - lineA1.X) - Math.Atan2(lineB2.Y - lineB1.Y, lineB2.X - lineB1.X);
			return valueAngle;
		}

		/// <summary>
		/// Determine the quadrant the given grid point lies in
		/// 1: left top (first quadrant, starting quadrant)
		/// 2: right top (second quadrant)
		/// 3: right bottom (third quadrant)
		/// 4: left bottom (fourth quadrant, finishing quadrant)
		/// </summary>
		internal static uint DetermineQuadrant(Point middleGridPoint, Point gridPoint)
		{
			// calculate the angle between the line between the 
			// line middleGridPoint - gridPoint
			// and the starting line middleGridPoint, new Point(middleGridPoint.X - 1, middleGridPoint.Y)
			double angle = CalculateAngle(
				gridPoint, middleGridPoint,
				new Point(middleGridPoint.X - 1, middleGridPoint.Y), middleGridPoint				
				);

			if (angle < 0)
			{
				angle += (2*Math.PI);
			}

			if ((angle >= 0) && (angle < Math.PI/2)) { return 1; }
			if (angle < Math.PI) { return 2; }
			if (angle < (3*Math.PI/2)) { return 3; }
			return 4;
		}

		#endregion internal methods

		#region private methods

		private static double CalculateLength(Point p1, Point p2)
		{
			double result = Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
			return result;
		}

		/// <summary>
		/// Check if the two parallel lines intersect
		/// </summary>		
		private static bool CheckIf2LinesIntersect_ParallelScenario(Point lineA1, Point lineA2, Point lineB1, Point lineB2)
		{
			// either one of the endpoints is on the other line or the two lines have no intersection
			if (CheckIfPointIsOnLine(lineA1, lineA2, lineB1)) { return true; }
			if (CheckIfPointIsOnLine(lineA1, lineA2, lineB2)) { return true; }
			if (CheckIfPointIsOnLine(lineB1, lineB2, lineA1)) { return true; }
			if (CheckIfPointIsOnLine(lineB1, lineB2, lineA2)) { return true; }
			return false;
		}

		/// <summary>
		/// Check if the point lies on the line
		/// </summary>	
		private static bool CheckIfPointIsOnLine(Point lineA1, Point lineA2, Point pointC)
		{
			double lineXMin = Math.Min(lineA1.X, lineA2.X);
			double lineXMax = Math.Max(lineA1.X, lineA2.X);
			double lineYMin = Math.Min(lineA1.Y, lineA2.Y);
			double lineYMax = Math.Max(lineA1.Y, lineA2.Y);

			if (
					(lineXMin <= pointC.X)
				&& (lineXMax >= pointC.X)
				&& (lineYMin <= pointC.Y)
				&& (lineYMax >= pointC.Y)
				)
			{
				if ((lineA1.X == pointC.X) && (lineA1.Y == pointC.Y))
				{
					return true;
				}

				double angle = CalculateAngle(lineA1, lineA2, lineA1, pointC);

				// we need a certain tolerance
				double tolerance = .001;
				if (Math.Abs(angle) <= tolerance)
				// if (angle == 0.0)
				{
					return true;
				}

				return false;
			}

			return false;
		}

		#endregion private methods
	}
}
