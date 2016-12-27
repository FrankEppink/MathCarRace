using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	internal class TrackLoader : ITrackLoader
	{
		public ITrack LoadTrack(string filePath)
		{
			// load and parse the file
			// TODO

			// fill into LoadedTrack
			LoadedTrack lp = new LoadedTrack();

			lp.widthLeftStartingPoint = 10;
			lp.widthRightStartingPoint = 40;

			lp.outerPoints = new List<Point>();
			lp.outerPoints.Add(new Point(10, 10));
			lp.outerPoints.Add(new Point(90, 10));
			lp.outerPoints.Add(new Point(90, 90));
			lp.outerPoints.Add(new Point(10, 90));

			lp.innerPoints = new List<Point>();
			lp.innerPoints.Add(new Point(40, 40));
			lp.innerPoints.Add(new Point(60, 40));
			lp.innerPoints.Add(new Point(60, 60));
			lp.innerPoints.Add(new Point(40, 60));

			return lp;
		}
	}
}
