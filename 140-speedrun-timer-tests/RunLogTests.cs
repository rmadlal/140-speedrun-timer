using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedrunTimerMod;

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

			for (var i = 1; i <= log.IndividualLevelTimes.Length; i++)
			{
				timer += timeIncrement;
				log.LevelStart(i, timer);

				var expectedLevelTime = new SpeedrunTime();
				for (var j = 0; j < i; j++)
				{
					timer += timeIncrement;
					expectedLevelTime += timeIncrement;
				}

				log.CompleteLevel(i, timer);

				Assert.AreEqual(expectedLevelTime, log.IndividualLevelTimes[i - 1]);
				Assert.AreEqual(timer, log.LevelSplitTimes[i - 1]);
			}
		}
	}
}
