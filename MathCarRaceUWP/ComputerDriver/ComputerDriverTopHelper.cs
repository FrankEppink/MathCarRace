using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	internal static class ComputerDriverTopHelper
	{
		/// <summary>
		/// this method checks if we need to (re-)calculate the best route
		/// </summary>
		/// <returns></returns>
		internal static bool IsRecalculationOfBestRouteNecessary(IList<Point> routeGridPoints, IList<Point> bestRouteGridPoints)
		{
			if (bestRouteGridPoints.Count == 0)
			{
				return true;
			}

			bool firstRouteStartOfBestRoute = IsFirstRouteStartofSecondRoute(routeGridPoints, bestRouteGridPoints);
			return !firstRouteStartOfBestRoute;
		}

		/// <summary>
		/// Check if the first route equals the start of the second route, i.e.
		/// if the first route is included in the second route and if
		/// extending the first route to get the second route is possible
		/// </summary>
		internal static bool IsFirstRouteStartofSecondRoute(IList<Point> firstRouteGridPoints, IList<Point> secondRouteGridPoints)
		{
			if ((firstRouteGridPoints == null) || (secondRouteGridPoints == null))
			{
				throw new ArgumentException("firstRouteGridPoints and/or secondRouteGridPoints are null");
			}

			// if the firstRouteGridPoints is longer than the second return false
			if (firstRouteGridPoints.Count > secondRouteGridPoints.Count)
			{
				// the firstRouteGridPoints is already longer than the secondRouteGridPoints -> return false
				return false;
			}

			bool diffFound = false;
			int index = 0;
			while ((diffFound == false) && (index < firstRouteGridPoints.Count))
			{
				if (firstRouteGridPoints[index] != secondRouteGridPoints[index])
				{
					diffFound = true;
				}
				else
				{
					index++;
				}
			}

			return !diffFound;
		}
	}
}
