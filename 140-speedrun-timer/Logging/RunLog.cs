using System;
using System.Reflection;

namespace SpeedrunTimerMod.Logging
{
	public class RunLog
	{
		const int LEVEL_COUNT = 4;

		public SpeedrunTime[] IndividualLevelTimes { get; private set; }
		public SpeedrunTime[] LevelSplitTimes { get; private set; }
		public DateTime StartDate { get; set; }
		public bool CheatsEnabled { get; set; }
		public bool IsLegacy { get; set; }
		public Version Version { get; set; }

		//TODO: class holding the info for one level
		// writers should take a IList<ThatClass> instead

		SpeedrunTime[] _levelStartTimes;

		public RunLog()
		{
			IndividualLevelTimes = new SpeedrunTime[LEVEL_COUNT];
			LevelSplitTimes = new SpeedrunTime[LEVEL_COUNT];
			_levelStartTimes = new SpeedrunTime[LEVEL_COUNT];
			IsLegacy = ModLoader.IsLegacyVersion;
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}

		public void LevelStart(int level, SpeedrunTime timestamp)
		{
			ThrowIfLevelOutOfRange(level);

			_levelStartTimes[level - 1] = timestamp;
		}

		public void CompleteLevel(int level, SpeedrunTime timestamp)
		{
			ThrowIfLevelOutOfRange(level);

			var index = level - 1;
			IndividualLevelTimes[index] = timestamp - _levelStartTimes[index];
			LevelSplitTimes[index] = timestamp;
		}

		public bool CheckIfLevelDone(int level)
		{
			ThrowIfLevelOutOfRange(level);

			return IndividualLevelTimes[level - 1].RealTime != TimeSpan.Zero;
		}

		public DateTime GetLevelStartDate(int level)
		{
			ThrowIfLevelOutOfRange(level);

			var index = level - 1;
			var levelSplitTime = LevelSplitTimes[index].RealTime;
			var levelTime = IndividualLevelTimes[index].RealTime;
			var levelStartTime = levelSplitTime - levelTime;
			return StartDate + levelStartTime;
		}

		void ThrowIfLevelOutOfRange(int level)
		{
			if (!CheckLevelRange(level))
				throw new ArgumentOutOfRangeException(nameof(level));
		}

		bool CheckLevelRange(int level)
		{
			return level >= 1 && level <= LEVEL_COUNT;
		}
	}
}
