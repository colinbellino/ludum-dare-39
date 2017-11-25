using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class Movement : MonoBehaviour {

		int moveCost = 1;
		bool canMove = true;
		GridManager gridManager;
		ChargeUser chargeUser;

		void Awake() {
			Init();

			InputManager.OnMove += OnMove;
			UI.TitleScreen.OnVisibilityChanged += OnVisibilityChanged;
		}

		void OnDestroy() {
			InputManager.OnMove -= OnMove;
			UI.TitleScreen.OnVisibilityChanged -= OnVisibilityChanged;
		}

		void Init() {
			GameObject levelManager = GameObject.Find("LevelManager");
			if (levelManager == null) {
				throw new UnityException("Object LevelManager not found");
			}

			GridManager gridManager = levelManager.GetComponent<GridManager>();
			if (gridManager == null) {
				throw new UnityException("Component GridManager not found");
			}
			this.gridManager = gridManager;

			chargeUser = GetComponent<ChargeUser>();
		}

		void OnMove(Direction direction) {
			if (!canMove) { return; }

			var wasAbleToMove = gridManager.MoveInDirection(gameObject, direction);

			if (wasAbleToMove) {
				// Consume a charge
				if (chargeUser != null) {
					chargeUser.Consume(moveCost);
				}
			}
		}

		void OnVisibilityChanged(bool isVisible) {
			canMove = !isVisible;
		}
	}
}
