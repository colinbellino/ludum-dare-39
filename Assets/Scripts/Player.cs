using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class Player : MonoBehaviour {

		GameManager gameManager;

		void Awake() {
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			if (gameManager == null) {
				throw new UnityException("Could not find GameManager");
			}

			GridManager.OnGridGenerated += OnGridGenerated;
			ChargeUser.OnChargeDepleted += OnChargeDepleted;
		}

		void OnDestroy() {
			GridManager.OnGridGenerated -= OnGridGenerated;
			ChargeUser.OnChargeDepleted -= OnChargeDepleted;
		}

		void OnGridGenerated(int numberOfCharges) {
			var chargeUser = GetComponent<ChargeUser>();
			if (chargeUser != null) {
				chargeUser.SetCharges(numberOfCharges);
			}
		}

		void OnChargeDepleted(GameObject source) {
			if (gameManager != null) {
				gameManager.GameOver();
			}
		}
	}
}
