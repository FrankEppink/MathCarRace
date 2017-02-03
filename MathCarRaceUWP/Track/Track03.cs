using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Track03 is a rectangle
	/// </summary>
	internal class Track03 : TrackBase, ITrack
	{
		#region member

		private Rectangle outerRectangle;
		private Rectangle innerRectangle;

		#endregion member

		#region ITrack

		public string GetTrackId()
		{
			return "Track03";
		}

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			const uint outerTrackCanvasBorderOffset = 10;
			const uint innerTrackCanvasBorderOffset = 30;

			// Draw outer track
			outerRectangle = new Rectangle();
			// size
			outerRectangle.Width = width / 100 * (100 - outerTrackCanvasBorderOffset);
			outerRectangle.Height = height / 100 * (100 - outerTrackCanvasBorderOffset);
			// position
			Canvas.SetLeft(outerRectangle, width / 100 * outerTrackCanvasBorderOffset / 2);
			Canvas.SetTop(outerRectangle, height / 100 * outerTrackCanvasBorderOffset / 2);
			// other properties
			base.SetOuterBorderProperties(outerRectangle);
			children.Add(outerRectangle);

			// Add starting line
			Point startingLineLeft = new Point
			{
				X = width / 100 * outerTrackCanvasBorderOffset / 2,
				Y = startingLineCoordinate
			};
			Point startingLineRight = new Point
			{
				X = width / 100 * innerTrackCanvasBorderOffset / 2,
				Y = startingLineCoordinate
			};
			Shape startingLine = base.CreateStartingLine(startingLineLeft, startingLineRight);
			children.Add(startingLine);

			// Draw inner track
			innerRectangle = new Rectangle();
			// size
			innerRectangle.Width = width / 100 * (100 - innerTrackCanvasBorderOffset);
			innerRectangle.Height = height / 100 * (100 - innerTrackCanvasBorderOffset);
			// position
			Canvas.SetLeft(innerRectangle, width / 100 * innerTrackCanvasBorderOffset / 2);
			Canvas.SetTop(innerRectangle, height / 100 * innerTrackCanvasBorderOffset / 2);
			// other properties
			base.SetInnerBorderProperties(innerRectangle);
			children.Add(innerRectangle);
		}

		public bool ValidateVector(IList<Point> routeGridPoints, Point gridPointClicked)
		{
			bool? baseValidationResult;
			BaseValidateVector(routeGridPoints, gridPointClicked, out baseValidationResult);
			if (baseValidationResult.HasValue)
			{
				// we are done, base class already made the decision
				return baseValidationResult.Value;
			}

			// further checks
			Point point = GridBackgroundHelper.ConvertGridPoint2Point(gridPointClicked);
			bool innerRectangleIncluded = Contains(innerRectangle, point);
			if (innerRectangleIncluded)
			{
				// point is in the inner rectangle so it is NOT on the track
				return false;
			}
			// if it is NOT in the inner rectangle then simply check if it is included in the outer rectangle
			return Contains(outerRectangle, point);
		}

		#endregion ITrack

		#region 

		private static bool Contains(Rectangle rectangle, Point location)
		{
			double left = Canvas.GetLeft(rectangle);
			double top = Canvas.GetTop(rectangle);
			double width = rectangle.Width;
			double height = rectangle.Height;

			if (location.X < left) { return false; };
			if (location.X > left + width) { return false; };

			if (location.Y < top) { return false; };
			if (location.Y > top + height) { return false; };

			return true;
		}

		#endregion
	}
}
