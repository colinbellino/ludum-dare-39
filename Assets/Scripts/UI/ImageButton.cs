using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LD39.UI {
	public class ImageButton : MonoBehaviour {

		[SerializeField] Image activeButtonImage;

		void OnEnable() {
			OnDeselect();
		}

		void OnDisable() {
			OnDeselect();
		}

		public void OnSelect() {
			activeButtonImage.gameObject.SetActive(true);
		}

		public void OnDeselect() {
			activeButtonImage.gameObject.SetActive(false);
		}

	}
}
