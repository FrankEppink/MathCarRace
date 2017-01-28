namespace MathCarRaceUWP
{
	internal static class StateHelper
	{
		internal enum RaceState
		{
			Manual,
			ComputerDriver,
			Finished
		};

		#region constants

		internal const string startRaceString = "Choose Position On Starting Line";
		internal const string endRaceString = "Race Is Finished!";
		internal const string humanInDriversSeatString = "GoGoGo";
		internal const string computerInDriversSeatString = "Computer Is Driving";

		#endregion constants
	}
}
