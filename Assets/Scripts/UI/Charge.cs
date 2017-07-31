using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39.UI {
	public class Charge : MonoBehaviour {

		[SerializeField] GameObject wrapper;
		[SerializeField] Image iconImage;
		[SerializeField] Image[] barImages;

		public void Show() {
			wrapper.SetActive(true);
		}

		public void Hide() {
			wrapper.SetActive(false);
		}

		public void SetValue(int value) {
			for (int i = 0; i < barImages.Length; i++) {
				barImages[i].gameObject.SetActive(i < value);
			}
		}
	}
}
