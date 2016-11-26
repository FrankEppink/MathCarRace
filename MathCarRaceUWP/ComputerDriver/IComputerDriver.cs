using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	interface IComputerDriver
	{
		/// <summary>
		/// Get the next grid point
		/// </summary>
		/// <param name="track">current track</param>
		/// <param name="car">current car</param>
		/// <param name="routeGridPoints">not modified in this method</param>
		/// <param name="middleGridPoint">the middle grid point</param>
		/// <param name="nrBacktracks">number of moves to be backtracked, i.e. undone</param>
		/// <param name="gridPoint">the next selected grid point</param>
		void GetNextGridPoint(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint, out uint nrBacktracks, out Point gridPoint);
	}
}
