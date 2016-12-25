using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	/// <summary>
	/// The top computer driver calculates all possibilites (with favorable directions only)
	/// and chooses the shortest one
	/// TODO: right now it seems that the first valid route is taken
	/// TODO: ends in an infinite loop when switched to 'top' driver in a dead-end situation
	/// </summary>
	internal class ComputerDriverTop : IComputerDriver
	{
		#region members

		private IList<Point> mCurrentBestRoute = new List<Point>();
				
		#endregion members

		#region IComputerDriver

		/// <summary>
		/// IComputerDriver
		/// </summary>
		public void GetNextGridPoint(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint,
			out uint nrBacktracks, out Point gridPoint)
		{
			// the top computer driver makes no mistakes and therefore never backtracks (from the grid background point of view) :-)
			nrBacktracks = 0;
			bool recalc = ComputerDriverTopHelper.IsRecalculationOfBestRouteNecessary(routeGridPoints, mCurrentBestRoute);
			if (recalc)
			{
				// do it
				mCurrentBestRoute.Clear();
				IList<Point> currentRoute = routeGridPoints.ToList();
				CalculateBestRoute(track, car, currentRoute, middleGridPoint);
			}

			if (mCurrentBestRoute.Count == 0)
			{
				// TODO handle this correctly, also it must be
				// mCurrentBestRoute.Count < routeGridPoints.Count or similar
				nrBacktracks = 1;
			}
			else
			{
				// return the current next best grid point
				gridPoint = mCurrentBestRoute[routeGridPoints.Count];
			}
		}

		#endregion IComputerDriver

		#region private methods
				
		private void CalculateBestRoute(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint)
		{
			IList<Point> candidateGridPoints = GetCandidatePoints(track, car, routeGridPoints);
			IList<Point> candidateGridPointsFiltered = ComputerDriverHelper.FilterCandidatePoints_RemoveInvalids
											(track, candidateGridPoints, routeGridPoints);
			if (candidateGridPointsFiltered.Count == 0)
			{
				// backtrack, because we are at a dead end
				routeGridPoints.RemoveAt(routeGridPoints.Count - 1);
				return;
			}

			// we only take candidate grid points with a quadrant valid direction
			IList<Point> candidateGridPointsFilteredQuadrantDir = ComputerDriverHelper.FilterCandidatePoints_RemoveUnfavorableDir
				(candidateGridPointsFiltered, routeGridPoints, middleGridPoint);
			for (int index=0; index < candidateGridPointsFilteredQuadrantDir.Count; index++)
			{
				Point newPoint = candidateGridPointsFilteredQuadrantDir[index];
				
				if (track.CheckIfRaceIsFinished(routeGridPoints, newPoint))
				{
					// we have found one complete route that finishes
					routeGridPoints.Add(newPoint);
					// now compare if this is better than the previous one
					if (IsThisBetterThanCurrentBestRoute(routeGridPoints))
					{
						// yes it is better -> overwrite
						mCurrentBestRoute = routeGridPoints.ToList();
					}
					else
					{
						// ignore it
						routeGridPoints.RemoveAt(routeGridPoints.Count - 1);
					}
				}
				else
				{
					// continue route to finish the race
					routeGridPoints.Add(newPoint);
					CalculateBestRoute(track, car, routeGridPoints, middleGridPoint);
				}
			}
		}
		
		private static IList<Point> GetCandidatePoints(ITrack track, ICar car, IList<Point> routeGridPoints)
		{
			if (routeGridPoints.Count == 0)
			{
				return track.GetStartingLinePoints();
			}
			else
			{
				return car.GetCandidateGridPoints(routeGridPoints);				
			}			
		}

		private bool IsThisBetterThanCurrentBestRoute(IList<Point> routeGridPoints)
		{
			if (mCurrentBestRoute.Count == 0)
			{
				// always better than the empty route
				return true;
			}
			else if (routeGridPoints.Count < mCurrentBestRoute.Count)
			{
				// shorter than the current best route
				return true;
			}

			return false;
		}

		#endregion private methods
	}
}
