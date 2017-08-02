using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LD39 {
	public class LevelManager : MonoBehaviour {

		[SerializeField] GridManager gridManager;

		[SerializeField] List<string> levelNames;
		[HideInInspector] [SerializeField] public string currentLevelName;
		[HideInInspector] [SerializeField] Dictionary<string, Level> levels = new Dictionary<string, Level>();

		public void NextLevel() {
			int activeIndex = levelNames.FindIndex(
				(levelName) => levelName == currentLevelName
			);
			bool isLastLevel = activeIndex == (levelNames.Count - 1);

			if (isLastLevel) {
				Debug.Log("OnGameWin");
				// if (OnGameWin != null) {
				// 	OnGameWin();
				// }
				return;
			}

			int nextIndex = activeIndex + 1;
			ChangeLevel(levelNames[nextIndex]);
		}

		public void ChangeLevel(string levelName) {
			var currentLevel = GetCurrentLevel();

			// Destroy the current level
			if (currentLevel != null) {
				gridManager.DestroyGrid();
			}

			// Import the level only if it's not already loaded
			if (!levels.ContainsKey(levelName)) {
				ImportLevel(levelName);
			}

			currentLevelName = levelName;

			var nextLevel = GetLevelByName(levelName);
			gridManager.GenerateGrid(nextLevel);
		}

		public Level GetCurrentLevel() {
			return GetLevelByName(currentLevelName);
		}

		public Level GetLevelByName(string levelName) {
			if (levelName == "") {
				return null;
			}

			Level level;
			levels.TryGetValue(levelName, out level);

			return level;
		}

		void ImportLevel(string levelName) {
			string levelsPath = Path.Combine(Application.streamingAssetsPath, "Levels");
			string filePath = Path.Combine(levelsPath, levelName + ".json");

			if(!File.Exists(filePath)) {
				Debug.LogError("Couldn't import level: " + filePath);
				return;
			}

			string dataAsJson = File.ReadAllText(filePath);
			Level level = JsonUtility.FromJson<Level>(dataAsJson);

			if (IsLevelValid(level)) {
				levels.Add(levelName, level);
			}
		}

		bool IsLevelValid(Level level) {
			bool hasPlayer = false;
			bool hasExit = false;

			if (level.charges <= 0) {
				Debug.LogError("Invalid number of charges!");
				return false;
			}

			for (int x = 0; x < level.rows.Length; x++) {
				for (int y = 0; y < level.rows[0].columns.Length; y++) {
					if (level.rows[x].columns[y] == "p") {
						hasPlayer = true;
					}
					if (level.rows[x].columns[y] == "e") {
						hasExit = true;
					}
				}
			}

			if (!hasPlayer) {
				Debug.LogError("The blueprint has no PLAYER spawn point!");
				return false;
			}
			if (!hasExit) {
				Debug.LogError("The blueprint has no EXIT!");
				return false;
			}

			return true;
		}
	}

	[System.Serializable]
	public class Level {
		public int charges;
		public MultiDimensionalString[] rows;
	}

	[System.Serializable]
	public class MultiDimensionalString {
		public string[] columns;
	}

}
