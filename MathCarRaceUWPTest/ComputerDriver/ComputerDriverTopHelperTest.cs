using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MathCarRaceUWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MathCarRaceUWPTest
{
	[TestClass]
	public class ComputerDriverTopHelperTest
	{
		/// <summary>
		/// IsRecalculationOfBestRouteNecessary
		/// </summary>
		[TestMethod]
		public void ComputerDriverTopHelper_IsRecalculationOfBestRouteNecessary()
		{
			IList<Point> currentRoute = new List<Point>();
			IList<Point> bestRoute = new List<Point>();
			bool result;

			// bestRoute is empty, we need to recalculate -> result = true
			result = ComputerDriverTopHelper.IsRecalculationOfBestRouteNecessary(currentRoute, bestRoute);
			Assert.IsTrue(result);

			// currentRoute is longer than bestRoute -> we need to recalculate
			Point firstPoint = new Point(0, 0);
			currentRoute.Add(firstPoint);
			result = ComputerDriverTopHelper.IsRecalculationOfBestRouteNecessary(currentRoute, bestRoute);
			Assert.IsTrue(result);

			// currentRoute and bestRoute are identical -> we do NOT need to recalculate
			bestRoute.Add(firstPoint);
			result = ComputerDriverTopHelper.IsRecalculationOfBestRouteNecessary(currentRoute, bestRoute);
			Assert.IsFalse(result);

			// currentRoute is included in bestRoute -> we do NOT need to recalculate
			Point secondPoint = new Point(100, 20);
			bestRoute.Add(secondPoint);
			result = ComputerDriverTopHelper.IsRecalculationOfBestRouteNecessary(currentRoute, bestRoute);
			Assert.IsFalse(result);
		}

		/// <summary>
		/// IsFirstRouteStartofSecondRoute
		/// </summary>
		[TestMethod]
		public void ComputerDriverTopHelper_IsFirstRouteStartofSecondRoute()
		{
			IList<Point> firstRoute = new List<Point>();
			IList<Point> secondRoute = new List<Point>();
			bool result;

			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsTrue(result);

			Point firstPoint = new Point(0, 0);
			firstRoute.Add(firstPoint);
			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsFalse(result);

			secondRoute.Add(firstPoint);
			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsTrue(result);

			Point secondPoint = new Point(100, 20);
			secondRoute.Add(secondPoint);
			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsTrue(result);

			firstRoute.Add(secondPoint);
			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsTrue(result);

			Point thirdPoint = new Point(23, 23);
			secondRoute.Add(thirdPoint);
			secondRoute.Add(new Point(233, 234));

			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsTrue(result);

			firstRoute.Add(new Point(thirdPoint.X + 1, thirdPoint.Y));
			result = ComputerDriverTopHelper.IsFirstRouteStartofSecondRoute(firstRoute, secondRoute);
			Assert.IsFalse(result);
		}
	}
}
