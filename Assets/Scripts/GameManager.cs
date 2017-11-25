using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class GameManager : MonoBehaviour {

		public delegate void GameWin();
		public static event GameWin OnGameWin;

		public delegate void GameLose();
		public static event GameLose OnGameLose;

		public static GameManager instance;
		[SerializeField] List<string> levelNames;
		bool isLoadingLevel = false;

		void Awake () {
			DontDestroyOnLoad(this);
			if(!instance) {
				instance = this;
			} else {
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);

			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		void OnDestroy() {
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		}

		void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
			isLoadingLevel = false;
		}

		public void LoadFirstLevel() {
			SceneManager.LoadScene(levelNames[0]);
		}

		public void LoadNextLevel() {
			Scene activeScene = SceneManager.GetActiveScene();
			int activeIndex = levelNames.FindIndex(
				(levelName) => levelName == activeScene.name
			);
			int nextIndex = activeIndex + 1;

			if (nextIndex < levelNames.Count) {
				LoadLevel(levelNames[nextIndex]);
			} else {
				if (OnGameWin != null) {
					OnGameWin();
					isLoadingLevel = true; // Dirty hack
				}
			}
		}

		public void LoadLevel(string levelName) {
			if (isLoadingLevel) {
				Debug.LogWarning("Tried to load a level when one was already loading");
				return;
			}
			isLoadingLevel = true;
			SceneManager.LoadScene(levelName);
		}

		public void ExitGame() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}

		public void GameOver() {
			LoadLevel(SceneManager.GetActiveScene().name);
		}

		public bool IsTitleScreen() {
			return SceneManager.GetActiveScene().name == "TitleScreen";
		}
	}
}
