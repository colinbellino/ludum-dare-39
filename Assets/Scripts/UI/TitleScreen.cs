using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39.UI {
	public class TitleScreen : MonoBehaviour {

		public delegate void VisibilityChanged(bool isVisible);
		public static event VisibilityChanged OnVisibilityChanged;

		[SerializeField] GameObject wrapper;
		[SerializeField] Text titleText;
		[SerializeField] Image backgroundImage;
		[SerializeField] Button startButton;
		[SerializeField] Button levelSelectButton;
		[SerializeField] Button exitButton;

		GameManager gameManager;
		bool isVisible = false;

		void Awake() {
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			if (gameManager == null) {
				throw new UnityException("Could not find GameManager");
			}

			InputManager.OnAction += OnAction;
		}

		void OnDestroy() {
			InputManager.OnAction -= OnAction;
		}

		void OnAction(InputAction action) {
			// When pressing Start
			if (action == InputAction.Start) {
				if (!gameManager.IsTitleScreen()) {
					Toggle();
				}
			}

			if (action == InputAction.Cancel && isVisible) {
				gameManager.ExitGame();
			}
		}

		public void Toggle() {
			if (isVisible) {
				Hide();
			} else {
				Show();
			}
		}

		public void Show(float backgroundAlpha = 1f) {
			isVisible = true;
			wrapper.SetActive(true);

			gameObject.GetComponentInChildren<Button>().Select();

			if (OnVisibilityChanged != null) {
				OnVisibilityChanged(isVisible);
			}
		}

		public void Hide() {
			isVisible = false;
			wrapper.SetActive(false);

			if (OnVisibilityChanged != null) {
				OnVisibilityChanged(isVisible);
			}
		}

	}
}
