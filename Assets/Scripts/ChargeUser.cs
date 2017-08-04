using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class ChargeUser : MonoBehaviour {

		[SerializeField] [HideInInspector] int charges;

		public delegate void UpdateCharge(int numberOfCharge);
		public static event UpdateCharge OnUpdateCharge;

		public delegate void ChargeDepleted(GameObject source);
		public static event ChargeDepleted OnChargeDepleted;

		void OnEnable() {
			if (OnUpdateCharge != null) {
				OnUpdateCharge(charges);
			}
		}

		public int Consume(int cost) {
			charges -= cost;

			if (OnUpdateCharge != null) {
				OnUpdateCharge(charges);
			}

			CheckRemainingCharges();

			return charges;
		}

		public int Recharge(int cost) {
			charges += cost;

			if (OnUpdateCharge != null) {
				OnUpdateCharge(charges);
			}

			CheckRemainingCharges();

			return charges;
		}

		public int SetCharges(int charges) {
			this.charges = charges;

			if (OnUpdateCharge != null) {
				OnUpdateCharge(charges);
			}

			return this.charges;
		}

		public bool HasEnough(int cost) {
			return charges >= cost;
		}

		void CheckRemainingCharges() {
			if (charges <= 0) {
				if (OnChargeDepleted != null) {
					OnChargeDepleted(gameObject);
				}
			}
		}
	}
}
