﻿using System.Collections.Generic;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	/// <summary>
	/// ICar is the interface that represents the cars and their possibilities to move
	/// All cars have in common that they have a middle candidate grid point, see method 'GetMiddleCandidateGridPiont' below.
	/// 
	/// All methods only deal with the car movement and ignore track constraints
	/// </summary>
	interface ICar
	{
		/// <summary>
		/// Validate if the given movement vector is valid
		/// </summary>
		bool IsValid(IList<Point> routeGridPoints, Point gridPointClicked);
		
		/// <summary>
		/// Get the list of candidate grid points, i.e. the possible grid points for the next move of this car.
		/// Constraints of the track are not considered here.
		/// </summary>
		/// <returns></returns>
		IList<Point> GetCandidateGridPoints(IList<Point> routeGridPoints);

		/*
		/// <summary>
		/// Get the longest candidate grid point among the received candidate grid points
		/// longest grid point = longest movement vector
		/// only supported for at least two points in routeGridPoints
		/// </summary>
		/// <param name="routeGridPoints">the current route grid points</param>
		/// <param name="candidateGridPoints">the candidate grid points among which to search for the longest grid point</param>
		/// <returns></returns>
		IList<Point> GetLongestCandidateGridPoint(IList<Point> routeGridPoints, IList<Point> candidateGridPoints);
		*/
	}
}
