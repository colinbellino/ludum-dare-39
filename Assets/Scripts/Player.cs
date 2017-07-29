using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class Player : MonoBehaviour {
		void Awake() {
			ChargeUser.OnChargeDepleted += OnChargeDepleted;
			GridManager.OnGridGenerated += OnGridGenerated;
		}

		void OnDisable() {
			ChargeUser.OnChargeDepleted -= OnChargeDepleted;
			GridManager.OnGridGenerated -= OnGridGenerated;
		}

		void OnChargeDepleted(GameObject source) {
			Debug.LogError("DEFEAT");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		void OnGridGenerated(int numberOfCharges) {
			var chargeUser = GetComponent<ChargeUser>();
			if (chargeUser != null) {
				chargeUser.SetCharges(numberOfCharges);
			}
		}
	}
}
