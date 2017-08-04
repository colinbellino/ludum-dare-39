using UnityEngine;
using System.Collections;

namespace LD39 {
	public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {

		public static Instance instance;
		public bool isPersistant;

		public virtual void OnEnable() {
			if(isPersistant) {
				if(!instance) {
					instance = this as Instance;
				}
				else {
					DestroyObject(gameObject);
				}
				DontDestroyOnLoad(gameObject);
			}
			else {
				instance = this as Instance;
			}
		}
	}

}
