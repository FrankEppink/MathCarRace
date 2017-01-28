using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace MathCarRaceUWP
{
	/// <summary>
	/// GridBackground
	/// Manages all stuff related to the grid and it's grid points, e.g.
	/// painting the grid, converting click coordinates into grid points and vice versa
	/// </summary>
	public sealed partial class GridBackground : Page
	{
		#region constants

		public const int LOAD_TRACK_NR = 9999;

		#endregion constants
		
		#region members

		#region race state members

		/// <summary>
		/// The grid points of the current route, empty if the track is fresh and the race has not started yet
		/// </summary>
		private IList<Point> mRouteGridPoints = new List<Point>();

		/// <summary>
		/// The uiElements that represent/paint the current route
		/// </summary>
		private IList<UIElement> mRouteElements = new List<UIElement>();

		/// <summary>
		/// enum for current state of the race
		/// </summary>
		private StateHelper.RaceState mRaceState = StateHelper.RaceState.Manual;

		#endregion race state members

		#region helper interface objects

		/// <summary>
		/// The class which takes care of painting and removing the candidate grid points
		/// </summary>
		private ICandidateGridPointsPainter cgpp = new CandidateGridPointsPainter();

		/// <summary>
		/// The object that handles the specific track
		/// </summary>
		private ITrack mActiveTrack = null;

		/// <summary>
		/// The object that represents the active car
		/// </summary>
		private ICar mActiveCar = null;

		/// <summary>
		/// the computer driver manager, which includes the computer driver (IComputerDriver)
		/// </summary>
		private IComputerDriverManager mComputerDriverManager = new ComputerDriverManager();

		#endregion track and car objects

		#endregion members

		#region properties

		private uint GetNrOfVectors()
		{
			if (mRouteGridPoints.Count == 0)
			{
				return 0;
			}

			// the first grid point is on the starting line and needs to be subtracted
			return (uint)(mRouteGridPoints.Count - 1);
		}

		private void SetStatusField(string statusText)
		{
			xRaceStatus.Text = statusText;
		}

		#endregion properties

		#region constructor and stuff

		/// <summary>
		/// constructor
		/// </summary>
		public GridBackground()
		{
			InitializeComponent();

			xMyCanvas.PointerReleased += handlePointerReleased;

			uint nrGridRows = GridLinePainter.GetNrGridRows(xMyCanvas);

			// paint background color
			this.xMyCanvas.Background = GridBrushDefs.backgroundBrush;

			// paint the background grid lines
			GridLinePainter.PaintGridLines(xMyCanvas, nrGridRows);

			// get a standard car with acceleration = 1
			mActiveCar = new StandardCar(1);
		}

		#endregion constructor and stuff

		#region paint track (call ITrack to do that) and grid lines

		/// <summary>
		/// Get the track that was chosen and paint that
		/// </summary>
		/// <param name="e"></param>
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			// first paint the track
			int? trackNumber = e.Parameter as int?;
			if (trackNumber != null)
			{
				try
				{
					if (trackNumber != LOAD_TRACK_NR)
					{
						// a predefined track was chosen, get this track
						mActiveTrack = TrackProvider.GetTrack((uint)trackNumber);
					}
					else
					{
						// user chose "Load Track"
						// FileBrowseDialog
						string filePath = await MyFilePicker.LetUserPickFile2Open();

						if (filePath != null)
						{
							// load track
							mActiveTrack = await TrackLoader.LoadTrack(filePath);
						}
					}

					if (mActiveTrack != null)
					{
						mActiveTrack.PaintTrack(xMyCanvas.Children, xMyCanvas.Width, xMyCanvas.Height,
									GridLinePainter.GetMiddleGridRowYCoordinate(xMyCanvas));
						InitRace();
					}
					else
					{
						// loading track failed, navigate back to main
						Back2Main();
					}
				}
				catch (Exception)
				{
					// an exception occurred, return to main page
					Back2Main();
				}
			}			
		}

		private void Back2Main()
		{
			this.Frame.Navigate(typeof(MainPage));
		}

		/// <summary>
		/// Set nr of vectors and paint candidate grid points on starting line
		/// </summary>
		private void InitRace()
		{
			// clear the route
			mRouteGridPoints.Clear();
			xNrVectors.Text = GetNrOfVectors().ToString();

			// remove the driven route to start again
			for (int index = 0; index < mRouteElements.Count; index++)
			{
				xMyCanvas.Children.Remove(mRouteElements[index]);
			}
			mRouteElements.Clear();

			// remove the current candidate grid points
			cgpp.RemoveCurrentCandidateGridPoints(xMyCanvas.Children);
			
			// now mark the candidate grid points for the next move
			IList<Point> candidateGridPoints = mActiveTrack.GetStartingLinePoints();
			cgpp.MarkCandidateGridPoints(xMyCanvas.Children, candidateGridPoints);

			SetStatusField(StateHelper.startRaceString);
			mRaceState = StateHelper.RaceState.Manual;
			xDriveModeSelection.SelectedIndex = 0;
			xDriveModeSelection.IsEnabled = true;
		}

		#endregion paint track and grid lines

		#region button event handlers

		private void restart_Click(object sender, RoutedEventArgs e)
		{
			if (mRaceState == StateHelper.RaceState.ComputerDriver)
			{
				StopComputerDriver();
			}
			InitRace();
		}

		private void back2Main_Click(object sender, RoutedEventArgs e)
		{
			if (mRaceState == StateHelper.RaceState.ComputerDriver)
			{
				StopComputerDriver();
			}

			this.Frame.Navigate(typeof(MainPage));
		}

		private void xDriveModeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string selectedType = xDriveModeSelection.SelectedItem as string;
			if (string.CompareOrdinal("Careful Computer", selectedType) == 0)
			{
				HandleDriveModeChange(true, ComputerDriverType.Careful);
			}
			else if (string.CompareOrdinal("Risky Computer", selectedType) == 0)
			{
				HandleDriveModeChange(true, ComputerDriverType.Risky);
			}
			else if (string.CompareOrdinal("Human", selectedType) == 0)
			{
				HandleDriveModeChange(false, ComputerDriverType.None);				
			}
		}

		#endregion button event handlers

		#region drive mode change

		private void HandleDriveModeChange(bool computerDriver, ComputerDriverType cdt)
		{
			if (computerDriver)
			{
				// first set the new computer drive mode
				mComputerDriverManager.SelectType(cdt);

				if (mRaceState == StateHelper.RaceState.Manual)
				{
					// switch to computer driver as we are in manual mode now
					StartComputerDriver();
				}
				else
				{
					// nothing to do, computer only change driving mode
				}
			}
			else
			{
				if (mRaceState == StateHelper.RaceState.ComputerDriver)
				{
					// stop computer driver
					StopComputerDriver();
				}
			}
		}

		#endregion drive mode change

		#region computer driver

		private void MyTimerCallback(object state)
		{
			// check if the computer driver is still active, could have been stopped in the meantime
			// before the next timer event is fired
			if (mRaceState == StateHelper.RaceState.ComputerDriver)
			{
				if (state is SynchronizationContext)
				{
					SynchronizationContext syncContext = state as SynchronizationContext;
					syncContext.Post(DoOneComputerDriverMove, state);
				}
			}
		}

		private void DoOneComputerDriverMove(object state)
		{
			uint nrBacktracks;
			Point gridPoint;

			mComputerDriverManager.GetNextGridPoint(mActiveTrack, mActiveCar, mRouteGridPoints,
						new Point(	GridLinePainter.GetMiddleGridPointX(xMyCanvas), 
									GridLinePainter.GetMiddleGridPointY(xMyCanvas)),
						out nrBacktracks, out gridPoint);

			if (nrBacktracks == 0)
			{
				if (gridPoint != null)
				{
					HandleGridPoint(gridPoint);
				}
			}
			else
			{
				for (uint index=0; index < nrBacktracks; index++)
				{
					RemoveLastMovementVector();
				}
				
				if (gridPoint != null)
				{
					HandleGridPoint(gridPoint);
				}
			}
		}

		#endregion computer driver

		#region handlePointerReleased and its validation methods

		/// <summary>
		/// Handle a pointer release (mouse click) event and
		/// 1. do validation
		/// 2. paint new movement vector
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void handlePointerReleased(object sender, PointerRoutedEventArgs eventArgs)
		{
			if (hpr_ValidateStatus() == false)
			{
				// do nothing
				return;
			}

			Point gridPointClicked;
			// grid validation of click
			bool clickedPointIsValid = hpr_ValidateClickedPointOnGrid(eventArgs);
			if (clickedPointIsValid == false)
			{
				return;
			}

			// find a matching grid point
			bool validMovementVector = hpr_GetMatchingGridPoint(eventArgs, out gridPointClicked);			
			if (validMovementVector == false)
			{
				return;
			}

			// track validation of gridPointSelected and painting on track
			HandleGridPoint(gridPointClicked);		
		}

		/// <summary>
		/// validate the status of the race for the event that a pointer has been released (e.g. mouse click)
		/// </summary>
		/// <returns></returns>
		private bool hpr_ValidateStatus()
		{
			// first validation - is the race already finished?
			if (mRaceState == StateHelper.RaceState.Finished)
			{
				// we are done, no further movements -> ignore this pointer release
				return false;
			}

			if (mRaceState == StateHelper.RaceState.ComputerDriver)
			{
				// the computer is driving -> ignore this pointer release
				return false;
			}

			return true;
		}

		/// <summary>
		/// Do the validation if pointer release point (i.e. click point) was on right area
		/// </summary>
		/// <param name="eventArgs"></param>
		/// <returns></returns>
		private bool hpr_ValidateClickedPointOnGrid(PointerRoutedEventArgs eventArgs)
		{
			// verify if the clicked point is inside of the outer border or if a gridline has been clicked
			// we cannot differentiate the gridlines, i.e. if they are correct or not, but this first validation filters away clicked points that we do not care about
			var origSource = eventArgs.OriginalSource as Shape;
			if (origSource != null)
			{
				int theZIndex = Canvas.GetZIndex(origSource);
				if ((theZIndex != ZIndexValues.outerBorder)
						&& (theZIndex != ZIndexValues.grid)
						&& (theZIndex != ZIndexValues.candidateGridPointLine))
				{
					// remark: the grid lines that are outside of the track are also accepted, they will be filtered later
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Get a matching grid point if it was clicked near enough to a grid point
		/// </summary>
		private bool hpr_GetMatchingGridPoint(PointerRoutedEventArgs eventArgs, out Point gridPointClicked)
		{
			// get the current point and find the nearest grid point
			PointerPoint ppClicked = eventArgs.GetCurrentPoint(null);
			gridPointClicked = GridBackgroundHelper.ConvertClickedPoint2NearestGridPoint_WithOffset(xMyCanvas, xMyStackPanel, ppClicked);

			// now check if the nearest grid point is close enough in order to uniquely identify which grid point the user intended to click
			Point canvasOffset = GridBackgroundHelper.GetCanvasOffset(xMyCanvas, xMyStackPanel);
			double xGridAsPixel = (gridPointClicked.X * GridBackgroundHelper.gridDistance) + canvasOffset.X;
			double yGridAsPixel = (gridPointClicked.Y * GridBackgroundHelper.gridDistance) + canvasOffset.Y;
			// do the math
			double distance = Math.Sqrt(Math.Abs((xGridAsPixel - ppClicked.Position.X)) + Math.Abs((yGridAsPixel - ppClicked.Position.Y)));
			bool match = distance < GridBackgroundHelper.nearestGridPointDistanceTolerance;
			// only if the user clicked near enough an existing grid point we accept this as valid input
			return match;

			// next lines for debugging only
			/*
			this.xCoordinate.Text = ppClicked.Position.X.ToString();
			this.yCoordinate.Text = ppClicked.Position.Y.ToString();
			this.xGrid.Text = gridPointSelected.X.ToString();
			this.yGrid.Text = gridPointSelected.Y.ToString();
			this.distance.Text = distance.ToString();
			this.match.Text = match.ToString();
			*/
		}

		#endregion handlePointerReleased ... 

		#region handle selected valid grid point methods
		
		/// <summary>
		/// continue with track validation
		/// used both in manual and in computer mode
		/// </summary>
		private void HandleGridPoint(Point gridPointSelected)
		{
			if (mActiveTrack == null)
			{
				throw new InvalidOperationException("we have no track");
			}

			bool trackValidationResult = mActiveTrack.ValidateVector(mRouteGridPoints, gridPointSelected);
			if (trackValidationResult == false)
			{
				return;
			}

			if (mRouteGridPoints.Count == 0)
			{
				HandleGridPoint_StartingLine(gridPointSelected);
			}
			else
			{
				HandleGridPoint_Movement(gridPointSelected);
			}
		}

		/// <summary>
		/// handle a point on the starting line
		/// </summary>
		/// <param name="gridPointClicked"></param>
		private void HandleGridPoint_StartingLine(Point gridPointClicked)
		{
			// very first click on the starting line -> 
			// only store gridPointSelected in routeGridPoints, no vector to paint yet
			mRouteGridPoints.Add(gridPointClicked);

			// remove current candidate grid points (which are on the starting line)
			cgpp.RemoveCurrentCandidateGridPoints(xMyCanvas.Children);

			// now mark the candidate grid points for the next move
			IList<Point> candidateGridPoints = mActiveCar.GetCandidateGridPoints(mRouteGridPoints);
			cgpp.MarkCandidateGridPoints(xMyCanvas.Children, candidateGridPoints);

			if (mRaceState == StateHelper.RaceState.Manual)
			{
				SetStatusField(StateHelper.humanInDriversSeatString);
			}
			else
			{
				SetStatusField(StateHelper.computerInDriversSeatString);
			}			
		}

		/// <summary>
		/// we have a point that represents a movement, i.e. an end point of a new movement vector
		/// </summary>
		/// <param name="gridPointClicked"></param>
		private void HandleGridPoint_Movement(Point gridPointClicked)
		{
			// car validation of gridPointSelected
			bool movementVectorIsValid = mActiveCar.IsValid(mRouteGridPoints, gridPointClicked);
			if (movementVectorIsValid)
			{
				cgpp.RemoveCurrentCandidateGridPoints(xMyCanvas.Children);

				HandleValidMovementVector(gridPointClicked);

				if (mRaceState != StateHelper.RaceState.Finished)
				{
					// now mark the candidate grid points for the next move
					IList<Point> candidateGridPoints = mActiveCar.GetCandidateGridPoints(mRouteGridPoints);
					cgpp.MarkCandidateGridPoints(xMyCanvas.Children, candidateGridPoints);
				}
			}
		}

		/// <summary>
		/// The movement vector is valid, now deal with it and
		/// - paint the movement vector
		/// - increase counter
		/// - detect if race is finished
		/// </summary>
		/// <param name="gridPointClicked"></param>
		private void HandleValidMovementVector(Point gridPointClicked)
		{
			// paint the movement vector
			IList<UIElement> uiElemsMovVector = MovementVectorPainter.PaintMovementVector
							(xMyCanvas.Children, mRouteGridPoints.Last(), gridPointClicked);
			for (int index = 0; index < uiElemsMovVector.Count; index++)
			{
				mRouteElements.Add(uiElemsMovVector[index]);
			}

			// verify if the race is finished
			if (mActiveTrack.CheckIfRaceIsFinished(mRouteGridPoints, gridPointClicked))
			{
				FinishRace();
			}

			mRouteGridPoints.Add(gridPointClicked);
			xNrVectors.Text = GetNrOfVectors().ToString();
		}

		/// <summary>
		/// Remove the last movement vector
		/// </summary>
		private void RemoveLastMovementVector()
		{
			// remove from grid points collection
			mRouteGridPoints.RemoveAt(mRouteGridPoints.Count - 1);
			xNrVectors.Text = GetNrOfVectors().ToString();

			// remove painting from grid
			uint nrOfUIElements = MovementVectorPainter.RemoveLastMovementVector(xMyCanvas.Children, mRouteElements);

			// remove from ui elements collection
			for (uint index=0; index < nrOfUIElements; index++)
			{
				mRouteElements.RemoveAt(mRouteElements.Count - 1);
			}
		}

		#endregion

		#region state changes

		private void FinishRace()
		{
			mRaceState = StateHelper.RaceState.Finished;
			mComputerDriverManager.Stop();
			xDriveModeSelection.IsEnabled = false;
			SetStatusField(StateHelper.endRaceString);
		}

		private void StartComputerDriver()
		{
			mRaceState = StateHelper.RaceState.ComputerDriver;
			SetStatusField(StateHelper.computerInDriversSeatString);
			mComputerDriverManager.Start(MyTimerCallback);
		}

		private void StopComputerDriver()
		{
			mComputerDriverManager.Stop();
			mRaceState = StateHelper.RaceState.Manual;
			SetStatusField(StateHelper.humanInDriversSeatString);
		}

		#endregion state changes
	}
}
