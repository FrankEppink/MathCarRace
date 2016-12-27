using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCarRaceUWP
{
	internal interface ITrackLoader
	{
		ITrack LoadTrack(string filePath);
	}
}
