using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Foundation;
using MathCarRaceUWP;

namespace MathCarRaceUWPTest
{
    [TestClass]
    public class CarHelperTest
    {
        [TestMethod]
        public void CalculateDistance()
        {
			Point p1;
			Point p2;
			
			uint d;

			p1.X = 0;
			p1.Y = 0;
			p2.X = 0;
			p2.Y = 0;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint) 0, d);

			p1.X = 1;
			p1.Y = 1;
			p2.X = 1;
			p2.Y = 1;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)0, d);

			p2.Y = 2;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)1, d);

			p2.X = 2;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)1, d);

			p2.X = 0;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)1, d);

			p2.Y = 0;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)1, d);

			p2.Y = 3;
			d = CarHelper.CalculateDistance(p1, p2);
			Assert.AreEqual((uint)2, d);
		}

		[TestMethod]
		public void CheckSecondPoint()
		{
			Point startingLinePoint;
			Point secondPoint;
			bool result;

			startingLinePoint.X = 0;
			startingLinePoint.Y = 0;
			secondPoint.X = 0;
			secondPoint.Y = 0;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsFalse(result);

			startingLinePoint.X = 100;
			startingLinePoint.Y = 0;
			secondPoint.X = 100;
			secondPoint.Y = 0;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsFalse(result);

			startingLinePoint.X = 0;
			startingLinePoint.Y = 0;
			secondPoint.X = 1;
			secondPoint.Y = 0;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsFalse(result);

			startingLinePoint.X = 0;
			startingLinePoint.Y = 1;
			secondPoint.X = 0;
			secondPoint.Y = 0;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsTrue(result);

			startingLinePoint.X = 0;
			startingLinePoint.Y = -1;
			secondPoint.X = 0;
			secondPoint.Y = 0;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsFalse(result);

			startingLinePoint.X = 0;
			startingLinePoint.Y = 0;
			secondPoint.X = 0;
			secondPoint.Y = -1;
			result = CarHelper.CheckSecondPoint(startingLinePoint, secondPoint);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CalculateNextPoint()
		{
			Point secondLastPoint;
			Point lastPoint;
			Point nextPoint;

			secondLastPoint.X = 0;
			secondLastPoint.Y = 0;
			lastPoint.X = 1;
			lastPoint.Y = 1;
			nextPoint = CarHelper.CalculateNextMiddlePoint(secondLastPoint, lastPoint);
			Assert.AreEqual(2, nextPoint.X);
			Assert.AreEqual(2, nextPoint.Y);

			secondLastPoint.X = 0;
			secondLastPoint.Y = 0;
			lastPoint.X = 2;
			lastPoint.Y = 2;
			nextPoint = CarHelper.CalculateNextMiddlePoint(secondLastPoint, lastPoint);
			Assert.AreEqual(4, nextPoint.X);
			Assert.AreEqual(4, nextPoint.Y);

			secondLastPoint.X = 50;
			secondLastPoint.Y = 40;
			lastPoint.X = 40;
			lastPoint.Y = 42;
			nextPoint = CarHelper.CalculateNextMiddlePoint(secondLastPoint, lastPoint);
			Assert.AreEqual(30, nextPoint.X);
			Assert.AreEqual(44, nextPoint.Y);
		}
	}
}
