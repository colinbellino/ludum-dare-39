using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39.UI {
	public class EndScreen : MonoBehaviour {

		GameManager gameManager;

		[SerializeField] GameObject wrapper;
		bool isVisible = true;

		void OnEnable() {
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			if (gameManager == null) {
				throw new UnityException("Could not find GameManager");
			}
		}

		public void Toggle(float backgroundAlpha = 1f) {
			if (isVisible) {
				Hide();
			} else {
				Show();
			}
		}

		public void Show() {
			wrapper.SetActive(true);
			gameObject.GetComponentInChildren<Button>().Select();
		}

		public void Hide() {
			wrapper.SetActive(false);
		}

	}
}
