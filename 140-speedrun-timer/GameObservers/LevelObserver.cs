using UnityEngine;

namespace SpeedrunTimerMod.GameObservers
{
	[GameObserver]
	class LevelObserver : MonoBehaviour
	{
		bool _isInMenu;
		bool _endSoundPlaying;

		void Start()
		{
			SubscribeGlobalBeatMaster();
			_isInMenu = Application.loadedLevelName == "Level_Menu";
			_endSoundPlaying = TheEndSound.EndSoundPlaying() && _isInMenu;

			if (!_isInMenu && !Cheats.LevelLoadedByCheat)
				SpeedrunTimer.Instance.Split();
		}

		void OnEnable()
		{
			SubscribeGlobalBeatMaster();
		}

		void OnDisable()
		{
			UnsubGlobalBeatMaster();
		}

		void SubscribeGlobalBeatMaster()
		{
			if (Globals.beatMaster == null)
				return;

			UnsubGlobalBeatMaster();
			Globals.beatMaster.globalBeatStarted += OnGlobalBeatStarted;
		}

		void UnsubGlobalBeatMaster()
		{
			if (Globals.beatMaster == null)
				return;

			Globals.beatMaster.globalBeatStarted -= OnGlobalBeatStarted;
		}

		void OnGlobalBeatStarted()
		{
			// we know the beat starts 1 second after level load
			// except when end sound is playing
			// see GlobalBeatMaster.startTime
			var beatStartTime = !_endSoundPlaying ? -1000 : -3000;

			if (SpeedrunTimer.Instance.IsRunning)
			{
				SpeedrunTimer.Instance.EndLoad(beatStartTime);
			}

			if (!_isInMenu)
			{
				var level = (int)char.GetNumericValue(Application.loadedLevelName[5]);
				if (level > 0)
				{
					Debug.Log($"Level {level} started\n" + DebugBeatListener.DebugStr);
					SpeedrunTimer.Instance.LevelStart(level, beatStartTime);
				}

				if (ModLoader.Settings.ILMode)
				{
					SpeedrunTimer.Instance.StartTimer(beatStartTime);
				}

				Debug.Log($"GlobalBeatStarted: added {beatStartTime}ms to timer\n"
					+ DebugBeatListener.DebugStr);
			}
		}
	}
}
