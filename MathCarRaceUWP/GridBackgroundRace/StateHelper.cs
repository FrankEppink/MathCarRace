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

		internal const string startString = "Choose Position On Starting Line";
		internal const string drivingString = "GoGoGo";
		internal const string computerInDriversSeatString = "Computer Is Driving";
		internal const string endString = "Race Is Finished!";

		#endregion constants
	}
}
