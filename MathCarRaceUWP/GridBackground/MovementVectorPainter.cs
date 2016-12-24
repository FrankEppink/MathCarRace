using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal static class MovementVectorPainter
	{
		#region consts

		/// <summary>
		/// the brush of the race movement vectors
		/// </summary>
		private static SolidColorBrush raceVectorBrush = new SolidColorBrush { Color = Windows.UI.Colors.Brown };

		private const double lengthOfArrowLines = 10;
		private const double angle1 = Math.PI / 8;
		private const double angle2 = -angle1;

		/// <summary>
		/// 3 UIElements = main line, arrow line angle1 and arrow line angle2 
		/// </summary>
		private const uint nrOfUIElements = 3;

		#endregion consts

		#region internal methods
				
		/// <summary>
		/// Paint the movement vector
		/// </summary>
		internal static IList<UIElement> PaintMovementVector(UIElementCollection children, Point gridPointLast, Point gridPointClicked)
		{
			IList<UIElement> uiElementsMovVector = new List<UIElement>();

			// Draw the main line
			Point pointLast = GridBackgroundHelper.ConvertGridPoint2Point(gridPointLast);
			Point pointClicked = GridBackgroundHelper.ConvertGridPoint2Point(gridPointClicked);
			uiElementsMovVector.Add(AddLine(children, pointLast, pointClicked));
			
			// make the line an arrow - angle1
			Point p1 = VectorMath.CalculateLineEndpointSatisfyingAngle(pointLast, pointClicked, lengthOfArrowLines, angle1);
			uiElementsMovVector.Add(AddLine(children, pointClicked, p1));

			// make the line an arrow - angle2
			Point p2 = VectorMath.CalculateLineEndpointSatisfyingAngle(pointLast, pointClicked, lengthOfArrowLines, angle2);
			uiElementsMovVector.Add(AddLine(children, pointClicked, p2));

			return uiElementsMovVector;
		}

		/// <summary>
		/// remove the last movement vector and return the number of UIElements actually removed
		/// </summary>
		/// <param name="children"></param>
		/// <returns></returns>
		internal static uint RemoveLastMovementVector(UIElementCollection children, IList<UIElement> movementVectorElements)
		{
			for(uint index=0; index < nrOfUIElements; index++)
			{
				// remove arrow line angle2, arrow line angle1 and the main line
				int movVecElIndex = (int) (movementVectorElements.Count - 1 - index);
				children.Remove(movementVectorElements[movVecElIndex]);
			}

			return nrOfUIElements;
		}

		#endregion internal methods

		#region private methods

		private static UIElement AddLine(UIElementCollection children, Point p1, Point p2)
		{
			// Draw the main line
			Line raceLine = new Line();
			raceLine.Stroke = raceVectorBrush;
			raceLine.X1 = p1.X;
			raceLine.Y1 = p1.Y;
			raceLine.X2 = p2.X;
			raceLine.Y2 = p2.Y;
			Canvas.SetZIndex(raceLine, ZIndexValues.raceLine);
			children.Add(raceLine);
			return raceLine;
		}

		#endregion private methods
	}
}
