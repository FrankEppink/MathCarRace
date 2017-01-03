using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Like VectorMath but based on UI Elements
	/// </summary>
	internal static class VectorMathUI
	{
		/// <summary>
		/// check if 'oneLine' intersects with any line in 'lineColl'
		/// </summary>
		/// <param name="oneLine">the line to check for intersections with lineColl</param>
		/// <param name="lineColl"></param>
		/// <param name="position">if there is an intersection, this is the index (zero-based) of the first line that intersects</param>
		/// <returns>true: an intersection was found, false: otherwise</returns>
		internal static bool CheckIfLineIntersectsWithLineCollection(Line oneLine, IList<Line> lineColl, out int position)
		{
			bool intersectionFound = false;
			position = -1;

			Point oneLine01 = new Point(oneLine.X1, oneLine.Y1);
			Point oneLine02 = new Point(oneLine.X2, oneLine.Y2);

			for(int index=0; (index < lineColl.Count) && (intersectionFound == false); index++)			
			{
				Line lineInColl = lineColl[index];
				Point lineColl01 = new Point(lineInColl.X1, lineInColl.Y1);
				Point lineColl02 = new Point(lineInColl.X2, lineInColl.Y2);

				if (VectorMath.CheckIf2LinesIntersect(oneLine01, oneLine02, lineColl01, lineColl02))
				{
					intersectionFound = true;
					position = index;
				}
			}

			return intersectionFound;
		}
	}
}
