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

		internal const string startString = "Choose your position on the starting line";
		internal const string drivingString = "GoGoGo";
		internal const string computerInDriversSeatString = "Computer is driving";
		internal const string endString = "Race is finished!";

		#endregion constants
	}
}
