using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	/// <summary>
	/// The careful computer driver always goes slow and orientates at the inner curve
	/// </summary>
	internal class ComputerDriverCareful : IComputerDriver
	{
		#region IComputerDriver

		/// <summary>
		/// IComputerDriver
		/// </summary>
		public void GetNextGridPoint(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint,
			out uint nrBacktracks, out Point gridPoint)
		{
			if (routeGridPoints.Count == 0)
			{
				GetNextGridPoint_Count0(track, out nrBacktracks, out gridPoint);
			}
			else if (routeGridPoints.Count == 1)
			{
				GetNextGridPoint_Count1(track, car, routeGridPoints, out nrBacktracks, out gridPoint);
			}
			else
			{
				GetNextGridPoint_CountGreater1(track, car, routeGridPoints, middleGridPoint, out nrBacktracks, out gridPoint);				
			}
		}

		#endregion IComputerDriver

		#region private helper methods

		#region choose point on starting line scenario

		/// <summary>
		/// get the next grid point for scenario: count = 0 - choose point on starting line
		/// </summary>
		private static void GetNextGridPoint_Count0(ITrack track, out uint nrBacktracks, out Point gridPoint)
		{
			// handle selection of point on starting line
			nrBacktracks = 0;
			gridPoint = ChoosePointOnStartingLine(track);
		}

		/// <summary>
		/// choose point on the starting line
		/// </summary>
		private static Point ChoosePointOnStartingLine(ITrack track)
		{
			IList<Point> startingLinePoints = track.GetStartingLinePoints();
			if ((startingLinePoints == null) || (startingLinePoints.Count == 0))
			{
				throw new ArgumentException("starting line points are empty!");
			}

			Point rightmostStartingLinePoint = new Point(double.MinValue, double.MinValue);
			for (int index = 0; index < startingLinePoints.Count; index++)
			{
				if (startingLinePoints[index].X > rightmostStartingLinePoint.X)
				{
					// this point is more right (inner curve) than our current candidate
					rightmostStartingLinePoint.X = startingLinePoints[index].X;
					rightmostStartingLinePoint.Y = startingLinePoints[index].Y;
				}
			}

			return rightmostStartingLinePoint;
		}

		#endregion choose point on starting line scenario

		#region choose end point of first movement vector

		/// <summary>
		/// get the next grid point for scenario: count = 1 - choose first movement vector
		/// </summary>
		private static void GetNextGridPoint_Count1(ITrack track, ICar car, IList<Point> routeGridPoints, 
						out uint nrBacktracks, out Point gridPoint)
		{
			// handle start, i.e. choose first vector
			// remove candidate points that are not valid
			IList<Point> candidateGridPointsFiltered = ComputerDriverHelper.FilterCandidatePoints_RemoveInvalids
										(track, car.GetCandidateGridPoints(routeGridPoints), routeGridPoints);
			if (candidateGridPointsFiltered.Count > 0)
			{
				nrBacktracks = 0;
				gridPoint = ChooseFirstMovementEndPoint(candidateGridPointsFiltered);
			}
			else
			{
				// not really a solution... but seems never to happen
				nrBacktracks = 1;
			}
		}

		/// <summary>
		/// special scenario choose among the candidate grid points for the starting line point
		/// </summary>
		private static Point ChooseFirstMovementEndPoint(IList<Point> candidateGridPoints)
		{
			if ((candidateGridPoints == null) || (candidateGridPoints.Count == 0))
			{
				throw new ArgumentException("candidateGridPoints is empty!");
			}

			// we can safely assume that the starting line is horizontal, therefore
			// get the end point with highest x and highest y
			// because that is the point closest to the start line (highest y coordinate) and 
			// closest to the inner curve (highest x coordinate)
			Point firstMovementEndPoint = candidateGridPoints[0];
			for (int index = 1; index < candidateGridPoints.Count; index++)
			{
				if (candidateGridPoints[index].X > firstMovementEndPoint.X)
				{
					firstMovementEndPoint.X = candidateGridPoints[index].X;
					firstMovementEndPoint.Y = candidateGridPoints[index].Y;
				}
				else if (candidateGridPoints[index].X == firstMovementEndPoint.X)
				{
					if (candidateGridPoints[index].Y > firstMovementEndPoint.Y)
					{
						firstMovementEndPoint.X = candidateGridPoints[index].X;
						firstMovementEndPoint.Y = candidateGridPoints[index].Y;
					}
				}
			}

			return firstMovementEndPoint;
		}

		#endregion choose end point of first movement vector

		#region normal scenario - we are already moving

		private static void GetNextGridPoint_CountGreater1(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint,
					out uint nrBacktracks, out Point gridPoint)
		{
			nrBacktracks = 0;
			// get candidate grid point set from car and then remove those that are invalid because of the track and remove the current point
			IList<Point> candidateGridPointsFiltered = ComputerDriverHelper.FilterCandidatePoints_RemoveInvalids
											(track, car.GetCandidateGridPoints(routeGridPoints), routeGridPoints);
			if (candidateGridPointsFiltered.Count == 0)
			{
				// remark this is not a solution, because we do not have a better grid point
				nrBacktracks = 1;
				return;
			}

			// now find the "optimum" among the valid candidate grid points
			bool optimumPointFound = false;
			ComputerDriverHelper.GetNextGridPoint_CountGreater1_FindOptimum(routeGridPoints, candidateGridPointsFiltered, middleGridPoint, true, out optimumPointFound, out gridPoint);
			if (optimumPointFound == false)
			{
				// no optimum was found, this can happen because all grid points have an "invalid" direction
				// take one of the closest to the current point, ignoring the direction
				IList<Point> closestPoints2CurrentPoint = VectorMath.FindPointsInTheListClosest2FixPoint(routeGridPoints.Last(), candidateGridPointsFiltered);
				gridPoint = closestPoints2CurrentPoint[0];
			}
		}

		#endregion normal scenario

		#endregion private helper methods
	}
}
