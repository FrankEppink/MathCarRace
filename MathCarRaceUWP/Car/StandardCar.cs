using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using System;

namespace MathCarRaceUWP
{
	internal class StandardCar : ICar
	{
		#region member

		/// <summary>
		/// a standard car has an acceleration of x, 
		/// which means brake by x and accelerate by x
		/// </summary>
		private readonly uint mAcceleration;

		#endregion member

		#region constructor

		/// <summary>
		/// Standard car has max de-acceleration equal acceleration
		/// </summary>
		/// <param name="acceleration"></param>
		internal StandardCar(uint acceleration)
		{
			mAcceleration = acceleration;
		}

		#endregion constructor

		#region ICar

		public bool IsValid(IList<Point> routeGridPoints, Point gridPointClicked)
		{
			if ((routeGridPoints == null) | (routeGridPoints.Count == 0))
			{
				// very first click - nothing to validate here
				return true;
			}

			if (routeGridPoints.Count == 1)
			{
				return CarHelper.CheckSecondPoint(routeGridPoints.Last(), gridPointClicked);
			}

			return CarHelper.CheckNextPoint
				(routeGridPoints[routeGridPoints.Count - 2], routeGridPoints.Last(), gridPointClicked, mAcceleration);
		}

		/// <summary>
		/// Get the list of candidate grid points, i.e. the possible grid points for the next move
		/// </summary>
		/// <returns></returns>
		public IList<Point> GetCandidateGridPoints(IList<Point> routeGridPoints)
		{
			IList<Point> cgp;
			
			if (routeGridPoints.Count == 0)
			{
				// nothing to do, return empty list
				cgp = new List<Point>();
			}
			else if (routeGridPoints.Count == 1)
			{
				// special case, we are on the starting line
				Point currentPosition = routeGridPoints.Last();
				double leftX = currentPosition.X - mAcceleration;
				double rightX = currentPosition.X + mAcceleration;
				double topY = currentPosition.Y - mAcceleration;
				double bottomY = currentPosition.Y - 1;	// need to be above the starting line

				cgp = AddCandidates(leftX, rightX, topY, bottomY);
			}
			else
			{
				// standard case, we have a movement vector
				Point currentPosition = routeGridPoints.Last();
				Point previousPosition = routeGridPoints[routeGridPoints.Count - 2];
				Point nextMiddlePoint = CarHelper.CalculateNextMiddlePoint(previousPosition, currentPosition);

				// now collect all points that are around the nextMiddlePoint, including the nextMiddlePoint
				double leftX = nextMiddlePoint.X - mAcceleration;
				double rightX = nextMiddlePoint.X + mAcceleration;
				double topY = nextMiddlePoint.Y - mAcceleration;
				double bottomY = nextMiddlePoint.Y + mAcceleration;

				cgp = AddCandidates(leftX, rightX, topY, bottomY);
			}

			return cgp;
		}

		#endregion ICar

		#region private methods

		private IList<Point> AddCandidates(double leftX, double rightX, double topY, double bottomY)
		{
			IList<Point> cgp = new List<Point>();

			for (double xIndex = leftX; xIndex <= rightX; xIndex++)
			{
				for (double yIndex = topY; yIndex <= bottomY; yIndex++)
				{
					Point candidate = new Point(xIndex, yIndex);
					cgp.Add(candidate);
				}
			}

			return cgp;
		}
				
		#endregion private methods
	}
}
