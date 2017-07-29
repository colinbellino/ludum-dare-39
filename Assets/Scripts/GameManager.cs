using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace LD39 {
	public class GameManager : MonoBehaviour {

		string[] levels = new string[] {"Level1", "Level2"};

		void Awake () {
			DontDestroyOnLoad(this);

		}

		void GetFirstLevel() {
			SceneManager.LoadScene(levels[0]);
		}

		public void GetNextLevel(string level) {
			// string next = levels[]
			int index = System.Array.IndexOf(levels, level);
			if ( index !=-1 && index +1 < levels.Length) {
				SceneManager.LoadScene(levels[index+1]);
			}
		}

	}
}
