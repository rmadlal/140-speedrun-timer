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
			var lastSplitOld = OldTimingLog.LevelSplitTimes[lastSplitIndex];

			return GetLogCsv(categoryName, Log.StartDate, lastSplit, lastSplitOld);
		}

		string GetIndividualLevelCsv(int levelIndex)
		{
			var levelTime = Log.IndividualLevelTimes[levelIndex];
			var levelTimeOld = OldTimingLog.IndividualLevelTimes[levelIndex];

			var startDate = Log.StartDate;
			if (levelIndex >= 0)
			{
				startDate = Log.GetLevelStartDate(levelIndex + 1);
			}

			return GetLogCsv($"LEVEL {levelIndex + 1}", startDate, levelTime, levelTimeOld);
		}

		string GetLogCsv(string category, DateTime startDate, SpeedrunTime time,
			SpeedrunTime oldTimingTime)
		{
			var startDateStr = startDate.ToString("s", CultureInfo.InvariantCulture);

			var decimals = 3;
			var realTimeStr = Utils.FormatTime(time.RealTime, decimals);
			var gameTimeStr = Utils.FormatTime(time.GameTime, decimals);

			var realTimeOldStr = Utils.FormatTime(oldTimingTime.RealTime, decimals);
			var gameTimeOldStr = Utils.FormatTime(oldTimingTime.GameTime, decimals);

			var version = Log.Version;
			var gameVersion = Log.IsLegacy ? "2013" : "2017";

			return category + ",START DATE (UTC),REAL TIME,GAME TIME,GAME TIME (RAW)"
				+ ",REAL TIME (OLD TIMING),GAME TIME (OLD TIMING)"
				+ ",CHEATS,MOD VERSION,GAME VERSION"
				+ Environment.NewLine
				+ $",{startDateStr},{realTimeStr},{gameTimeStr},{time.GameBeatTime}"
				+ $",{realTimeOldStr},{gameTimeOldStr}"
				+ $",{Log.CheatsEnabled},{Log.Version},{gameVersion}"
				+ Environment.NewLine;
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
