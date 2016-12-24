namespace MathCarRaceUWP
{
	/// <summary>
	/// The class that paints the different tracks on the grid	
	/// </summary>
	internal static class TrackProvider
	{
		internal static ITrack GetTrack(uint trackNumber)
		{
			ITrack iTrack = null;

			switch (trackNumber)
			{
				case 1:
					iTrack = new Track01();
					break;
				case 2:
					iTrack = new Track02();
					break;
				case 3:
					iTrack = new Track03();
					break;
				case 4:
					iTrack = new TrackPolygonRandom();
					break;
				case 5:
					iTrack = new Track05RealTrack();
					break;
				case 6:
					iTrack = new Track06RealTrack();
					break;
				default:
					break;
			}

			return iTrack;
		}		
	}
}
