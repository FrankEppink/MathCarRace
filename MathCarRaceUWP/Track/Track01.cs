using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Track01 is a circle / ellipse
	/// </summary>
	internal class Track01 : TrackBase, ITrack
	{
		#region member

		private Ellipse outerEllipse;
		private Ellipse innerEllipse;

		#endregion member

		#region ITrack

		public void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate)
		{
			const uint outerTrackCanvasBorderOffset = 10;
			const uint innerTrackCanvasBorderOffset = 30;

			// Draw outer track
			outerEllipse = new Ellipse();
			// size
			outerEllipse.Width = width / 100 * (100 - outerTrackCanvasBorderOffset);
			outerEllipse.Height = height / 100 * (100 - outerTrackCanvasBorderOffset);
			// position
			Canvas.SetLeft(outerEllipse, width / 100 * outerTrackCanvasBorderOffset / 2);
			Canvas.SetTop(outerEllipse, height / 100 * outerTrackCanvasBorderOffset / 2);
			// other properties
			base.SetOuterBorderProperties(outerEllipse);
			children.Add(outerEllipse);

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
			innerEllipse = new Ellipse();
			// size			
			innerEllipse.Width = width / 100 * (100 - innerTrackCanvasBorderOffset);
			innerEllipse.Height = height / 100 * (100 - innerTrackCanvasBorderOffset);
			// position
			Canvas.SetLeft(innerEllipse, width / 100 * innerTrackCanvasBorderOffset / 2);
			Canvas.SetTop(innerEllipse, height / 100 * innerTrackCanvasBorderOffset / 2);
			// other properties
			base.SetInnerBorderProperties(innerEllipse);

			children.Add(innerEllipse);
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
			bool innerEllipseIncluded = Contains(innerEllipse, point);
			if (innerEllipseIncluded)
			{
				// point is in the inner ellipse so it is NOT on the track
				return false;
			}
			// if it is NOT in the inner ellipse then simply check if it is included in the outer ellipse
			return Contains(outerEllipse, point);
		}

		#endregion ITrack

		#region private methods

		/// <summary>
		/// http://stackoverflow.com/questions/13285007/how-to-determine-if-a-point-is-within-an-ellipse
		/// </summary>
		/// <param name="theEllipse"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		private static bool Contains(Ellipse theEllipse, Point location)
		{
			Point center = new Point(
				  Canvas.GetLeft(theEllipse) + (theEllipse.Width / 2),
				  Canvas.GetTop(theEllipse) + (theEllipse.Height / 2));

			double _xRadius = theEllipse.Width / 2;
			double _yRadius = theEllipse.Height / 2;
			
			if (_xRadius <= 0.0 || _yRadius <= 0.0)
				return false;
			/* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */

			Point normalized = new Point(location.X - center.X,
										 location.Y - center.Y);

			return ((double)(normalized.X * normalized.X)
					 / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
				<= 1.0;
		}

		#endregion private methods
	}
}
