using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class UIManager : MonoBehaviour {

		[SerializeField] string titleScreenSceneName;
		[SerializeField] UI.Charge chargeUI;
		[SerializeField] UI.TitleScreen titleScreenUI;
		[SerializeField] UI.LevelSelect levelSelectUI;
		[SerializeField] UI.EndScreen endScreenUI;

		void Awake() {
			ChargeUser.OnUpdateCharge += UpdateCharge;
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
			GameManager.OnGameWin += OnGameWin;
		}

		void OnDestroy() {
			ChargeUser.OnUpdateCharge -= UpdateCharge;
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
			GameManager.OnGameWin -= OnGameWin;
		}

		void OnGameWin() {
			ShowEndScreen();
		}

		void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
			if (scene.name == titleScreenSceneName) {
				titleScreenUI.Show();
				levelSelectUI.Hide();
				chargeUI.Hide();
				endScreenUI.Hide();
			} else {
				levelSelectUI.Hide();
				titleScreenUI.Hide();
				chargeUI.Show();
				endScreenUI.Hide();
			}
		}

		void UpdateCharge(int numberOfCharge) {
			chargeUI.SetValue(numberOfCharge);
		}

		public void ShowTitleScreen() {
			titleScreenUI.Show();
			levelSelectUI.Hide();
			chargeUI.Hide();
			endScreenUI.Hide();
		}

		public void ShowEndScreen() {
			titleScreenUI.Hide();
			levelSelectUI.Hide();
			chargeUI.Hide();
			endScreenUI.Show();
		}

		public void ShowLevelSelect() {
			titleScreenUI.Hide();
			levelSelectUI.Show();
			chargeUI.Hide();
			endScreenUI.Hide();
		}

		public void HideLevelSelect() {
			levelSelectUI.Hide();
			titleScreenUI.Show();
			chargeUI.Hide();
			endScreenUI.Hide();
		}

	}
}
