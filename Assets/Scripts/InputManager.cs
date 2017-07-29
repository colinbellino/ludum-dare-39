using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class InputManager : MonoBehaviour {

		public delegate void MoveAction(Direction direction);
		public static event MoveAction OnMove;

		public delegate void Action(InputAction action);
		public static event Action OnAction;

		// Update is called once per frame
		void Update () {
			if (Input.GetKeyDown("up") && OnMove != null) {
				OnMove(Direction.Up);
			}
			else if (Input.GetKeyDown("right") && OnMove != null) {
				OnMove(Direction.Right);
			}
			else if (Input.GetKeyDown("down") && OnMove != null) {
				OnMove(Direction.Down);
			}
			else if (Input.GetKeyDown("left") && OnMove != null) {
				OnMove(Direction.Left);
			}
			else if (Input.GetButtonDown("Fire1") && OnAction != null) {
				OnAction(InputAction.Fire1);
			}
			else if (Input.GetButtonDown("Fire2") && OnAction != null) {
				OnAction(InputAction.Fire2);
			}
		}
	}
}