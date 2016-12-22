using Windows.UI.Xaml.Media;

namespace MathCarRaceUWP
{
	internal static class TrackBrushDefs
	{
		/// <summary>
		/// The brush of the track border
		/// </summary>
		internal static SolidColorBrush trackBorderBrush = new SolidColorBrush { Color = Windows.UI.Colors.Black };

		/// <summary>
		/// the brush of the track itself, i.e. the ground the cars race on
		/// </summary>
		internal static SolidColorBrush trackBrush = new SolidColorBrush { Color = Windows.UI.Colors.SandyBrown };

		/// <summary>
		/// the brush of the starting line
		/// </summary>
		internal static SolidColorBrush startingLineBrush = new SolidColorBrush { Color = Windows.UI.Colors.Brown };

		/// <summary>
		/// Inner Part Brush, i.e. the meadow in the middle
		/// </summary>
		internal static SolidColorBrush innerPartBrush = new SolidColorBrush { Color = Windows.UI.Colors.Black };
	}
}
