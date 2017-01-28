using System.Threading;

namespace MathCarRaceUWP
{
	public enum ComputerDriverType
	{
		None,
		Careful,
		Risky,
		Top
	}

	interface IComputerDriverManager : IComputerDriver
	{
		void SelectType(ComputerDriverType compDriverType);

		void Start(TimerCallback callback);
		void Stop();
	}
}
