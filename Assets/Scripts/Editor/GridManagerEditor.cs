using UnityEngine;
using UnityEditor;

namespace LD39 {
	[CustomEditor(typeof(GridManager))]
	public class GridManagerEditor : Editor {

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			DrawGridBlueprint();
		}

		private void DrawGridBlueprint() {
			GridManager gridManager = (GridManager) target;

			// Initialize to an empty grid
			if (gridManager.blueprint == null) {
				gridManager.blueprint = new MultiDimensionalString[] {
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
					new MultiDimensionalString { rows = new string[] {"", "", "", "", "", "", "", "", "", ""} },
				};
			}

			GUILayout.Label("Blueprint");
			GUILayout.BeginHorizontal();
			for (int x = 0; x < gridManager.blueprint.Length; x++) {
				GUILayout.BeginVertical();
				for (int y = 0; y < gridManager.blueprint[0].rows.Length; y++) {
					gridManager.blueprint[x].rows[y] = GUILayout.TextField(
						gridManager.blueprint[x].rows[y]
					);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

			if (Application.isPlaying) {
				GUILayout.Label("Grid");
				GUILayout.BeginHorizontal();
				for (int x = 0; x < gridManager.grid.Length; x++) {
					GUILayout.BeginVertical();
					for (int y = 0; y < gridManager.grid[0].rows.Length; y++) {
						GUI.enabled = false;
						var value = "";
						if (gridManager.grid[x].rows[y].content != null) {
							value = gridManager.grid[x].rows[y].content.name;
						}
						GUILayout.TextField(value);
						GUI.enabled = true;
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
			}

		}
	}
}
