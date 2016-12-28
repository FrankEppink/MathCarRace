using System;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace MathCarRaceUWP
{
	/// <summary>
	/// This class does conversion between points and grid points
	/// </summary>
	internal static class GridBackgroundHelper
	{
		#region constants

		internal const double gridDistance = 20.0;
		internal const double nearestGridPointDistanceTolerance = 4.0;

		#endregion constants

		/// <summary>
		/// Convert the clicked position (PointerPoint) into the nearest grid piont
		/// </summary>
		internal static Point ConvertClickedPoint2NearestGridPoint_WithOffset(UIElement uiElementCanvas, UIElement uiElementStackPanel, PointerPoint pointClicked)
		{
			Point canvasOffset = GetCanvasOffset(uiElementCanvas, uiElementStackPanel);

			double xConverted = (pointClicked.Position.X - canvasOffset.X) / gridDistance;
			double yConverted = (pointClicked.Position.Y - canvasOffset.Y) / gridDistance;

			// get the nearest grid point
			Point gridPointClicked;
			gridPointClicked.X = (uint)Math.Round(xConverted);
			gridPointClicked.Y = (uint)Math.Round(yConverted);
			return gridPointClicked;
		}

		/// <summary>
		/// Convert the given point into the nearest grid piont
		/// </summary>
		internal static Point ConvertPoint2NearestGridPoint_NoOffset(Point point2Convert)
		{
			double xConverted = point2Convert.X / gridDistance;
			double yConverted = point2Convert.Y / gridDistance;

			// get the nearest grid point
			Point gridPointClicked;
			gridPointClicked.X = (uint)Math.Round(xConverted);
			gridPointClicked.Y = (uint)Math.Round(yConverted);
			return gridPointClicked;
		}

		/// <summary>
		/// Convert the given grid point into the corresponding point
		/// </summary>
		internal static Point ConvertGridPoint2Point(Point gridPoint)
		{
			Point point = new Point(gridPoint.X * gridDistance, gridPoint.Y * gridDistance);
			return point;
		}

		/// <summary>
		/// Get the offset of the canvas element in its parent element (StackPanel)
		/// </summary>		
		internal static Point GetCanvasOffset(UIElement uiElementCanvas, UIElement uiElementStackPanel)
		{
			// https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.uielement.transformtovisual.aspx
			var canvas2StackPanel = uiElementCanvas.TransformToVisual(uiElementStackPanel);
			return canvas2StackPanel.TransformPoint(new Point(0, 0));
		}
	}
}
