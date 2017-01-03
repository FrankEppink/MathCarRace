using System.Collections.Generic;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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
			Step01_OuterCurve,
			Step01_OuterCurveFinished,
			Step02_InnerCurve,
			Step03_SaveTrack,
			Error,
		}

		#endregion enums

		#region constants

		private const string STARTING_LINE_DRAW = "Start drawing BELOW and end drawing ABOVE at the starting line.";		
		private const string STEP_01_OUTER_CURVE = "Please draw the outer curve in clockwise direction and in one movement.\n" + STARTING_LINE_DRAW;
		private const string STEP_01_OUTER_CURVE_FINISHED = "Outer curve finished.";
		private const string STEP_02_INNER_CURVE = "Please draw the inner curve in clockwise direction and in one movement.\n" + STARTING_LINE_DRAW;
		private const string STEP_03_SAVE_TRACK = "Please save the created track.";

		private const string ERROR_RESTART = "Please do 'restart' to start drawing from scratch";
		private const string ERROR_STARTING_LINE_X_WRONG_DIRECTION = "Please do not cross the starting line in the wrong direction!\n" + ERROR_RESTART;
		private const string ERROR_STARTING_LINE_NOT_X_TWICE = "Please cross the starting line twice for each curve!\n" + ERROR_RESTART;
		private const string ERROR_INTERNAL = "An internal error has occurred!\n" + ERROR_RESTART;
		private const string ERROR_INTERSECTION_OUTER = "Please do not cross the outer curve\n" + ERROR_RESTART;
		private const string ERROR_INTERSECTION_INNER = "Please do not cross the inner curve\n" + ERROR_RESTART;

		/// <summary>
		/// The background brush
		/// </summary>
		private static SolidColorBrush BackgroundBrushTrackCreation = new SolidColorBrush { Color = Windows.UI.Colors.White };

		/// <summary>
		/// The brush for error instruction texts
		/// </summary>
		private static SolidColorBrush ErrorInstructionBrush = new SolidColorBrush { Color = Windows.UI.Colors.Red };

		/// <summary>
		/// The brush for normal instruction texts
		/// </summary>
		private static SolidColorBrush NormalInstructionBrush = new SolidColorBrush { Color = Windows.UI.Colors.Chocolate };

		#endregion constants

		#region members

		/// <summary>
		/// The starting line candidate left point (which is always the same) 
		/// </summary>
		private Point mStartingLineCandidateLeftPoint;

		/// <summary>
		/// The starting line candidate right point (which is always the same) 
		/// </summary>
		private Point mStartingLineCandidateRightPoint;

		/// <summary>
		/// The current state
		/// </summary>
		private trackCreationState mTrackCreationState;

		/// <summary>
		/// Only if mPointerPressed is true we accept PointerMoved as a drawing action
		/// </summary>
		private bool mPointerPressed = false;

		/// <summary>
		/// the previous point (of the pointer, e.g. of the mouse)
		/// </summary>
		private Point? mPreviousPoint;

		/// <summary>
		/// The collection of outer curve UIElements (Lines)
		/// </summary>
		private IList<Line> mOuterCurveUIElements = new List<Line>();

		/// <summary>
		/// The collection of inner curve UIElements (Lines)
		/// </summary>
		private IList<Line> mInnerCurveUIElements = new List<Line>();

		/// <summary>
		/// The starting line needs to have been intersected/crossed twice (once at the start and once at the end) so that a curve is valid
		/// </summary>
		private uint mNrStartingLineIntersected = 0;

		#endregion members

		#region constructor and stuff

		/// <summary>
		/// constructor
		/// </summary>
		public GridBackgroundTrackCreation()
		{
			InitializeComponent();

			xMyCanvas.PointerPressed += handlePointerPressed;
			xMyCanvas.PointerMoved += handlePointerMoved;
			xMyCanvas.PointerReleased += handlePointerReleased;

			uint nrGridRows = GridLinePainter.GetNrGridRows(xMyCanvas);

			// paint background color of track creation grid
			this.xMyCanvas.Background = BackgroundBrushTrackCreation;

			// paint the background grid lines
			GridLinePainter.PaintGridLines(xMyCanvas, nrGridRows);

			// paint starting line candidate so that user knows where to start
			PaintStartingLineCandidate();

			// reset all drawing member variables
			ResetDrawings();
		}

		/// <summary>
		/// Paint starting line candidate so that user knows where to start
		/// </summary>
		private void PaintStartingLineCandidate()
		{
			uint startingLineYCoordinate = GridLinePainter.GetMiddleGridRowYCoordinate(xMyCanvas);
			uint startingLineXRightGridPoint = GridLinePainter.GetMiddleGridPointX(xMyCanvas);
			double startingLineXRightCoordinate = startingLineXRightGridPoint * GridBackgroundHelper.gridDistance;

			mStartingLineCandidateLeftPoint.X = 0.0;
			mStartingLineCandidateLeftPoint.Y = startingLineYCoordinate;

			mStartingLineCandidateRightPoint.X = startingLineXRightCoordinate;
			mStartingLineCandidateRightPoint.Y = startingLineYCoordinate;

			Line startingLineCandidate = new Line();
			startingLineCandidate.Stroke = TrackBrushDefs.startingLineBrush;
			startingLineCandidate.StrokeThickness = 3;
			startingLineCandidate.X1 = mStartingLineCandidateLeftPoint.X;
			startingLineCandidate.Y1 = mStartingLineCandidateLeftPoint.Y;
			startingLineCandidate.X1 = mStartingLineCandidateRightPoint.X;
			startingLineCandidate.Y2 = mStartingLineCandidateRightPoint.Y;
			
			Canvas.SetZIndex(startingLineCandidate, ZIndexValues.startingLine);
			xMyCanvas.Children.Add(startingLineCandidate);
		}

		/// <summary>
		/// Reset the complete drawings to enable the user to start from scratch
		/// </summary>
		private void ResetDrawings()
		{
			mTrackCreationState = trackCreationState.Step01_OuterCurve;
			SetInstructionText(false, STEP_01_OUTER_CURVE);

			foreach (UIElement elem in mOuterCurveUIElements) { xMyCanvas.Children.Remove(elem); }
			mOuterCurveUIElements.Clear();

			foreach (UIElement elem in mInnerCurveUIElements) { xMyCanvas.Children.Remove(elem); }
			mInnerCurveUIElements.Clear();

			ResetCurveDrawing();

			mTrackCreationState = trackCreationState.Step01_OuterCurve;
		}

		/// <summary>
		/// Reset after one curve has been drawn to enable the user to start drawing the second curve
		/// </summary>
		private void ResetCurveDrawing()
		{
			mPointerPressed = false;
			mPreviousPoint = new Point?();
			mNrStartingLineIntersected = 0;

			xSave.IsEnabled = true;
		}

		#endregion constructor and stuff

		#region button event handlers

		private async void save_Click(object sender, RoutedEventArgs e)
		{
			StorageFile myStorageFile = await MyFilePicker.LetUserPickFile2Save();

			if (myStorageFile != null)
			{
				save_DoIt(myStorageFile);

				DoNextStage();
			}
		}

		private void restart_Click(object sender, RoutedEventArgs e)
		{
			ResetDrawings();
		}

		private void back2Main_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}

		#endregion button event handlers

		#region handlePointer...

		private void handlePointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if ((mTrackCreationState == trackCreationState.Step01_OuterCurve) || (mTrackCreationState == trackCreationState.Step02_InnerCurve))
			{
				mPointerPressed = true;
			}
		}

		private void handlePointerMoved(object sender, PointerRoutedEventArgs e)
		{
			// first check if we are painting the outer or the inner curve
			if ((mTrackCreationState == trackCreationState.Step01_OuterCurve) || (mTrackCreationState == trackCreationState.Step02_InnerCurve))
			{
				// now check if the pointer is pressed
				if (mPointerPressed)
				{
					// get the current coordinates
					PointerPoint currentPointerPoint = e.GetCurrentPoint(xMyCanvas);
					Point currentPoint = currentPointerPoint.Position;

					// check if we already have a previous point so that we have line
					if (mPreviousPoint.HasValue)
					{
						// check if the distance is big enough so that it is worth for a line
						if (VectorMath.CalculateMaxXorYDiffAbsolute(currentPoint, mPreviousPoint.Value) > 1)
						{
							// check if we already have crossed the starting line so that we start painting
							// we ignore all painting before we have crossed the starting line for the first time
							// to make sure we have a proper start
							if (mNrStartingLineIntersected > 0)
							{
								Line newLine;
								bool valid = handlePointerMoved_Validate(currentPoint, out newLine);
								if (valid)
								{
									handlePointerMoved_PaintIt(newLine);
								}
							}

							// check if and handle the starting line intersections
							if (handlePointerMoved_StartingLineX(currentPoint) == false)
							{
								mPreviousPoint = currentPoint;
							}
						}
					}
					else
					{
						// no previous point -> set it initially from the current point
						mPreviousPoint = currentPoint;
					}
				}
			}	
		}

		/// <summary>
		/// Validate the new Point, i.e. the new line
		/// </summary>
		/// <param name="currentPoint"></param>
		/// <returns></returns>
		private bool handlePointerMoved_Validate(Point currentPoint, out Line newLine)
		{
			bool result = true;

			// Does the new line intersect with a curve, this is not allowed
			newLine = new Line();
			newLine.Stroke = TrackBrushDefs.trackBorderBrush;

			newLine.X1 = mPreviousPoint.Value.X;
			newLine.Y1 = mPreviousPoint.Value.Y;

			newLine.X2 = currentPoint.X;
			newLine.Y2 = currentPoint.Y;

			int position;
			if (mTrackCreationState == trackCreationState.Step01_OuterCurve)
			{
				// check for intersection with the outer curve, we however accept an intersection with the last line of outer curve,
				// because that is natural for lines that are chained together
				if (VectorMathUI.CheckIfLineIntersectsWithLineCollection(newLine, mOuterCurveUIElements, out position))
				{
					if (position != mOuterCurveUIElements.Count - 1)
					{
						// invalid intersection
						SetInstructionText(true, ERROR_INTERSECTION_OUTER);
						mTrackCreationState = trackCreationState.Error;
						result = false;
					}
				}
			}			
			else if (mTrackCreationState == trackCreationState.Step02_InnerCurve)
			{
				// user is currently drawing the inner curve ->
				// check for an intersection with the outer curve which is always invalid, independant of the position
				if (VectorMathUI.CheckIfLineIntersectsWithLineCollection(newLine, mOuterCurveUIElements, out position))
				{					
					// invalid intersection
					SetInstructionText(true, ERROR_INTERSECTION_OUTER);
					mTrackCreationState = trackCreationState.Error;
					result = false;
				}
				if (result)
				{
					// so far we are okay ->
					// check for an intersection with the inner curve, we accept an intersection with the last line of inner curve,
					// because that is natural for lines that are chained together
					if (VectorMathUI.CheckIfLineIntersectsWithLineCollection(newLine, mInnerCurveUIElements, out position))
					{
						if (position != mInnerCurveUIElements.Count - 1)
						{
							// invalid intersection
							SetInstructionText(true, ERROR_INTERSECTION_INNER);
							mTrackCreationState = trackCreationState.Error;
							result = false;
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Actually paint a part of the curve and memorize it
		/// </summary>
		private void handlePointerMoved_PaintIt(Line newLine)
		{
			// actually paint the new line that becomes part of the (current) curve
			xMyCanvas.Children.Add(newLine);

			// memorize the current point for later construction of the curve based on polygons
			if (mTrackCreationState == trackCreationState.Step01_OuterCurve) { mOuterCurveUIElements.Add(newLine); }
			else if (mTrackCreationState == trackCreationState.Step02_InnerCurve) { mInnerCurveUIElements.Add(newLine); }
		}

		/// <summary>
		/// Check if and handle the starting line intersections
		/// </summary>
		/// <returns>true: starting line has been crossed for the second time -> curve is finished, false: otherweise</returns>
		private bool handlePointerMoved_StartingLineX(Point currentPoint)
		{
			bool startingLineIntersected = VectorMath.CheckIf2LinesIntersect(mStartingLineCandidateLeftPoint, mStartingLineCandidateRightPoint, mPreviousPoint.Value, currentPoint);
			if (startingLineIntersected)
			{
				// check if we have the right angle, i.e. y coordinates must always get less
				if ((currentPoint.Y - mPreviousPoint.Value.Y) < 0)
				{
					// right direction
					mNrStartingLineIntersected++;
					if (mNrStartingLineIntersected == 2)
					{
						// we are done the starting line has been intersected in the right direction for the second time
						CurvePaintingFinished();
						return true;
					}
				}
				else
				{
					// wrong direction -> user needs to start drawing from scratch
					SetInstructionText(true, ERROR_STARTING_LINE_X_WRONG_DIRECTION);
					mTrackCreationState = trackCreationState.Error;
				}
			}

			return false;
		}

		/// <summary>
		/// Handle a pointer release (mouse click) event
		/// </summary>
		private void handlePointerReleased(object sender, PointerRoutedEventArgs eventArgs)
		{
			if (mTrackCreationState == trackCreationState.Step01_OuterCurveFinished)
			{
				// we need this for the transition of the outer curve to the inner curver
				DoNextStage();
			}
			else if ((mTrackCreationState == trackCreationState.Step01_OuterCurve) || (mTrackCreationState == trackCreationState.Step02_InnerCurve))
			{
				// check if the user has released the pointer too early
				if (mNrStartingLineIntersected < 2)
				{
					SetInstructionText(true, ERROR_STARTING_LINE_NOT_X_TWICE);
					mTrackCreationState = trackCreationState.Error;
				}
			}
		}

		#endregion handlePointerReleased ... 	

		#region validation of curve

		/// <summary>
		/// The painting of a curve is finished once the starting line has been crossed
		/// </summary>
		private void CurvePaintingFinished()
		{
			ResetCurveDrawing();

			// now validate if
			// 1. curve starts on starting line - Verified by starting line X check
			// 2. curve is complete - Verified by starting line X check
			// 3. curve ends on starting line - Verified by starting line X check
			// 4. curve surrounds the middle circle - ???

			// TODO: check if validations are sufficient and working fine, is it possible to workaround them?

			// TODO: missing validation: each curves must not cross
			// A. itself
			// B. the other curve

			// TODO start and end of starting line of each curve should be identical/similar

			// do next stage
			DoNextStage();
		}

		#endregion validation of curve

		#region handle state change

		private void DoNextStage()
		{
			switch (mTrackCreationState)
			{
				case trackCreationState.Step01_OuterCurve:
					mTrackCreationState = trackCreationState.Step01_OuterCurveFinished;
					SetInstructionText(false, STEP_01_OUTER_CURVE_FINISHED);
					break;
				case trackCreationState.Step01_OuterCurveFinished:
					mTrackCreationState = trackCreationState.Step02_InnerCurve;
					SetInstructionText(false, STEP_02_INNER_CURVE);
					break;
				case trackCreationState.Step02_InnerCurve:
					mTrackCreationState = trackCreationState.Step03_SaveTrack;
					SetInstructionText(false, STEP_03_SAVE_TRACK);
					xSave.IsEnabled = true;
					break;
				case trackCreationState.Step03_SaveTrack:
					this.Frame.Navigate(typeof(MainPage));
					break;
				default:
					SetInstructionText(true, ERROR_INTERNAL);
					break;
			}
		}

		#endregion handle state change

		#region set instruction text

		private void SetInstructionText(bool error, string text)
		{
			if (error) { xInstructions.Foreground = ErrorInstructionBrush; }
			else { xInstructions.Foreground = NormalInstructionBrush; }

			xInstructions.Text = text;
		}

		#endregion set instruction text

		#region private save methods

		private void save_DoIt(StorageFile myStorageFile)
		{
			IList<Point> outerCurve = ConvertUIElementList2PointList(mOuterCurveUIElements);
			IList<Point> innerCurve = ConvertUIElementList2PointList(mInnerCurveUIElements);

			TrackLoader.SaveTrack(myStorageFile, outerCurve, innerCurve);
		}

		private IList<Point> ConvertUIElementList2PointList(IList<Line> curve)
		{
			IList<Point> result = new List<Point>();

			foreach(Line oneLine in curve)
			{
				// always take the starting point of a line and
				// transform width and height into percentage values
				double percX1 = (oneLine.X1 / xMyCanvas.Width) * 100;
				double percY1 = (oneLine.Y1 / xMyCanvas.Height) * 100;
				Point onePoint = new Point(percX1, percY1);
				result.Add(onePoint);
			}

			return result;
		}

		#endregion private save methods
	}
}


