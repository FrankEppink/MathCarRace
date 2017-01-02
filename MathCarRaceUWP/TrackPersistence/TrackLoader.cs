using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace MathCarRaceUWP
{
	internal static class TrackLoader
	{
		#region constants

		/// <summary>
		/// symbol for the separation of segments like starting line, outer and inner curve
		/// </summary>
		private const char SEPARATOR_SEGMENT = '#';

		/// <summary>
		/// separate numbers or points in one segment with this
		/// </summary>
		private const char SEPARATOR_NUMBER = ',';

		/// <summary>
		/// the width and the height value of one point are separated by this
		/// </summary>
		private const char SEPARATOR_HEIGHT_WIDTH = '/';
		
		/// <summary>
		/// the TRIM CHARS are removed before trying to parse a string
		/// </summary>
		private static char[] TRIM_CHARS = new char[] { '\r', '\n', ' ' };

		#endregion constants

		#region internal methods

		internal static async Task<ITrack> LoadTrack(string filePath)
		{
			// load text that defines track from file
			string completeContent = await GetStringFromTrackFile(filePath);

			// now parse the text and fill into LoadedTrack
			return CreateTrack(completeContent);
		}

		#endregion internal methods

		#region private methods - LoadTrack

		private static async Task<string> GetStringFromTrackFile(string filePath)
		{
			StorageFile trackFile = await StorageFile.GetFileFromPathAsync(filePath);

			// load and parse the file
			StringBuilder sb = new StringBuilder();
			var buffer = await FileIO.ReadBufferAsync(trackFile);
			using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
			{
				string text = dataReader.ReadString(buffer.Length);
				sb.Append(text);
			}
			string completeContent = sb.ToString();

			return completeContent;
		}

		private static ITrack CreateTrack(string completeContent)
		{
			LoadedTrack loadedTrack = new LoadedTrack();			

			// split into segments: Starting line, outer curve, inner curve
			string[] segments = completeContent.Split(SEPARATOR_SEGMENT);
			if (segments == null) return null;
			if (segments.Length != 3) return null;

			// parse starting line
			bool segmentResult = ParseStartingLine(segments[0], loadedTrack);
			if (segmentResult == false) return null;
			
			segmentResult = ParseOuterCurve(segments[1], loadedTrack);
			if (segmentResult == false) return null;

			segmentResult = ParseInnerCurve(segments[2], loadedTrack);
			if (segmentResult == false) return null;

			return loadedTrack;
		}

		private static bool ParseStartingLine(string startingLine, LoadedTrack loadedTrack)
		{
			int searchIndex = 0;
			double parseResult;
			bool parseFlag;
			string subString;
			string trimString;

			int resultIndex = startingLine.IndexOf(SEPARATOR_NUMBER, searchIndex);
			if (resultIndex == -1) return false;

			subString = startingLine.Substring(0, resultIndex);
			parseFlag = Double.TryParse(subString, out parseResult);
			if (parseFlag == false) return false;

			loadedTrack.widthLeftStartingPoint = parseResult;
			searchIndex = resultIndex + 1; // length of SEPARATOR

			subString = startingLine.Substring(searchIndex);
			trimString = subString.Trim(TRIM_CHARS);

			parseFlag = Double.TryParse(trimString, out parseResult);
			if (parseFlag == false) return false;

			loadedTrack.widthRightStartingPoint = parseResult;

			return true;
		}

		private static bool ParseOuterCurve(string outerCurve, LoadedTrack loadedTrack)
		{
			loadedTrack.outerPoints = new List<Point>();
			return ParseCurve(outerCurve, loadedTrack.outerPoints);
		}

		private static bool ParseInnerCurve(string innerCurve, LoadedTrack loadedTrack)
		{
			loadedTrack.innerPoints = new List<Point>();
			return ParseCurve(innerCurve, loadedTrack.innerPoints);
		}

		private static bool ParseCurve(string curve, IList<Point> points)
		{
			int searchIndex = 0;		
			int resultIndex = curve.IndexOf(SEPARATOR_NUMBER, searchIndex);
			Point? nextPoint;
			string subString;

			while (resultIndex != -1)
			{
				// we found the next NUMBER/POINT separator
				subString = curve.Substring(searchIndex, resultIndex - searchIndex);
				nextPoint = ParsePoint(subString);
				if (nextPoint.HasValue == false) return false;
				points.Add(nextPoint.Value);
				searchIndex = resultIndex + 1;

				resultIndex = curve.IndexOf(SEPARATOR_NUMBER, searchIndex);
			}

			// get last point
			subString = curve.Substring(searchIndex);
			nextPoint = ParsePoint(subString);
			if (nextPoint.HasValue == false) return false;
			points.Add(nextPoint.Value);

			return true;
		}

		private static Point? ParsePoint(string point)
		{
			int resultIndex = point.IndexOf(SEPARATOR_HEIGHT_WIDTH);
			if (resultIndex == -1) return null;

			string weigthString = point.Substring(0, resultIndex);
			string heightString = point.Substring(resultIndex + 1);

			string weightStringTrimmed = weigthString.Trim(TRIM_CHARS);
			string heightStringTrimmed = heightString.Trim(TRIM_CHARS);

			bool parseFlag;
			double parseResultWeight;
			parseFlag = Double.TryParse(weightStringTrimmed, out parseResultWeight);
			if (parseFlag == false) return null;

			double parseResultHeight;
			parseFlag = Double.TryParse(heightStringTrimmed, out parseResultHeight);
			if (parseFlag == false) return null;

			Point nextPoint = new Point(parseResultWeight, parseResultHeight);
			return nextPoint;
		}

		#endregion private methods - LoadTrack
	}
}
