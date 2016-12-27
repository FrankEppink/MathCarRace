using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MathCarRaceUWP
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		private void raceTrack01_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 1);
		}

		private void raceTrack02_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 2);
		}

		private void raceTrack03_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 3);
		}

		private void raceTrack04Random_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 4);
		}

		private void raceTrack05Real_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 5);
		}

		private void raceTrack06Real_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 6);
		}

		private void createTrack_Click(object sender, RoutedEventArgs e)
		{
			// NYI
			this.createTrack.Content = "NYI";
		}

		private void loadTrack_Click(object sender, RoutedEventArgs e)
		{
			// passing the special number which says "Please load a track"
			this.Frame.Navigate(typeof(GridBackground), GridBackground.LOAD_TRACK_NR);
		}
	}
}
