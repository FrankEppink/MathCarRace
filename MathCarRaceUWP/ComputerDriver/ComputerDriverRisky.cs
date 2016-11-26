using System.Collections.Generic;
using Windows.Foundation;
using System;
using System.Linq;

namespace MathCarRaceUWP
{
	/// <summary>
	/// The risky computer driver always tries top speed and outer curve
	/// </summary>
	internal class ComputerDriverRisky : IComputerDriver
	{
		#region member

		/// <summary>
		/// stores the current backtrack level, i.e. what is the number of the grid point that we have to go back to
		/// </summary>
		private uint mBacktrackLevel = 0;

		/// <summary>
		/// This list contains those grid points that already have been checked but ended in a dead-end and were therefore backtracked
		/// </summary>
		private IList<Point> mBacktrackedRouteGridPoints = new List<Point>();

		#endregion member

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

		private void GetNextGridPoint_Count0(ITrack track, out uint nrBacktracks, out Point gridPoint)
		{
			// handle selection of point on starting line
			nrBacktracks = 0;
			gridPoint = ChoosePointOnStartingLine(track);

			mBacktrackLevel = 0;
		}

		/// <summary>
		/// choose point on the starting line
		/// </summary>
		/// <param name="track"></param>
		/// <returns></returns>
		private static Point ChoosePointOnStartingLine(ITrack track)
		{
			IList<Point> startingLinePoints = track.GetStartingLinePoints();
			if ((startingLinePoints == null) || (startingLinePoints.Count == 0))
			{
				throw new ArgumentException("starting line points are empty!");
			}

			Point leftmostStartingLinePoint = new Point(double.MaxValue, double.MaxValue);
			for (int index = 0; index < startingLinePoints.Count; index++)
			{
				if (startingLinePoints[index].X < leftmostStartingLinePoint.X)
				{
					// this point is more left (outer curve) than our current candidate
					leftmostStartingLinePoint.X = startingLinePoints[index].X;
					leftmostStartingLinePoint.Y = startingLinePoints[index].Y;
				}
			}

			return leftmostStartingLinePoint;
		}

		#endregion choose point on starting line scenario

		#region choose end point of first movement vector

		private void GetNextGridPoint_Count1(ITrack track, ICar car, IList<Point> routeGridPoints,
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

				mBacktrackLevel = 1;
			}
			else
			{
				nrBacktracks = 1;
				mBacktrackLevel = 0;
			}
		}

		private static Point ChooseFirstMovementEndPoint(IList<Point> candidateGridPoints)
		{
			if ((candidateGridPoints == null) || (candidateGridPoints.Count == 0))
			{
				throw new ArgumentException("candidateGridPoints is empty!");
			}

			// we can safely assume that the starting line is horizontal, therefore
			// get the end point with lowest x and lowest y coordinate
			// because that is the point farthest away (lowest y coordinate) and closest to the outer curve (lowest x coordinate)
			Point firstMovementEndPoint = new Point(double.MaxValue, double.MaxValue);
			for (int index = 0; index < candidateGridPoints.Count; index++)
			{
				if (candidateGridPoints[index].Y < firstMovementEndPoint.Y)
				{
					// this point is farther (lower y coordinate) than our current candidate					
					firstMovementEndPoint.X = candidateGridPoints[index].X;
					firstMovementEndPoint.Y = candidateGridPoints[index].Y;
				}
				else if (candidateGridPoints[index].Y == firstMovementEndPoint.Y)
				{
					if (candidateGridPoints[index].X < firstMovementEndPoint.X)
					{
						// this point is more outer/left (lower x coordinate) than our current candidate					
						firstMovementEndPoint.X = candidateGridPoints[index].X;
						firstMovementEndPoint.Y = candidateGridPoints[index].Y;
					}
				}
			}

			return firstMovementEndPoint;
		}

		#endregion choose end point of first movement vector

		#region normal scenario - we are already moving

		private void GetNextGridPoint_CountGreater1(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint,
					out uint nrBacktracks, out Point gridPoint)
		{
			nrBacktracks = 0;
			// get candidate grid point set from car and then remove those that are invalid because of the track and remove the current point
			IList<Point> candidateGridPointsFiltered = ComputerDriverHelper.FilterCandidatePoints_RemoveInvalids
											(track, car.GetCandidateGridPoints(routeGridPoints), routeGridPoints);
			if (candidateGridPointsFiltered.Count == 0)
			{
				GetNextGridPoint_CountGreater1_Backtrack(track, car, routeGridPoints, middleGridPoint, out nrBacktracks, out gridPoint);
			}
			else
			{
				mBacktrackLevel = 0;

				// now find the "optimum" among the valid candidate grid points
				bool optimumPointFound = false;
				ComputerDriverHelper.GetNextGridPoint_CountGreater1_FindOptimum(routeGridPoints, candidateGridPointsFiltered, middleGridPoint, false, out optimumPointFound, out gridPoint);
				if (optimumPointFound == false)
				{
					// no optimum was found, this can happen because all grid points have an "invalid" direction
					// take one of the closest to the current point, ignoring the direction
					IList<Point> closestPoints2CurrentPoint = VectorMath.FindPointsInTheListClosest2FixPoint(routeGridPoints.Last(), candidateGridPointsFiltered);
					gridPoint = closestPoints2CurrentPoint[0];
				}
			}
		}

		private void GetNextGridPoint_CountGreater1_Backtrack(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint,
						out uint nrBacktracks, out Point gridPoint)
		{
			bool foundOne = false;
			nrBacktracks = 0;	// only a dummy value

			while (foundOne == false)
			{
				mBacktrackLevel++;
				nrBacktracks = mBacktrackLevel;

				// doing backtracking of routeGridPoints
				IList<Point> routeGridPointsLessTheLast = routeGridPoints.ToList();
				for (uint index = 0; index < mBacktrackLevel; index++)
				{
					// add those that we backtrack to backtrack list so that we do not try them again
					if (mBacktrackedRouteGridPoints.Contains(routeGridPointsLessTheLast[routeGridPointsLessTheLast.Count - 1]) == false)
					{
						mBacktrackedRouteGridPoints.Add(routeGridPointsLessTheLast[routeGridPointsLessTheLast.Count - 1]);
					}
					routeGridPointsLessTheLast.RemoveAt(routeGridPointsLessTheLast.Count - 1);
				}

				IList<Point> candidateGridPointsFilteredLessTheLast = ComputerDriverHelper.FilterCandidatePoints_RemoveInvalids
										(track, car.GetCandidateGridPoints(routeGridPointsLessTheLast), routeGridPointsLessTheLast);
				// remove the backtracked points
				IList<Point> nextFilter = candidateGridPointsFilteredLessTheLast.Except(mBacktrackedRouteGridPoints).ToList();
				if (nextFilter.Count > 0)
				{
					foundOne = true;
					bool carefulOptimumPointFound;
					// we are too risky, use the careful computer driver -> option 4: closest = true
					ComputerDriverHelper.GetNextGridPoint_CountGreater1_FindOptimum(routeGridPointsLessTheLast, nextFilter,
												middleGridPoint, true, out carefulOptimumPointFound, out gridPoint);
				}
			}
		}

		#endregion normal scenario - we are already moving

		#endregion private helper methods
	}
}
