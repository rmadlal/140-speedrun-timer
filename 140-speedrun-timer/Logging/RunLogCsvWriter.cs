using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace SpeedrunTimerMod.Logging
{
	public class RunLogCsvWriter
	{
		public RunLog Log { get; set; }
		public RunLog OldTimingLog { get; set; }

		public bool Level1 { get; set; }
		public bool Level2 { get; set; }
		public bool Level3 { get; set; }
		public bool Level4 { get; set; }

		public bool AnyPercent { get; set; }
		public bool AllLevels { get; set; }

		public RunLogCsvWriter(RunLog log, RunLog oldTimingLog)
		{
			if (log == null || oldTimingLog == null)
				throw new ArgumentNullException();

			Log = log;
			OldTimingLog = oldTimingLog;
		}

		public IEnumerator WriteToLogAsyncOnFrameEnd()
		{
			yield return new WaitForEndOfFrame();
			WriteToLogAsync();
		}

		public void WriteToLog()
		{
			if (!AnyPercent && !AllLevels && CountSelectedLevels() == 0)
				throw new InvalidOperationException("No level selected.");

			RunLogFile.WriteLine(GetCsv());
		}

		public void WriteToLogAsync()
		{
			if (!AnyPercent && !AllLevels && CountSelectedLevels() == 0)
				throw new InvalidOperationException("No level selected.");

			RunLogFile.WriteLineAsync(GetCsv());
		}

		public string GetCsv()
		{
			var str = string.Empty;

			for (var i = 0; i < 4; i++)
			{
				if (CheckIfPrintLevel(i))
					str += GetIndividualLevelCsv(i);
			}

			if (AnyPercent)
			{
				str += GetAnyPercentCsv();
			}

			if (AllLevels)
			{
				str += GetAllLevelsCsv();
			}

			return str;
		}

		string GetAnyPercentCsv() => GetFullGameCsv("ANY%", 2);

		string GetAllLevelsCsv() => GetFullGameCsv("ALL LEVELS", 3);

		string GetFullGameCsv(string categoryName, int lastSplitIndex)
        {
			var lastSplit = Log.LevelSplitTimes[lastSplitIndex];
			var realTime = Utils.FormatTime(lastSplit.RealTime, 3);
			var gameTime = Utils.FormatTime(lastSplit.GameTime, 3);
			var gameBeatTime = lastSplit.GameBeatTime;

			var lastSplitOld = OldTimingLog.LevelSplitTimes[lastSplitIndex];
			var realTimeOld = Utils.FormatTime(lastSplitOld.RealTime, 3);
			var gameTimeOld = Utils.FormatTime(lastSplitOld.GameTime, 3);

			var metadata = GetMetadataCsv();

			return $"{categoryName},REAL TIME,GAME TIME,GAME TIME (RAW)"
				+ ",REAL TIME (OLD TIMING),GAME TIME (OLD TIMING)"
				+ "," + metadata[0]
				+ Environment.NewLine
				+ $",{realTime},{gameTime},{gameBeatTime}"
				+ $",{realTimeOld},{gameTimeOld}"
				+ "," + metadata[1]
				+ Environment.NewLine;
		}

		string GetIndividualLevelCsv(int levelIndex, bool withMetadata = true)
		{
			var levelTime = Log.IndividualLevelTimes[levelIndex];
			var realTime = Utils.FormatTime(levelTime.RealTime, 3);
			var gameTime = Utils.FormatTime(levelTime.GameTime, 3);
			var gameBeatTime = levelTime.GameBeatTime;

			var levelTimeOld = OldTimingLog.IndividualLevelTimes[levelIndex];
			var realTimeOld = Utils.FormatTime(levelTimeOld.RealTime, 3);
			var gameTimeOld = Utils.FormatTime(levelTimeOld.GameTime, 3);

			var metadataHeader= string.Empty;
			var metadataValues = string.Empty;
			if (withMetadata)
			{
				var metadata = GetMetadataCsv(levelIndex);
				metadataHeader = "," + metadata[0];
				metadataValues = "," + metadata[1];
			}

			return $"LEVEL {levelIndex + 1},REAL TIME,GAME TIME,GAME TIME (RAW)"
				+ ",REAL TIME (OLD TIMING),GAME TIME (OLD TIMING)"
				+ metadataHeader
				+ Environment.NewLine
				+ $",{realTime},{gameTime},{gameBeatTime},"
				+ $"{realTimeOld},{gameTimeOld}"
				+ metadataValues
				+ Environment.NewLine;
		}

		string[] GetMetadataCsv(int levelIndex = -1)
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			var startDate = Log.StartDate;

			if (levelIndex >= 0)
			{
				var levelSplitTime = Log.LevelSplitTimes[levelIndex].RealTime;
				var levelTime = Log.IndividualLevelTimes[levelIndex].RealTime;
				var levelStartTime = levelSplitTime - levelTime;
				startDate += levelStartTime;
			}

			var startDateStr = startDate.ToString("s", CultureInfo.InvariantCulture);
			return new string[] {
				"START DATE (UTC),CHEATS,VERSION",
				$"{startDateStr},{Cheats.Enabled},{version}"
			};
		}

		int CountSelectedLevels()
		{
			var count = 0;
			for (var i = 0; i < 4; i++)
			{
				if (CheckIfPrintLevel(i))
					count++;
			}
			return count;
		}

		bool CheckIfPrintLevel(int levelIndex)
		{
			switch (levelIndex)
			{
				case 0:
					return Level1;
				case 1:
					return Level2;
				case 2:
					return Level3;
				case 3:
					return Level4;
				default:
					return false;
			}
		}
	}
}
