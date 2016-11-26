using System.Collections.Generic;
using System.Threading;
using Windows.Foundation;

namespace MathCarRaceUWP
{
	internal class ComputerDriverManager : IComputerDriverManager
	{
		#region members

		/// <summary>
		/// the computer driver with the logic of calculating the next movements
		/// </summary>
		private IComputerDriver mComputerDriver = new ComputerDriverCareful();

		/// <summary>
		/// the timer for triggering the movements
		/// </summary>
		private Timer mComputerDriverTimer;

		/// <summary>
		/// time between the movements
		/// </summary>
		private const int computerDriverWaitTimeInMs = 200;

		#endregion members

		#region IComputerDriverManager

		public void SelectType(ComputerDriverType compDriverType)
		{
			if (compDriverType == ComputerDriverType.Careful)
			{
				mComputerDriver = new ComputerDriverCareful();
			}
			else if (compDriverType == ComputerDriverType.Risky)
			{
				mComputerDriver = new ComputerDriverRisky();
			}
			else
			{
				mComputerDriver = new ComputerDriverTop();
			}
		}

		public void Start(TimerCallback callback)
		{
			mComputerDriverTimer = new Timer(callback, SynchronizationContext.Current, 0, computerDriverWaitTimeInMs);
		}

		public void Stop()
		{
			if (mComputerDriverTimer != null)
			{
				mComputerDriverTimer.Dispose();
				mComputerDriverTimer = null;
			}
		}

		#endregion IComputerDriverManager

		#region IComputerDriver

		public void GetNextGridPoint(ITrack track, ICar car, IList<Point> routeGridPoints, Point middleGridPoint, out uint nrBacktracks, out Point gridPoint)
		{
			mComputerDriver.GetNextGridPoint(track, car, routeGridPoints, middleGridPoint, out nrBacktracks, out gridPoint);
		}

		#endregion IComputerDriver
	}
}
