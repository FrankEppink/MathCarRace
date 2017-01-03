using MathCarRaceUWP;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWPTest
{
	[TestClass]
	public class VectorMathUITest
	{
		/// <summary>
		/// empty collection
		/// </summary>
		[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
		public void VectorMathUI_CheckIfLineIntersectsWithLineCollection_01_EmptyColl()
		{
			Line oneLine = new Line();
			oneLine.X1 = 10;
			oneLine.Y1 = 10;
			oneLine.X2 = 20;
			oneLine.Y2 = 20;

			IList<Line> lineColl = new List<Line>();
			int position;
			bool result = VectorMathUI.CheckIfLineIntersectsWithLineCollection(oneLine, lineColl, out position);
			Assert.IsFalse(result);
		}

		/// <summary>
		/// collection with exactly one line, the line searched for
		/// </summary>
		[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
		public void VectorMathUI_CheckIfLineIntersectsWithLineCollection_02_CollContainsLine()
		{
			Line oneLine = new Line();
			oneLine.X1 = 10;
			oneLine.Y1 = 10;
			oneLine.X2 = 20;
			oneLine.Y2 = 20;

			IList<Line> lineColl = new List<Line>();
			lineColl.Add(oneLine);
			int position;
			bool result = VectorMathUI.CheckIfLineIntersectsWithLineCollection(oneLine, lineColl, out position);
			Assert.IsTrue(result);
			Assert.AreEqual(0, position);
		}

		/// <summary>
		/// collection with 5 lines, the 3rd intersects with the searched for line
		/// </summary>
		[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
		public void VectorMathUI_CheckIfLineIntersectsWithLineCollection_03_CollContainsLineAt3rdPosition()
		{
			Line oneLine = new Line();
			oneLine.X1 = 10;
			oneLine.Y1 = 10;
			oneLine.X2 = 20;
			oneLine.Y2 = 20;

			IList<Line> lineColl = new List<Line>();
			{
				Line line01 = new Line();
				line01.X1 = 100;
				line01.Y1 = 100;
				line01.X2 = 110;
				line01.Y2 = 110;
				lineColl.Add(line01);
			}

			{
				Line line02 = new Line();
				line02.X1 = 110;
				line02.Y1 = 110;
				line02.X2 = 120;
				line02.Y2 = 120;
				lineColl.Add(line02);
			}

			{
				// this line should cross oneLine
				Line line03 = new Line();
				line03.X1 = 10;
				line03.Y1 = 20;
				line03.X2 = 20;
				line03.Y2 = 10;
				lineColl.Add(line03);
			}

			{
				Line line04 = new Line();
				line04.X1 = 1100;
				line04.Y1 = 1100;
				line04.X2 = 1101;
				line04.Y2 = 1101;
				lineColl.Add(line04);
			}

			{
				Line line05 = new Line();
				line05.X1 = 2100;
				line05.Y1 = 2100;
				line05.X2 = 2101;
				line05.Y2 = 2101;
				lineColl.Add(line05);
			}

			int position;
			bool result = VectorMathUI.CheckIfLineIntersectsWithLineCollection(oneLine, lineColl, out position);
			Assert.IsTrue(result);
			Assert.AreEqual(2, position);
		}

		/// <summary>
		/// collection with 4 lines, no intersections
		/// </summary>
		[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
		public void VectorMathUI_CheckIfLineIntersectsWithLineCollection_04_CollDoesNotContainLine()
		{
			Line oneLine = new Line();
			oneLine.X1 = 10;
			oneLine.Y1 = 10;
			oneLine.X2 = 20;
			oneLine.Y2 = 20;

			IList<Line> lineColl = new List<Line>();
			{
				Line line01 = new Line();
				line01.X1 = 100;
				line01.Y1 = 100;
				line01.X2 = 110;
				line01.Y2 = 110;
				lineColl.Add(line01);
			}

			{
				Line line02 = new Line();
				line02.X1 = 110;
				line02.Y1 = 110;
				line02.X2 = 120;
				line02.Y2 = 120;
				lineColl.Add(line02);
			}

			{
				Line line04 = new Line();
				line04.X1 = 1100;
				line04.Y1 = 1100;
				line04.X2 = 1101;
				line04.Y2 = 1101;
				lineColl.Add(line04);
			}

			{
				Line line05 = new Line();
				line05.X1 = 2100;
				line05.Y1 = 2100;
				line05.X2 = 2101;
				line05.Y2 = 2101;
				lineColl.Add(line05);
			}

			int position;
			bool result = VectorMathUI.CheckIfLineIntersectsWithLineCollection(oneLine, lineColl, out position);
			Assert.IsFalse(result);
		}
	}
}
