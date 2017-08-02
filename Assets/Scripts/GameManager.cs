using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class GameManager : Singleton<GameManager> {

		public delegate void GameStart();
		public static event GameStart OnGameStart;

		public delegate void GameWin();
		public static event GameWin OnGameWin;

		public delegate void GameLose();
		public static event GameLose OnGameLose;

		[SerializeField] LevelManager levelManager;

		public void LoadFirstLevel() {
			levelManager.NextLevel();

			if (OnGameStart != null) {
				OnGameStart();
			}
		}

		public void LoadNextLevel() {
			levelManager.NextLevel();
		}

		public void LoadLevel(string levelName) {
			levelManager.ChangeLevel(levelName);
		}

		public void ExitGame() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif

			Application.Quit();
		}

		// TODO: do something
		public void GameOver() {
			Debug.Log("GameOver");
			// LoadLevel(SceneManager.GetActiveScene().name);
		}

		// TODO: delete me
		public bool IsTitleScreen() {
			return SceneManager.GetActiveScene().name == "TitleScreen";
		}
	}
}
