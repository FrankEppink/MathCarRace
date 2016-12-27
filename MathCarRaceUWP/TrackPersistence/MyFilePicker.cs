using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace MathCarRaceUWP
{
	/// <summary>
	/// This class lets the user 
	/// - pick a track file for loading 
	/// or 
	/// - choose a file for saving a created track into
	/// </summary>
	internal class MyFilePicker
	{
		#region constants

		private const string FILE_EXT = ".mcr";

		#endregion constants

		internal static async Task<string> LetUserPickFile2Open()
		{
			var picker = new Windows.Storage.Pickers.FileOpenPicker();
			picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
			picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.ComputerFolder;
			picker.FileTypeFilter.Add(FILE_EXT);

			StorageFile file = await picker.PickSingleFileAsync();
			if (file != null)
			{
				// we need to add this file to ... to prevent an 'UnauthorizedAccessException' when trying to access it
				var fileToken = StorageApplicationPermissions.FutureAccessList.Add(file);

				// Application now has read/write access to the picked file
				return file.Path;
			}
			else
			{
				return null;
			}
		}

		internal static string LetUserPickFile2Save()
		{
			// FileSavePicker
			// TODO
			return null;
		}
	}
}
