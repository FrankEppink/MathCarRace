using MathCarRaceUWP;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWPTest.ComputerDriver
{
	[TestClass]
	public class ComputerDriverHelperTest
	{
		/// <summary>
		/// IsRecalculationOfBestRouteNecessary
		/// </summary>
		[TestMethod]
		public void ComputerDriverHelper_DirectionValid4Quadrant()
		{
			Point startPoint = new Point(1, 1);
			Point endPoint = new Point(2, 2);
			bool result;
			result = ComputerDriverHelper.DirectionValid4Quadrant(1, startPoint, endPoint);
			Assert.IsFalse(result);

			startPoint.Y = endPoint.Y + 5;	// correct for quadrant 1
			result = ComputerDriverHelper.DirectionValid4Quadrant(1, startPoint, endPoint);
			Assert.IsTrue(result);
			
			startPoint.X = endPoint.X + 1;	// not correct for quadrant 1
			result = ComputerDriverHelper.DirectionValid4Quadrant(1, startPoint, endPoint);
			Assert.IsFalse(result);

			startPoint.X = 0;	// not correct for quadrant 1
			result = ComputerDriverHelper.DirectionValid4Quadrant(3, startPoint, endPoint);
			Assert.IsFalse(result);

			startPoint.X = endPoint.X - 1; // correct for quadrant 2
			startPoint.Y = endPoint.Y + 1; // NOT correct for quadrant 2
			result = ComputerDriverHelper.DirectionValid4Quadrant(2, startPoint, endPoint);
			Assert.IsFalse(result);
		}

		/// <summary>
		/// FilterCandidatePoints_RemoveUnfavorableDir
		/// </summary>
		[TestMethod]
		public void ComputerDriverHelper_FilterCandidatePoints_RemoveUnfavorableDir()
		{
			Point middleGridPoint = new Point(20, 10);

			// routeGridPoints with a valid x+2, y-2 movement in the first quadrant
			IList<Point> routeGridPoints = new List<Point>();
			Point startPoint = new Point(5, 10);
			Point endPoint = new Point(7, 8);
			routeGridPoints.Add(startPoint);
			routeGridPoints.Add(endPoint);
			uint quadrant = VectorMath.DetermineQuadrant(middleGridPoint, endPoint);
			Assert.AreEqual((uint) 1, quadrant);
			bool resultValidity = ComputerDriverHelper.DirectionValid4Quadrant(quadrant, startPoint, endPoint);
			Assert.IsTrue(resultValidity);

			{
				// the regular list of candidates (for a standard car with acceleration = 1)
				// all of which are valid
				IList<Point> candidateGridPoints = new List<Point>();
				candidateGridPoints.Add(new Point(8, 5));
				candidateGridPoints.Add(new Point(8, 6));
				candidateGridPoints.Add(new Point(8, 7));
				candidateGridPoints.Add(new Point(9, 5));
				candidateGridPoints.Add(new Point(9, 6));
				candidateGridPoints.Add(new Point(9, 7));
				candidateGridPoints.Add(new Point(10, 5));
				candidateGridPoints.Add(new Point(10, 6));
				candidateGridPoints.Add(new Point(10, 7));

				IList<Point> filterResult = ComputerDriverHelper.FilterCandidatePoints_RemoveUnfavorableDir(candidateGridPoints, routeGridPoints, middleGridPoint);
				Assert.AreEqual(candidateGridPoints.Count, filterResult.Count);
			}

			{
				IList<Point> candidateGridPoints = new List<Point>();
				// 3 valid
				candidateGridPoints.Add(new Point(8, 5));
				candidateGridPoints.Add(new Point(8, 6));
				candidateGridPoints.Add(new Point(8, 7));
				// 5 invalid
				candidateGridPoints.Add(new Point(6, 5));	// x too small
				candidateGridPoints.Add(new Point(6, 100)); // x too small, y too big
				candidateGridPoints.Add(new Point(10, 100)); // y too small
				candidateGridPoints.Add(new Point(1, 1));
				candidateGridPoints.Add(new Point(1000, 1000));				

				IList<Point> filterResult = ComputerDriverHelper.FilterCandidatePoints_RemoveUnfavorableDir(candidateGridPoints, routeGridPoints, middleGridPoint);
				Assert.AreEqual(3, filterResult.Count);
			}

			{
				IList<Point> candidateGridPoints = new List<Point>();
				
				// 5 invalid
				candidateGridPoints.Add(new Point(6, 5));   // x too small
				candidateGridPoints.Add(new Point(6, 100)); // x too small, y too big
				candidateGridPoints.Add(new Point(10, 100)); // y too small
				candidateGridPoints.Add(new Point(1, 1));   // x too small
				candidateGridPoints.Add(new Point(1000, 1000));	// y too big

				IList<Point> filterResult = ComputerDriverHelper.FilterCandidatePoints_RemoveUnfavorableDir(candidateGridPoints, routeGridPoints, middleGridPoint);
				Assert.AreEqual(0, filterResult.Count);
			}

			{
				IList<Point> candidateGridPoints = new List<Point>();

				IList<Point> filterResult = ComputerDriverHelper.FilterCandidatePoints_RemoveUnfavorableDir(candidateGridPoints, routeGridPoints, middleGridPoint);
				Assert.AreEqual(0, filterResult.Count);
			}
		}			
	}
}
