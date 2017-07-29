using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class TitleScreen : MonoBehaviour {

		[SerializeField] Image backgroundImage;
		[SerializeField] Button startButton;
		[SerializeField] Button exitButton;

		public void ShowTitleScreen() {

		}

		public static void LoadFirstLevel() {
			SceneManager.LoadScene("Level1");
		}
	}
}
