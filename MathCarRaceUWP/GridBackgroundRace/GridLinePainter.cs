using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal static class GridLinePainter
	{
		/// <summary>
		/// paint the grid lines
		/// </summary>
		/// <returns></returns>
		internal static void PaintGridLines(Canvas xMyCanvas, uint nrRows)
		{
			double cWidth = xMyCanvas.Width;
			double cHeight = xMyCanvas.Height;

			uint nrCols = (uint)(cWidth / GridBackgroundHelper.gridDistance);

			// vertical lines
			for (uint colIndex = 0; colIndex <= nrCols; colIndex++)
			{
				Line myLine = CreateGridLine();

				myLine.X1 = colIndex * GridBackgroundHelper.gridDistance;
				myLine.X2 = colIndex * GridBackgroundHelper.gridDistance;
				myLine.Y1 = 0;
				myLine.Y2 = cHeight;

				xMyCanvas.Children.Add(myLine);
			}

			// horizontal lines
			for (uint rowIndex = 0; rowIndex <= nrRows; rowIndex++)
			{
				Line myLine = CreateGridLine();

				myLine.X1 = 0;
				myLine.X2 = cWidth;
				myLine.Y1 = rowIndex * GridBackgroundHelper.gridDistance;
				myLine.Y2 = rowIndex * GridBackgroundHelper.gridDistance;

				xMyCanvas.Children.Add(myLine);
			}
		}

		/// <summary>
		/// Create a grid line and set all common properties
		/// </summary>
		/// <returns></returns>
		private static Line CreateGridLine()
		{
			Line myLine = new Line();

			myLine.Stroke = GridBrushDefs.gridLinesBrush;
			myLine.StrokeThickness = 1;
			myLine.Visibility = Visibility.Visible;

			Canvas.SetZIndex(myLine, ZIndexValues.grid);

			return myLine;
		}

		/// <summary>
		/// Get number of grid rows in the canvas
		/// </summary>
		/// <param name="xMyCanvas"></param>
		/// <returns></returns>
		internal static uint GetNrGridRows(Canvas xMyCanvas)
		{
			return (uint)(xMyCanvas.Height / GridBackgroundHelper.gridDistance);
		}

		internal static uint GetMiddleGridPointY(Canvas xMyCanvas)
		{
			return (GetNrGridRows(xMyCanvas) / 2);
		}

		/// <summary>
		/// return the middle grid row y coordinate
		/// </summary>
		/// <returns></returns>
		internal static uint GetMiddleGridRowYCoordinate(Canvas xMyCanvas)
		{
			return (uint)((GetNrGridRows(xMyCanvas) / 2) * GridBackgroundHelper.gridDistance);
		}

		internal static uint GetMiddleGridPointX(Canvas xMyCanvas)
		{
			return (GetNrGridCols(xMyCanvas) / 2);
		}

		private static uint GetNrGridCols(Canvas xMyCanvas)
		{
			return (uint)(xMyCanvas.Width / GridBackgroundHelper.gridDistance);
		}
	}
}
