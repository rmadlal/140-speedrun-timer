using System;

namespace SpeedrunTimerMod.Logging
{
	public class RunLog
	{
		const int LEVEL_COUNT = 4;

		public SpeedrunTime[] IndividualLevelTimes { get; private set; }
		public SpeedrunTime[] LevelSplitTimes { get; private set; }
		public DateTime StartDate { get; set; }

		SpeedrunTime[] _levelStartTimes;

		public RunLog()
		{
			IndividualLevelTimes = new SpeedrunTime[LEVEL_COUNT];
			LevelSplitTimes = new SpeedrunTime[LEVEL_COUNT];
			_levelStartTimes = new SpeedrunTime[LEVEL_COUNT];
		}

		public void Clear()
		{
			StartDate = default(DateTime);
			IndividualLevelTimes.Clear();
			LevelSplitTimes.Clear();
			_levelStartTimes.Clear();
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
