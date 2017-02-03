using Windows.Storage;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Thie class save and returns the highscores for given trackNames
	/// 
	/// The local settings can be found at
	/// c:\users\{username}\AppData\Local\Packages\{packageID}
	/// Example
	/// C:\Users\frank_000\AppData\Local\Packages\31735FrankEppink.MathCarRace_8tshadn8xt1wc\Settings
	/// </summary>
	internal static class Highscores
	{
		internal static void SaveHighscore(string trackName, uint nrOfVectors)
		{
			ApplicationData.Current.LocalSettings.Values[trackName] = nrOfVectors;
		}

		internal static uint? GetHighscore(string trackName)
		{
			object theValue;
			bool result = ApplicationData.Current.LocalSettings.Values.TryGetValue(trackName, out theValue);
			if (result)
			{
				return (uint) theValue;
			}
			else
			{
				return null;
			}
		}
	}
}
