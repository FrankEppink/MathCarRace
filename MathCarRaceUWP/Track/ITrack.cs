using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Track interface
	/// </summary>
	internal interface ITrack
	{
		/// <summary>
		/// Get the Id of this track
		/// Used internally for saving the personal highscore
		/// </summary>
		/// <returns></returns>
		string GetTrackId();

		/// <summary>
		/// paint the track
		/// </summary>
		/// <param name="children">the object where to add the UI elements of the track</param>
		/// <param name="width">the width of the canvas</param>
		/// <param name="height">the height of the canvas</param>
		/// <param name="startingLineCoordinate">the y coordinate of the starting line</param>
		void PaintTrack(UIElementCollection children, double width, double height, uint startingLineCoordinate);

		/// <summary>
		/// Validate if the given movement vector is on the track
		/// </summary>
		/// <param name="routeGridPoints">the previous route grid points</param>
		/// <param name="gridPointClicked">the new grid point</param>
		/// <returns></returns>
		bool ValidateVector(IList<Point> routeGridPoints, Point gridPointClicked);

		/// <summary>
		/// check if the new grid point has crossed the finish line
		/// the new grid point is either on the finish line or it has crossed it.
		/// it is also verified if the movement comes from the right direction, i.e. from below
		/// </summary>
		bool CheckIfRaceIsFinished(IList<Point> routeGridPoints, Point gridPointClicked);

		/// <summary>
		/// Get the list of starting line points
		/// </summary>
		/// <returns></returns>
		IList<Point> GetStartingLinePoints();
	}
}
