using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class CanAct : MonoBehaviour {

		void OnEnable() {
			InputManager.OnAction += Action;
		}

		void OnDisable() {
			InputManager.OnAction -= Action;
		}

		void Action(InputAction action) {

		}
	}
}
