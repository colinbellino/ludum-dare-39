using UnityEngine;
using UnityEditor;

namespace LD39 {
	[CustomEditor(typeof(LevelManager))]
	public class LevelManagerEditor : UnityEditor.Editor {

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			DrawLevel();
		}

		void DrawLevel() {
			LevelManager levelManager = (LevelManager) target;

			GUILayout.Label("Current level");
			Level currentLevel = levelManager.GetCurrentLevel();

			if (currentLevel != null) {
				var rows = currentLevel.rows;

				GUILayout.BeginHorizontal();
				for (int x = 0; x < rows.Length; x++) {
					GUILayout.BeginVertical();
					for (int y = 0; y < rows[0].columns.Length; y++) {
						GUILayout.TextField(rows[x].columns[y]);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
			} else {
				GUILayout.Label("No level loaded.");
			}
		}

	}
}
