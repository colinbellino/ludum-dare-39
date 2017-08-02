using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class Movement : MonoBehaviour {

		int moveCost = 1;
		GridManager gridManager;
		ChargeUser chargeUser;

		void Awake() {
			Init();

			InputManager.OnMove += OnMove;
		}

		void OnDestroy() {
			InputManager.OnMove -= OnMove;
		}

		void Init() {
			GameObject levelManager = GameObject.Find("GameManager");
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
			var wasAbleToMove = gridManager.MoveInDirection(gameObject, direction);

			if (wasAbleToMove) {
				// Consume a charge
				if (chargeUser != null) {
					chargeUser.Consume(moveCost);
				}
			}
		}
	}
}
