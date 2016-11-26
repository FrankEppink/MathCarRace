using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	internal class CandidateGridPointsPainter : ICandidateGridPointsPainter
	{
		#region consts

		/// <summary>
		/// the brush of the candidate grid points
		/// </summary>
		private static SolidColorBrush cgpBrush = new SolidColorBrush { Color = Windows.UI.Colors.AliceBlue };

		private const double pointDiff = 5;

		#endregion consts

		#region members

		/// <summary>
		/// the current candidate grid points markings
		/// they are stored, so that they can be removed after the next movement step done
		/// </summary>
		IList<UIElement> mCandidateGridPointMarkings = new List<UIElement>();

		#endregion members

		#region ICandidateGridPointsPainter

		/// <summary>
		/// mark the candidate grid points so that the user knows which grid points he can now choose
		/// </summary>
		/// <param name="candidateGridPoints"></param>
		public void MarkCandidateGridPoints(UIElementCollection elemColl, IList<Point> candidateGridPoints)
		{
			mCandidateGridPointMarkings = CreateMarkings(candidateGridPoints);
			for (int index = 0; index < mCandidateGridPointMarkings.Count; index++)
			{
				elemColl.Add(mCandidateGridPointMarkings[index]);
			}
		}

		/// <summary>
		/// Remove the current candidate grid points
		/// </summary>
		public void RemoveCurrentCandidateGridPoints(UIElementCollection elemColl)
		{
			for (int index = 0; index < mCandidateGridPointMarkings.Count; index++)
			{
				elemColl.Remove(mCandidateGridPointMarkings[index]);
			}

			mCandidateGridPointMarkings.Clear();
		}

		#endregion ICandidateGridPointsPainter

		#region private methods

		/// <summary>
		/// Create markings for the candidate grid points, i.e. the grid points that are candidates for being the next target
		/// i.e. end of the next movement vector
		/// </summary>
		private static IList<UIElement> CreateMarkings(IList<Point> candidateGridPoints)
		{
			IList<UIElement> candidateGridPointMarkings = new List<UIElement>();
			for (int index = 0; index < candidateGridPoints.Count; index++)
			{
				IList<UIElement> cgpMarkings = PaintCGP(candidateGridPoints[index]);
				for (int index2 = 0; index2 < cgpMarkings.Count; index2++)
				{
					candidateGridPointMarkings.Add(cgpMarkings[index2]);
				}
			}

			return candidateGridPointMarkings;
		}

		/// <summary>
		/// Paint the candidate grid points, i.e. the grid points that are candidates for being the next target
		/// i.e. end of the next movement vector
		/// </summary>
		private static IList<UIElement> PaintCGP(Point candidateGridPoint)
		{
			IList<UIElement> uiElements = new List<UIElement>();

			Point point = GridBackgroundHelper.ConvertGridPoint2Point(candidateGridPoint);
			
			// Draw the first line - left bottom to right top
			Point pointLeftBottom = new Point(point.X - pointDiff, point.Y + pointDiff);
			Point pointRightTop = new Point(point.X + pointDiff, point.Y - pointDiff);
			uiElements.Add(CreateLine(pointLeftBottom, pointRightTop));
			
			// Draw the second line - left top to right bottom
			Point pointLeftTop = new Point(point.X - pointDiff, point.Y - pointDiff);
			Point pointRightBottom = new Point(point.X + pointDiff, point.Y + pointDiff);
			uiElements.Add(CreateLine(pointLeftTop, pointRightBottom));

			return uiElements;
		}

		private static UIElement CreateLine(Point p1, Point p2)
		{
			// Draw the main line
			Line cgpLine = new Line();
			cgpLine.Stroke = cgpBrush;
			cgpLine.X1 = p1.X;
			cgpLine.Y1 = p1.Y;
			cgpLine.X2 = p2.X;
			cgpLine.Y2 = p2.Y;
			Canvas.SetZIndex(cgpLine, ZIndexValues.candidateGridPointLine);
			return cgpLine;
		}

		#endregion private methods
	}
}
