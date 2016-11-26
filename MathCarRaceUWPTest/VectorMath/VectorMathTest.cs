using MathCarRaceUWP;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWPTest
{
	[TestClass]
	public class VectorMathTest
	{
		/// <summary>
		/// Two identical lines
		/// </summary>
		[TestMethod]		
		public void VectorMath_CheckIf2LinesIntersect_Identical()
		{
			Point A1 = new Point(1, 1);
			Point A2 = new Point(2, 2);
			Point B1 = new Point(1, 1);
			Point B2 = new Point(2, 2);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsTrue(result);
		}

		/// <summary>
		/// two parallel lines that do not intersect
		/// </summary>
		[TestMethod]
		public void VectorMath_CheckIf2LinesIntersect_Parallel()
		{
			Point A1 = new Point(1, 1);
			Point A2 = new Point(2, 2);
			Point B1 = new Point(2, 1);
			Point B2 = new Point(3, 2);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsFalse(result);
		}

		/// <summary>
		/// One line starts where the other ends
		/// </summary>
		[TestMethod]
		public void VectorMath_CheckIf2LinesIntersect_01()
		{
			Point A1 = new Point(1, 1);
			Point A2 = new Point(2, 2);
			Point B1 = new Point(2, 2);
			Point B2 = new Point(3, 4);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsTrue(result);
		}

		/// <summary>
		/// 2 lines that intersect in the middle
		/// </summary>
		[TestMethod]
		public void VectorMath_CheckIf2LinesIntersect_CrossingInMiddle()
		{
			Point A1 = new Point(1, 1);
			Point A2 = new Point(3, 3);
			Point B1 = new Point(3, 1);
			Point B2 = new Point(1, 3);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsTrue(result);
		}

		/// <summary>
		/// 2 lines that do not intersect
		/// </summary>
		[TestMethod]
		public void VectorMath_CheckIf2LinesIntersect_Nointersection()
		{
			Point A1 = new Point(1, 1);
			Point A2 = new Point(10, 10);
			Point B1 = new Point(10, 1);
			Point B2 = new Point(5, 4);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsFalse(result);
		}

		/// <summary>
		/// 2 lines that do intersect
		/// angle ... tolerance
		/// </summary>
		[TestMethod]
		public void VectorMath_CheckIf2LinesIntersect_AngleTolerance()
		{
			Point A1 = new Point(2, 14);
			Point A2 = new Point(4, 14);
			Point B1 = new Point(4, 13);
			Point B2 = new Point(2, 16);
			bool result = VectorMath.CheckIf2LinesIntersect(A1, A2, B1, B2);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void VectorMath_FindOnePointInTheListClosest2FixPoint_01()
		{
			Point fixPoint = new Point(2, 2);
			IList<Point> pointList = new List<Point>();
			pointList.Add(new Point(3, 3));
			pointList.Add(new Point(3, 2));
			pointList.Add(new Point(2, 3));
			IList<Point> resultList = VectorMath.FindPointsInTheListClosest2FixPoint(fixPoint, pointList);
			Assert.AreEqual(2, resultList.Count);
		}

		[TestMethod]
		public void VectorMath_FindOnePointInTheListClosest2FixPoint_02()
		{
			Point fixPoint = new Point(2, 2);
			IList<Point> pointList = new List<Point>();
			pointList.Add(new Point(3, 3));
			pointList.Add(new Point(3, 2));
			pointList.Add(new Point(2, 3));
			pointList.Add(new Point(1, 1));
			pointList.Add(new Point(1, 2));
			IList<Point> resultList = VectorMath.FindPointsInTheListClosest2FixPoint(fixPoint, pointList);
			Assert.AreEqual(3, resultList.Count);
		}

		[TestMethod]
		public void VectorMath_DetermineQuadrant()
		{
			uint quadrant;

			// 1st quadrant, on the same line to the left, angle = 0
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(9, 10));
			Assert.AreEqual((uint) 1, quadrant);

			// 1st quadrant, angle 45 degree (PI/4)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(7, 7));
			Assert.AreEqual((uint) 1, quadrant);

			// 2nd quadrant, angle 90 degree (PI/2)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(10, 8));
			Assert.AreEqual((uint)2, quadrant);

			// 2nd quadrant, angle 135 degree (3/4 PI)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(11, 9));
			Assert.AreEqual((uint) 2, quadrant);

			// 3rd quadrant, angle 180 degree (PI)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(11, 10));
			Assert.AreEqual((uint) 3, quadrant);

			// 3rd quadrant, angle 225 degree (5/4 PI)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(12, 12));
			Assert.AreEqual((uint) 3, quadrant);

			// 3rd quadrant...
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(1100, 5000));
			Assert.AreEqual((uint) 3, quadrant);

			// 4th quadrant, angle 270 degree (3/2 PI)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(10, 14));
			Assert.AreEqual((uint) 4, quadrant);

			// 4th quadrant, angle 315 degree (7/4 PI)
			quadrant = VectorMath.DetermineQuadrant(new Point(10, 10), new Point(9, 11));
			Assert.AreEqual((uint)4, quadrant);
		}
	}
}
