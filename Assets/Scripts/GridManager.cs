using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LD39 {
	public class GridManager : MonoBehaviour {

		public delegate void GridGenerated(int charges);
		public static event GridGenerated OnGridGenerated;

		[HideInInspector] public MultiDimensionalCell[] grid;

		[SerializeField] GameObject cellObject;
		[SerializeField] GameObject cellOverlay;
		[SerializeField] [HideInInspector] GameObject gridContainer;

		Dictionary<string, string> contentDictionary = new Dictionary<string, string> {
			{ "p", "Player" },
			{ "e", "Exit" },
			{ "c", "Charge" },
			{ "r", "Rock" },
		};

		void OnEnable() {
			CreateGridContainer();
		}

		void OnDisable() {
			DestroyGridContainer();
		}

		void Update () {
			RenderGrid();
		}

		public void GenerateGrid(Level level) {
			grid = new MultiDimensionalCell[level.rows.Length];

			for (int x = 0; x < level.rows.Length; x++) {
				grid[x] = new MultiDimensionalCell{
					columns = new Cell[level.rows[0].columns.Length]
				};

				for (int y = 0; y < level.rows[0].columns.Length; y++) {
					Cell cell = new Cell {
						x = x,
						y = y,
					};
					grid[x].columns[y] = cell;
					InstanciateCell(cell, level.rows[x].columns[y]);
				}
			}

			if (OnGridGenerated != null) {
				OnGridGenerated(level.charges);
			}
		}

		public void DestroyGrid() {
			grid = null;

			foreach (Transform child in gridContainer.transform) {
				GameObject.Destroy(child.gameObject);
			}
		}

		public bool MoveInDirection(GameObject content, Direction direction) {
			bool wasAbleToMove = false;
			var startCell = GetCellByContent(content);
			var endCell = GetCellByDirection(startCell, direction);

			if (!CanMoveThere(endCell)) { return false; }

			if (endCell.content) {
				var triggerOnMove = endCell.content.GetComponent<TriggerOnMove>();
				if (triggerOnMove != null) {
					triggerOnMove.Trigger(startCell.content);
					wasAbleToMove = true;
				}
			} else {
				wasAbleToMove = true;
			}

			// TODO: Move the content instead of destroying / creating it
			SetCellContent(startCell, null);
			SetCellContent(endCell, content);

			return wasAbleToMove;
		}

		void CreateGridContainer() {
			GameObject root = new GameObject();
			root.name = "GridContainer";

			gridContainer = root;
		}

		void DestroyGridContainer() {
			Destroy(gridContainer);
			gridContainer = null;
		}

		void RenderGrid() {
			if (grid.Length == 0 || grid[0].columns.Length == 0) { return; }

			foreach (var rows in grid) {
				foreach (var cell in rows.columns) {
					RenderCellContent(cell);
				}
			}
		}

		void InstanciateCell(Cell cell, string contentKey) {
			// Generate root
			Vector3 pos = new Vector3(cell.x, 0f, cell.y);
			GameObject root = Instantiate(cellObject, pos, Quaternion.identity);
			root.transform.SetParent(gridContainer.transform);
			root.name = "Cell (" + cell.x + "-" + cell.y + ")";

			// Generate overlay
			GameObject overlay = Instantiate(cellOverlay, root.transform);

			// Generate content
			if (contentKey != " ") {
				string resourceName;
				contentDictionary.TryGetValue(contentKey, out resourceName);

				var res = Resources.Load(resourceName, typeof(GameObject));
				if (!res) {
					throw new UnityException("Could not load resource: " + resourceName);
				}

				GameObject content = Instantiate(res, root.transform) as GameObject;
				cell.content = content;
			}

			// Update the cell ref
			cell.root = root;
			cell.overlay = overlay;
		}

		void RenderCellContent(Cell cell) {
			// If the cell has no content
			if (!cell.content) {
				if (!cell.lastContent) { return; }

				// If we have no content to destroy, stop here
				Transform existingContent = cell.root.transform.Find(cell.lastContent.name);
				if (!existingContent) { return; }

				Destroy(existingContent.gameObject);
			}

			// If the cell has content
			if (cell.content) {
				Transform existingContent = cell.root.transform.Find(cell.content.name);
				// If we already created the content, stop here
				if (!existingContent) {
					// Instanciate the content object
					GameObject newContent = Instantiate(cell.content, cell.root.transform);
					newContent.name = cell.content.name;
					cell.content = newContent;
				}

				// Destroy the content form the last frame
				if (cell.lastContent && cell.lastContent.name != cell.content.name) {
					Destroy(cell.lastContent.gameObject);
				}
			}
		}

		Cell SetCellContent(Cell cell, GameObject content) {
			cell.lastContent = cell.content;
			cell.content = content;
			return cell;
		}

		Cell GetCellByContent(GameObject content) {
			foreach (var column in grid) {
				foreach (var cell in column.columns) {
					if (cell.content == content) {
						return cell;
					}
				}
			}
			return null;
		}

		bool CanMoveThere(Cell cell) {
			if (cell == null) { return false; }

			if (cell.content != null) {
				return IsTrigger(cell);
			}

			return true;
		}

		bool IsTrigger(Cell cell) {
			var isTrigger = cell.content.GetComponent<TriggerOnMove>();
			return isTrigger != null;
		}

		Cell GetCellByDirection(GameObject content, Direction direction) {
		 	return GetCellByDirection(GetCellByContent(content), direction);
		}

		// TODO: Don't destroy and re-instance the content, this has a high performance cost :(
		Cell GetCellByDirection(Cell cell, Direction direction) {
			if (cell == null) { return null; }

			switch (direction) {
				case Direction.Up:
					if (cell.x < grid[0].columns.Length - 1) {
						return grid[cell.x + 1].columns[cell.y];
					}
					break;
				case Direction.Right:
					if (cell.y > 0) {
						return grid[cell.x].columns[cell.y - 1];
					}
					break;
				case Direction.Down:
					if (cell.x > 0) {
						return grid[cell.x - 1].columns[cell.y];
					}
					break;
				case Direction.Left:
					if (cell.y < grid.Length - 1) {
						return grid[cell.x].columns[cell.y + 1];
					}
					break;
			}
			return cell;
		}
	}

	[System.Serializable]
	public class Cell {
		public int x;
		public int y;
		public GameObject root;
		public GameObject overlay;
		public GameObject content;
		public GameObject lastContent;
	}

	[System.Serializable]
	public class MultiDimensionalCell {
		public Cell[] columns;
	}
}
