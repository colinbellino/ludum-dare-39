using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class Recharge : MonoBehaviour, TriggerOnMove {

		[SerializeField]
		int charge = 3;

		public void Trigger(GameObject actor) {
			Player player = actor.GetComponent<Player>();

			if (player) {
				var chargeUser = player.GetComponent<ChargeUser>();
				if (chargeUser) {
					chargeUser.Recharge(charge);
				}
			}
		}

	}
}
