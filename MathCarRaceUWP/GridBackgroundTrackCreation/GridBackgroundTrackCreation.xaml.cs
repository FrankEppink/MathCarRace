using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace MathCarRaceUWP
{
	/// <summary>
	/// GridBackgroundTrackCreation	
	/// </summary>
	public sealed partial class GridBackgroundTrackCreation : Page
	{
		#region enums

		private enum trackCreationState
		{
			Step01_LeftEndStartLine,
			Step02_RightEndStartLine,
			Step03_OuterCurve,
			Step04_InnerCurve,
			Step05_SaveTrack,
		}

		#endregion enums

		#region constants

		private const string STEP_01_LEFT_END_STARTING_LINE = "Please select the left end of the starting line.";
		private const string STEP_02_RIGHT_END_STARTING_LINE = "Please select the right end of the starting line.";
		private const string STEP_03_OUTER_CURVE = "Please draw the outer curve.";
		private const string STEP_04_INNER_CURVE = "Please draw the inner curve.";
		private const string STEP_05_SAVE_TRACK = "Please save the created track.";

		#endregion constants

		#region members

		private trackCreationState mTrackCreationState;

		#endregion members

		#region constructor and stuff

		/// <summary>
		/// constructor
		/// </summary>
		public GridBackgroundTrackCreation()
		{
			InitializeComponent();

			xMyCanvas.PointerReleased += handlePointerReleased;

			uint nrGridRows = GridLinePainter.GetNrGridRows(xMyCanvas);

			// paint background color
			this.xMyCanvas.Background = GridBrushDefs.backgroundBrush;

			// paint the background grid lines
			GridLinePainter.PaintGridLines(xMyCanvas, nrGridRows);

			mTrackCreationState = trackCreationState.Step01_LeftEndStartLine;
			xInstructions.Text = STEP_01_LEFT_END_STARTING_LINE;

			// TODO paint starting line and mark area where left end of starting line can be selected
		}

		#endregion constructor and stuff

		#region paint track (call ITrack to do that) and grid lines

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// ... something to do here?
		}
		
		#endregion paint track and grid lines

		#region button event handlers

		private void save_Click(object sender, RoutedEventArgs e)
		{
			// TODO track already ready for saving?
		}

		private void back2Main_Click(object sender, RoutedEventArgs e)
		{
			// TODO ask question ... do you want to throw away the current track ...

			this.Frame.Navigate(typeof(MainPage));
		}

		#endregion button event handlers
		
		#region handlePointerReleased and its validation methods

		/// <summary>
		/// Handle a pointer release (mouse click) event and
		/// ...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void handlePointerReleased(object sender, PointerRoutedEventArgs eventArgs)
		{
			
		}

		#endregion handlePointerReleased ... 	

		#region handle state change

		private void DoNextStage()
		{
			switch (mTrackCreationState)
			{
				case trackCreationState.Step01_LeftEndStartLine:
					mTrackCreationState = trackCreationState.Step02_RightEndStartLine;
					xInstructions.Text = STEP_02_RIGHT_END_STARTING_LINE;
					break;
				case trackCreationState.Step02_RightEndStartLine:
					mTrackCreationState = trackCreationState.Step03_OuterCurve;
					xInstructions.Text = STEP_03_OUTER_CURVE;
					break;
				case trackCreationState.Step03_OuterCurve:
					mTrackCreationState = trackCreationState.Step04_InnerCurve;
					xInstructions.Text = STEP_04_INNER_CURVE;
					break;
				case trackCreationState.Step04_InnerCurve:
					mTrackCreationState = trackCreationState.Step05_SaveTrack;
					xInstructions.Text = STEP_05_SAVE_TRACK;
					break;
				default:
					// TODO ...
					break;

			}
		}

		#endregion handle state change
	}
}

