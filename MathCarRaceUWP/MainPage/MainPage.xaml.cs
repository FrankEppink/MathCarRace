
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

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
			// this.Navigate(new Uri("Grid.xaml", UriKind.Relative));

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

		private void raceTrack04_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GridBackground), 4);
		}
	}
}
