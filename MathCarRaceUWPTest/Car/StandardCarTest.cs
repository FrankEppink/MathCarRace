using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Foundation;
using MathCarRaceUWP;
using System.Collections.Generic;

namespace MathCarRaceUWPTest
{
	[TestClass]
	public class StandardCarTest
	{
		[TestMethod]
		public void StandardCar_IsValidTest_01()
		{
			// StandardCar with acceleration = 1
			ICar sc = new StandardCar(1);

			IList<Point> routeGridPoints = new List<Point>();
			Point gridPointClicked = new Point();
			bool result;

			routeGridPoints.Add(new Point(2, 2));
			routeGridPoints.Add(new Point(4, 4));

			gridPointClicked.X = 5;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 5;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 5;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 4;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 5;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 8;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 4;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 5;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
			gridPointClicked.X = 8;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 3;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 8;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void StandardCar_IsValidTest_02()
		{
			// StandardCar with acceleration = 2
			ICar sc = new StandardCar(2);

			IList<Point> routeGridPoints = new List<Point>();
			Point gridPointClicked = new Point();
			bool result;

			routeGridPoints.Add(new Point(2, 2));
			routeGridPoints.Add(new Point(4, 4));

			gridPointClicked.X = 5;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 5;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 5;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 5;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 7;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 4;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 5;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 8;
			gridPointClicked.Y = 4;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 4;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 5;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 6;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 7;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);
			gridPointClicked.X = 8;
			gridPointClicked.Y = 8;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 4;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 8;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsTrue(result);

			gridPointClicked.X = 3;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 9;
			gridPointClicked.Y = 6;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 6;
			gridPointClicked.Y = 3;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);

			gridPointClicked.X = 6;
			gridPointClicked.Y = 9;
			result = sc.IsValid(routeGridPoints, gridPointClicked);
			Assert.IsFalse(result);
		}
	}
}
