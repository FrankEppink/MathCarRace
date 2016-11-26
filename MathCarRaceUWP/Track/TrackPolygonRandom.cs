using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class TrackPolygonRandom : TrackPolygon, ITrack
	{
		#region constants

		private const int leftOffsetStartingLine = 10;
		private const int minWidthStartingLine = 10;
		private const int maxWidthStartingLine = 30;
		private const int minStep = 10;  // the mininum step in one direction for one generated line
		private const int maxStep = 50; // the maximum step in one direction for one generated line

		private const int minPercentageLow = 1;
		private const int maxPercentageLow = 31;
		private const int minPercentageHigh = 5;
		private const int maxPercentageHigh = 45;

		#endregion constants

		#region ITrack

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			// get width of starting line
			Random random = new Random();
			double widthOfStartingLine = random.Next(minWidthStartingLine, maxWidthStartingLine);

			// Add starting line
			Point startingLineLeft = new Point
			{
				X = width / 100 * leftOffsetStartingLine,
				Y = startingLineCoordinate
			};
			Point startingLineRight = new Point
			{
				X = width / 100 * (leftOffsetStartingLine + widthOfStartingLine),
				Y = startingLineCoordinate
			};
			Shape startingLine = base.CreateStartingLine(startingLineLeft, startingLineRight);
			children.Add(startingLine);

			// needed for checking if a quadrant has been left
			double middleX = width / 100 * 50;
			double middleY = startingLineCoordinate;

			// Draw outer track
			// first point on starting line
			outerPolygon = GetPolygon(startingLineLeft, middleX, middleY);
			base.SetOuterBorderProperties(outerPolygon);
			children.Add(outerPolygon);

			// Draw inner track
			// first point on starting line
			innerPolygon = GetPolygon(startingLineRight, middleX, middleY);
			base.SetInnerBorderProperties(innerPolygon);
			children.Add(innerPolygon);
		}

		#endregion ITrack

		#region private methods

		private static Polygon GetPolygon(Point firstPoint, double middleX, double middleY)
		{
			// currentPoint.X is always the offset from the outer canvas border
			double canvasOuterBorderOffset = firstPoint.X;

			Polygon myPolygon = new Polygon();
			// first point on starting line
			Point currentPoint = firstPoint;
			myPolygon.Points.Add(currentPoint);

			// how many points do we have available on x and on y until the next quadrant is reached
			double xAvailable;
			double yAvailable;

			// percentage values that are the interval for the desired movements
			int minXPercentage;
			int maxXPercentage;
			int minYPercentage;
			int maxYPercentage;

			// ************************//
			// first left top quadrant //
			// ************************//
			// how many points do we have available on x and on y until the next quadrant is reached
			xAvailable = middleX - currentPoint.X;
			yAvailable = currentPoint.Y - canvasOuterBorderOffset;
			int counter = 0;

			// start with percentage values that represent the primary movements we want to have
			// low x percentages -> low x movements at the start of this quadrant
			minXPercentage = minPercentageLow;
			maxXPercentage = maxPercentageLow;
			minYPercentage = minPercentageHigh;
			maxYPercentage = maxPercentageHigh;
			
			while (currentPoint.X < middleX)
			{
				Random random = new Random(counter * DateTime.Now.Millisecond);
				// get random percentage values that state how much of x and y we are moving
				double xPercentage = random.Next(minXPercentage, maxXPercentage);
				double yPercentage = random.Next(minYPercentage, maxYPercentage);

				double xDiff = (xPercentage / 100) * xAvailable;
				double yDiff = (yPercentage / 100) * yAvailable;
				// without the following correction factor we would infinitely approach the value middleX
				// because the available values get really small and we only take a percentage from them
				xDiff += 1.0;
				currentPoint.X = currentPoint.X + xDiff;
				currentPoint.Y = currentPoint.Y - yDiff;
				myPolygon.Points.Add(currentPoint);
				xAvailable -= xDiff;
				yAvailable -= yDiff;
				// draw the curve, the longer we draw, the more movement on x-axis, the less on the y-axis
				if (minXPercentage + 1 < 100) {	minXPercentage++; }
				if (maxXPercentage + 1 < 100) { maxXPercentage++; }
				if (minYPercentage - 1 > 0) { minYPercentage--; }
				if (maxYPercentage - 1 > 0) { maxYPercentage--; }
				counter++;
			}

			// **************************//
			// second right top quadrant //
			// **************************//
			xAvailable = ((middleX * 2) - canvasOuterBorderOffset) - currentPoint.X;
			yAvailable = middleY - currentPoint.Y;
			counter = 0;

			// high x percentages -> high x movements at the start of this quadrant
			minXPercentage = minPercentageHigh;
			maxXPercentage = maxPercentageHigh;
			minYPercentage = minPercentageLow;
			maxYPercentage = maxPercentageLow;

			while (currentPoint.Y < middleY)
			{
				Random random = new Random(counter * DateTime.Now.Millisecond);
				// get random percentage values that state how much of x and y we are moving
				double xPercentage = random.Next(minXPercentage, maxXPercentage);
				double yPercentage = random.Next(minYPercentage, maxYPercentage);

				double xDiff = (xPercentage / 100) * xAvailable;
				double yDiff = (yPercentage / 100) * yAvailable;
				// without the following correction factor we would infinitely approach the value middleY
				// because the available values get really small and we only take a percentage from them
				yDiff += 1.0;
				currentPoint.X = currentPoint.X + xDiff;
				currentPoint.Y = currentPoint.Y + yDiff;
				myPolygon.Points.Add(currentPoint);
				xAvailable -= xDiff;
				yAvailable -= yDiff;
				// draw the curve, the longer we draw, the more movement on y-axis, the less on the x-axis
				if (minXPercentage - 1 > 0) { minXPercentage--; }
				if (maxXPercentage - 1 > 0) { maxXPercentage--; }
				if (minYPercentage + 1 > 0) { minYPercentage++; }
				if (maxYPercentage + 1 > 0) { maxYPercentage++; }
				counter++;
			}

			// ****************************//
			// third right bottom quadrant //
			// ****************************//
			xAvailable = currentPoint.X - middleX;
			double yLimit = (middleY * 2) - canvasOuterBorderOffset;
			yAvailable = yLimit - currentPoint.Y;
			counter = 0;

			// low x percentages -> low x movements at the start of this quadrant
			minXPercentage = minPercentageLow;
			maxXPercentage = maxPercentageLow;
			minYPercentage = minPercentageHigh;
			maxYPercentage = maxPercentageHigh;

			while (currentPoint.X > middleX)
			{
				Random random = new Random(counter * DateTime.Now.Millisecond);
				// get random percentage values that state how much of x and y we are moving
				double xPercentage = random.Next(minXPercentage, maxXPercentage);
				double yPercentage = random.Next(minYPercentage, maxYPercentage);

				double xDiff = (xPercentage / 100) * xAvailable;
				double yDiff = (yPercentage / 100) * yAvailable;
				// without the following correction factor we would infinitely approach the value middleX
				// because the available values get really small and we only take a percentage from them
				xDiff += 1.0;
				currentPoint.X = currentPoint.X - xDiff;
				currentPoint.Y = currentPoint.Y + yDiff;
				myPolygon.Points.Add(currentPoint);
				xAvailable -= xDiff;
				yAvailable -= yDiff;
				// draw the curve, the longer we draw, the more movement on x-axis, the less on the y-axis
				if (minXPercentage + 1 < 100) { minXPercentage++; }
				if (maxXPercentage + 1 < 100) { maxXPercentage++; }
				if (minYPercentage - 1 > 0) { minYPercentage--; }
				if (maxYPercentage - 1 > 0) { maxYPercentage--; }
				counter++;
			}

			// ****************************//
			// fourth left bottom quadrant //
			// ****************************//
			xAvailable = currentPoint.X - canvasOuterBorderOffset;
			yAvailable = currentPoint.Y - middleY;
			counter = 0;

			// high x percentages -> high x movements at the start of this quadrant
			minXPercentage = minPercentageHigh;
			maxXPercentage = maxPercentageHigh;
			minYPercentage = minPercentageLow;
			maxYPercentage = maxPercentageLow;

			while (currentPoint.Y > middleY)
			{
				Random random = new Random(counter * DateTime.Now.Millisecond);
				// get random percentage values that state how much of x and y we are moving
				double xPercentage = random.Next(minXPercentage, maxXPercentage);
				double yPercentage = random.Next(minYPercentage, maxYPercentage);

				double xDiff = (xPercentage / 100) * xAvailable;
				double yDiff = (yPercentage / 100) * yAvailable;
				// without the following correction factor we would infinitely approach the value middleY
				// because the available values get really small and we only take a percentage from them
				yDiff += 1.0;
				currentPoint.X = currentPoint.X - xDiff;
				currentPoint.Y = currentPoint.Y - yDiff;
				myPolygon.Points.Add(currentPoint);
				xAvailable -= xDiff;
				yAvailable -= yDiff;
				// draw the curve, the longer we draw, the more movement on y-axis, the less on the x-axis
				if (minXPercentage - 1 > 0) { minXPercentage--; }
				if (maxXPercentage - 1 > 0) { maxXPercentage--; }
				if (minYPercentage + 1 > 0) { minYPercentage++; }
				if (maxYPercentage + 1 > 0) { maxYPercentage++; }
				counter++;
			}

			// special case stop one earlier, so that the starting line is not crossed
			// -> remove the last point
			myPolygon.Points.RemoveAt(myPolygon.Points.Count - 1);
			
			return myPolygon;
		}	

		#endregion private methods
	}
}
