using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class Exit : MonoBehaviour, TriggerOnMove {

		GameManager gameManager;

		void OnEnable() {
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			if (gameManager == null) {
				throw new UnityException("Could not find GameManager");
			}
		}

		public void Trigger(GameObject actor) {
			Player player = actor.GetComponent<Player>();

			if (player) {
				if (gameManager) {
					gameManager.LoadNextLevel();
				}
			}
		}

	}
}
