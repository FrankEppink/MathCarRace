using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class TrackBase
	{
		#region members

		/// <summary>
		/// the left point of the starting line
		/// </summary>
		private Point startingLineGridPointLeft;

		/// <summary>
		/// the right point of the starting line
		/// </summary>
		private Point startingLineGridPointRight;
		
		#endregion members

		#region ITrack

		/// <summary>
		/// ITrack Interface method
		/// </summary>
		/// <returns>null if this method cannot decide</returns>
		public void BaseValidateVector(IList<Point> routeGridPoints, Point gridPointClicked, out bool? validationResult)
		{
			// set default return value
			validationResult = null;
			if (routeGridPoints.Count == 0)
			{
				// race is just starting, only check if the first point is on starting line
				validationResult = IsGridPointOnStartingLine(gridPointClicked);				
			}
			else
			{
				// it is not allowed to pass the starting line from the wrong direction
				bool rightDirection;
				bool crossed = CheckIfStartingLineIsCrossed(routeGridPoints, gridPointClicked, out rightDirection);
				if (crossed && !rightDirection)
				{
					validationResult = false;
				}
			}
		}

		/// <summary>
		/// ITrack Interface method
		/// </summary>
		public bool CheckIfRaceIsFinished(IList<Point> routeGridPoints, Point gridPointClicked)
		{
			bool rightDirection;
			bool crossed = CheckIfStartingLineIsCrossed(routeGridPoints, gridPointClicked, out rightDirection);

			return crossed && rightDirection;
		}

		/// <summary>
		/// ITrack Interface method
		/// </summary>
		public IList<Point> GetStartingLinePoints()
		{
			IList<Point> result = new List<Point>();

			result.Add(startingLineGridPointLeft);

			double xDiff = startingLineGridPointRight.X - startingLineGridPointLeft.X;
			double xIndex = startingLineGridPointLeft.X + 1;
			while (xIndex < startingLineGridPointRight.X)
			{
				Point stlgPoint = new Point(xIndex, startingLineGridPointLeft.Y);
				result.Add(stlgPoint);
				xIndex++;
			}

			result.Add(startingLineGridPointRight);

			return result;
		}


		#endregion ITrack

		#region protected methods

		/// <summary>
		/// Set the common properties of the outerBorder
		/// </summary>
		/// <param name="outerBorder"></param>
		protected void SetOuterBorderProperties(Shape outerBorder)
		{
			outerBorder.Stroke = TrackBrushDefs.trackBorderBrush;
			outerBorder.Fill = TrackBrushDefs.trackBrush;
			Canvas.SetZIndex(outerBorder, ZIndexValues.outerBorder);
		}

		/// <summary>
		/// Set the common properties of the innerBorder
		/// </summary>
		/// <param name="innerBorder"></param>
		protected void SetInnerBorderProperties(Shape innerBorder)
		{
			// TODO this is not working...
			// https://msdn.microsoft.com/en-us/library/hh763341.aspx
			// ImageBrush innerPartBrush = new ImageBrush();
			// innerPartBrush.ImageSource = new BitmapImage(new Uri(@"Images\meadow.jpg", UriKind.Relative));
			// innerPartBrush.ImageSource = new BitmapImage(new Uri("ms-appx-web:///Images/meadow.jpg", UriKind.Absolute));

			innerBorder.Stroke = TrackBrushDefs.trackBorderBrush;
			innerBorder.Fill = TrackBrushDefs.innerPartBrush;  // overwrite the brush of the outer track
			
			Canvas.SetZIndex(innerBorder, ZIndexValues.innerBorder);
		}

		/// <summary>
		/// Create a starting line
		/// </summary>
		/// <param name="startingLineLeft"></param>
		/// <param name="startingLineRight"></param>
		/// <returns></returns>
		protected Shape CreateStartingLine(Point startingLineLeft, Point startingLineRight)
		{
			if (startingLineLeft.Y != startingLineRight.Y)
			{
				throw new ArgumentException("starting line must be horizontal but is not");
			}

			Line startingLine = new Line();
			startingLine.Stroke = TrackBrushDefs.startingLineBrush;
			
			startingLine.X1 = startingLineLeft.X;
			startingLine.Y1 = startingLineLeft.Y;
			
			startingLine.X2 = startingLineRight.X;
			startingLine.Y2 = startingLineRight.Y;			

			// Must Call, so that the starting line is memorized as grid points for later validation
			SetStartingLineGridPoints(startingLineLeft, startingLineRight);

			Canvas.SetZIndex(startingLine, ZIndexValues.startingLine);

			return startingLine;
		}

		#endregion protected methods

		#region private methods

		/// <summary>
		/// Check if a grid point is on the starting line
		/// </summary>
		/// <param name="gridPoint">the (clicked) grid point</param>
		/// <returns></returns>
		private bool IsGridPointOnStartingLine(Point gridPoint)
		{
			if ((startingLineGridPointLeft == null) || (startingLineGridPointRight == null))
			{
				throw new InvalidOperationException("no starting line is known");
			}

			if (gridPoint.Y != startingLineGridPointLeft.Y) return false;	// different y coordinate
			if (gridPoint.X < startingLineGridPointLeft.X) return false; // left of starting line
			if (gridPoint.X > startingLineGridPointRight.X) return false; // right of starting line

			return true;	// on the starting line
		}

		/// <summary>
		/// Set the starting line grid points
		/// so that we can detect if the first click is on a starting line grid point 
		/// 
		/// A starting line is always horizontal
		/// </summary>
		private void SetStartingLineGridPoints(Point startingLineLeftPoint, Point startingLineRightPoint)
		{
			startingLineGridPointLeft = GridBackgroundHelper.ConvertPoint2NearestGridPoint_NoOffset(startingLineLeftPoint);
			startingLineGridPointRight = GridBackgroundHelper.ConvertPoint2NearestGridPoint_NoOffset(startingLineRightPoint);

			if (startingLineGridPointLeft.Y != startingLineGridPointRight.Y)
			{
				throw new ArgumentException("starting line is not horizontal, left and right Y coordinate differ");
			}

			if (startingLineGridPointLeft.X > startingLineGridPointRight.X)
			{
				throw new ArgumentException("left and right points of starting line are mixed");
			}
		}
				
		/// <summary>
		/// check if the starting line is crossed and return the direction
		/// </summary>
		private bool CheckIfStartingLineIsCrossed(IList<Point> routeGridPoints, Point gridPointClicked, out bool rightDirection)
		{
			if (routeGridPoints.Count < 2)
			{
				rightDirection = false;
				// too few
				return false;
			}

			// determine if we are going in the right direction for the starting line
			// right direction is from down to up, i.e. from a higher Y-coordinate to a lower
			rightDirection = routeGridPoints.Last().Y > gridPointClicked.Y;

			// does the clicked grid point actually lie on the starting line
			if (IsGridPointOnStartingLine(gridPointClicked))
			{
				// yes it does, return true, no need to check for an intersection
				return true;
			}

			// for the intersection check we need to extend the starting line beyond the grid points
			// as the starting line also includes the part which is left of the left grid point
			// and which is right of the right grid point.
			Point extendedLeft = new Point(startingLineGridPointLeft.X, startingLineGridPointLeft.Y);
			if (extendedLeft.X > 0) { extendedLeft.X -= 1; };
			Point extendedRight = new Point(startingLineGridPointRight.X + 1, startingLineGridPointRight.Y);

			// now check if the car has crossed has the finish line
			return VectorMath.CheckIf2LinesIntersect(extendedLeft, extendedRight, routeGridPoints.Last(), gridPointClicked);
		}

		#endregion private methods
	}
}
