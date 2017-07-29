using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD39 {
	public class AutoRotate : MonoBehaviour {

		[SerializeField]
		Vector3 speed = new Vector3(1, 0, 0);

		void FixedUpdate() {
			transform.Rotate(
				speed.x * Time.deltaTime,
				speed.y * Time.deltaTime,
				speed.z * Time.deltaTime
			);
		}
	}
}
