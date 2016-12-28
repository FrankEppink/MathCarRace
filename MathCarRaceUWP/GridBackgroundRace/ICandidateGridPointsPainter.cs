using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace MathCarRaceUWP
{
	interface ICandidateGridPointsPainter
	{
		/// <summary>
		/// mark the candidate grid points so that the user knows which grid points he can now choose
		/// </summary>
		/// <param name="candidateGridPoints"></param>
		void MarkCandidateGridPoints(UIElementCollection elemColl, IList<Point> candidateGridPoints);


		/// <summary>
		/// Remove the current candidate grid points
		/// </summary>
		void RemoveCurrentCandidateGridPoints(UIElementCollection elemColl);		
	}
}
