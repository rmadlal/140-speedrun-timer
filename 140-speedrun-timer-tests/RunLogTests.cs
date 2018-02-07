using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedrunTimerMod;
using SpeedrunTimerMod.Logging;

namespace SpeedrunTimerModTests
{
	[TestClass]
	public class RunLogTests
	{
		[TestMethod]
		public void AllLevels()
		{
			var log = new RunLog();
			var timeIncrement = new SpeedrunTime(TimeSpan.FromMinutes(3.1374), TimeSpan.FromMinutes(2.7684));
			var timer = new SpeedrunTime();

			for (var lvl = 1; lvl <= log.IndividualLevelTimes.Length; lvl++)
			{
				timer += timeIncrement;
				log.LevelStart(lvl, timer);

				var expectedLevelTime = new SpeedrunTime();
				for (var i = 0; i < lvl; i++)
				{
					timer += timeIncrement;
					expectedLevelTime += timeIncrement;
				}

				log.CompleteLevel(lvl, timer);

				Assert.AreEqual(expectedLevelTime, log.IndividualLevelTimes[lvl - 1]);
				Assert.AreEqual(timer, log.LevelSplitTimes[lvl - 1]);
			}
		}
	}
}
