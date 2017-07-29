using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class UIManager : MonoBehaviour {

		[SerializeField] Text chargeText;
		[SerializeField] GameObject titleScreen;

		void OnEnable() {
			ChargeUser.OnUpdateCharge += UpdateCharge;
		}

		void OnDisable() {
			ChargeUser.OnUpdateCharge -= UpdateCharge;
		}

		void Awake() {
			if (SceneManager.GetActiveScene().name == "TitleScreen") {
				titleScreen.SetActive(true);
			}
		}

		void UpdateCharge(int numberOfCharge) {
			chargeText.text = numberOfCharge.ToString();
		}
	}
}
