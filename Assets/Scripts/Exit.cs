using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class Exit : MonoBehaviour, TriggerOnMove {

		public void Trigger(GameObject actor) {
			Player player = actor.GetComponent<Player>();

			if (player) {
				Debug.LogWarning("VICTORY");
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			} 
		}

	}
}
