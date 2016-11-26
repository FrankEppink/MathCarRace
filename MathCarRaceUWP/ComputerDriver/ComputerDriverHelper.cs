using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	internal static class ComputerDriverHelper
	{
		/// <summary>
		/// filter the candidate grid points, i.e. remove those that are no real candidates,
		/// because they are not suitable for the track
		/// or they are the source grid point
		/// </summary>
		internal static IList<Point> FilterCandidatePoints_RemoveInvalids(ITrack track, IList<Point> candidateGridPoints, IList<Point> routeGridPoints)
		{
			if (routeGridPoints.Count > 0)
			{
				// remove the current grid point, i.e. the current position of the car
				// this might be included but we do not want that as the next move end point
				candidateGridPoints.Remove(routeGridPoints.Last());
			}

			// also remove those grid points that are not valid on this track
			return candidateGridPoints.Where(x => (track.ValidateVector(routeGridPoints, x))).ToList();
		}

		/// <summary>
		/// filter the candidate grid points, i.e. remove those that have an unfavorable direction regarding the quadrant
		/// </summary>
		internal static IList<Point> FilterCandidatePoints_RemoveUnfavorableDir(IList<Point> candidateGridPoints, IList<Point> routeGridPoints, Point middleGridPoint)
		{
			if (routeGridPoints.Count == 0)
			{
				// nothing to filter, doing selection on starting line where there is no direction
				return candidateGridPoints;
			}

			IList<Point> filterResult = new List<Point>();

			for (int index = 0; index < candidateGridPoints.Count; index++)
			{
				uint quadrant = VectorMath.DetermineQuadrant(middleGridPoint, candidateGridPoints[index]);
				bool validDir = DirectionValid4Quadrant(quadrant, routeGridPoints.Last(), candidateGridPoints[index]);
				if (validDir)
				{
					filterResult.Add(candidateGridPoints[index]);
				}
			}

			return filterResult;
		}

		/// <summary>
		/// Among the given filtered (invalid have been removed) candidate grid points find the optimum
		/// </summary>
		/// <param name="routeGridPoints">the current route grid points</param>
		/// <param name="candidateGridPointsFiltered">the filtered candidated grid points</param>
		/// <param name="middleGridPoint">middle grid point</param>
		/// <param name="closest">
		/// true: find the point that is closest to middle and to current grid point
		/// false: find the point that is farthest from middle point and farthest from current grid point</param>
		/// <param name="nextPointFound">out bool: true = a next point was find</param>
		/// <param name="gridPoint">the found next point</param>
		internal static void GetNextGridPoint_CountGreater1_FindOptimum(IList<Point> routeGridPoints, IList<Point> candidateGridPointsFiltered,
					Point middleGridPoint, bool closest, out bool nextPointFound, out Point gridPoint)
		{
			// find that point which is 
			// - closest to last point
			// - closest to the inner curve, i.e. closest to the middle grid point.
			// - does not go in the opposite direction of current quadrant
			nextPointFound = false;
			while (nextPointFound == false)
			{
				// 1. last point distance
				IList<Point> points2CurrentPoint;
				if (closest)
				{
					points2CurrentPoint = VectorMath.FindPointsInTheListClosest2FixPoint(routeGridPoints.Last(), candidateGridPointsFiltered);
				}
				else
				{
					points2CurrentPoint = VectorMath.FindPointsInTheListFarthest2FixPoint(routeGridPoints.Last(), candidateGridPointsFiltered);
				}

				if (points2CurrentPoint.Count == 0)
				{
					return;
				}

				// remove them from the candidateGridPoints list, so that they are not in the list in case we need to have another go in this loop
				candidateGridPointsFiltered = candidateGridPointsFiltered.Except(points2CurrentPoint).ToList();

				bool secondFoundFlag = false;
				while (secondFoundFlag == false)
				{
					// 2. distance to the inner curve, i.e. distance to the middle grid point.
					IList<Point> points2MiddleGridPoint;
					if (closest)
					{
						points2MiddleGridPoint = VectorMath.FindPointsInTheListClosest2FixPoint(middleGridPoint, points2CurrentPoint);
					}
					else
					{
						points2MiddleGridPoint = VectorMath.FindPointsInTheListFarthest2FixPoint(middleGridPoint, points2CurrentPoint);
					}

					if ((points2MiddleGridPoint == null) || (points2MiddleGridPoint.Count == 0))
					{
						// leave this inner loop to continue searching with the remaining points in candidateGridPoints
						break;
					}

					// remove them from the points2CurrentPoint list, 
					// so that they are not in the list in case we need to have another go in this loop
					points2CurrentPoint = points2CurrentPoint.Except(points2MiddleGridPoint).ToList();

					if (routeGridPoints.Count >= 3)
					{
						// filter candidates with a unfavorable direction (opposite direction for the current quadrant)
						IList<Point> filterResult = FilterCandidatePoints_RemoveUnfavorableDir
									(points2MiddleGridPoint, routeGridPoints, middleGridPoint);
						if (filterResult.Count > 0)
						{
							// now take the first that does not go into the quadrant opposite direction
							secondFoundFlag = true;
							nextPointFound = true;
							gridPoint = filterResult.First();
						}						
					}
					else
					{
						// only 2 grid points so far, we do not need to do the pre-pre-vector check because there is no pre-pre-vector
						// just take the first in closestPoints2MiddleGridPoint
						secondFoundFlag = true;
						nextPointFound = true;
						gridPoint = points2MiddleGridPoint[0];
					}
				}
			}
		}

		/// <summary>
		/// check if the vector is "valid" for the current quadrant
		/// "valid" means the normal direction that should be taken in this quadrant
		/// </summary>
		internal static bool DirectionValid4Quadrant(uint quadrant, Point start, Point end)
		{
			Point forbiddenDirection = DetermineForbiddenMovement(quadrant);
			Point actualDirection = new Point(end.X - start.X, end.Y - start.Y);
			if (forbiddenDirection.X > 0)
			{
				if (actualDirection.X > 0)
				{
					return false;
				}
			}
			else if (forbiddenDirection.X < 0)
			{
				if (actualDirection.X < 0)
				{
					return false;
				}
			}

			if (forbiddenDirection.Y > 0)
			{
				if (actualDirection.Y > 0)
				{
					return false;
				}
			}
			else if (forbiddenDirection.Y < 0)
			{
				if (actualDirection.Y < 0)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Return the "forbidden" movement for the given quadrant.
		/// "forbidden" means that it is a step back
		/// </summary>
		private static Point DetermineForbiddenMovement(uint quadrant)
		{
			Point forbiddenDirection = new Point();
			switch (quadrant)
			{
				case 1:
					forbiddenDirection.X = -1;
					forbiddenDirection.Y = 1;
					break;
				case 2:
					forbiddenDirection.X = -1;
					forbiddenDirection.Y = -1;
					break;
				case 3:
					forbiddenDirection.X = 1;
					forbiddenDirection.Y = -1;
					break;
				case 4:
					forbiddenDirection.X = 1;
					forbiddenDirection.Y = 1;
					break;
				default:
					throw new ArgumentException("quadrant <1 or >4");
			}

			return forbiddenDirection;
		}
	}
}
