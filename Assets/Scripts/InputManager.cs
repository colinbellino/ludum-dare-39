using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class InputManager : MonoBehaviour {

		public delegate void MoveAction(Direction direction);
		public static event MoveAction OnMove;

		public delegate void Action(InputAction action);
		public static event Action OnAction;

		Repeater horizontalRepeater = new Repeater("Horizontal");
		Repeater verticalRepeater = new Repeater("Vertical");

		void Update () {
			int x = horizontalRepeater.Update();
			int y = verticalRepeater.Update();

			if (y > 0 && OnMove != null) {
				OnMove(Direction.Up);
			}
			else if (x > 0 && OnMove != null) {
				OnMove(Direction.Right);
			}
			else if (y < 0 && OnMove != null) {
				OnMove(Direction.Down);
			}
			else if (x < 0 && OnMove != null) {
				OnMove(Direction.Left);
			}
			else if (Input.GetButtonDown("Submit") && OnAction != null) {
				OnAction(InputAction.Submit);
			}
			else if (Input.GetButtonDown("Cancel") && OnAction != null) {
				OnAction(InputAction.Cancel);
			}
			else if (Input.GetButtonDown("Start") && OnAction != null) {
				OnAction(InputAction.Start);
			}
		}
	}
}
