using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LD39 {
	public class GridManager : MonoBehaviour {

		public delegate void GridGenerated(int charges);
		public static event GridGenerated OnGridGenerated;

		public MultiDimensionalString[] blueprint;
		public MultiDimensionalCell[] grid;

		bool blueprintIsValid = false;
		[SerializeField] int charges;
		[SerializeField] GameObject cellObject;
		[SerializeField] GameObject cellOverlay;

		Dictionary<string, string> contentDictionary = new Dictionary<string, string> {
			{ "p", "Player" },
			{ "e", "Exit" },
			{ "c", "Charge" },
			{ "r", "Rock" },
		};

		void Start () {
			CheckBlueprintValidity();

			if (blueprintIsValid) {
				GenerateGrid();
			}
		}

		void Update () {
			if (blueprintIsValid) {
				UpdateGrid();
			}
		}

		void CheckBlueprintValidity() {
			bool hasPlayer = false;
			bool hasExit = false;

			if (charges <= 0) {
				throw new UnityException("Invalid number of charges!");
			}

			for (int x = 0; x < blueprint.Length; x++) {
				for (int y = 0; y < blueprint[0].rows.Length; y++) {
					if (blueprint[x].rows[y] == "p") {
						hasPlayer = true;
					}
					if (blueprint[x].rows[y] == "e") {
						hasExit = true;
					}
				}
			}

			if (!hasPlayer) {
				throw new UnityException("The blueprint has no PLAYER spawn point!");
			}
			if (!hasExit) {
				throw new UnityException("The blueprint has no EXIT!");
			}

			blueprintIsValid = true;
		}

		void GenerateGrid() {
			grid = new MultiDimensionalCell[blueprint.Length];

			for (int x = 0; x < blueprint.Length; x++) {
				grid[x] = new MultiDimensionalCell{
					rows = new Cell[blueprint[0].rows.Length]
				};

				for (int y = 0; y < blueprint[0].rows.Length; y++) {
					Cell cell = new Cell {
						x = x,
						y = y,
					};
					grid[x].rows[y] = cell;
					InstanciateCell(cell, blueprint[x].rows[y]);
				}
			}

			if (OnGridGenerated != null) {
				OnGridGenerated(charges);
			}
		}

		void UpdateGrid() {
			if (grid.Length == 0) { return; }

			foreach (var column in grid) {
				foreach (var cell in column.rows) {
					RenderCellContent(cell);
				}
			}
		}

		void InstanciateCell(Cell cell, string contentKey) {
			// Generate root
			Vector3 pos = new Vector3(cell.x, 0f, cell.y);
			GameObject root = Instantiate(cellObject, pos, Quaternion.identity);
			root.name = "Cell (" + cell.x + "-" + cell.y + ")";

			// Generate overlay
			GameObject overlay = Instantiate(cellOverlay, root.transform);

			// Generate content
			if (contentKey != "") {
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
			// If the cell has no content
			else {
				if (!cell.lastContent) { return; }

				// If we have no content to destroy, stop here
				Transform existingContent = cell.root.transform.Find(cell.lastContent.name);
				if (!existingContent) { return; }

				Destroy(existingContent.gameObject);
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

		Cell SetCellContent(Cell cell, GameObject content) {
			cell.lastContent = cell.content;
			cell.content = content;
			return cell;
		}

		Cell GetCellByContent(GameObject content) {
			foreach (var column in grid) {
				foreach (var cell in column.rows) {
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
					if (cell.y < grid.Length - 1) {
						return grid[cell.x].rows[cell.y + 1];
					}
					break;
				case Direction.Right:
					if (cell.x < grid[0].rows.Length - 1) {
						return grid[cell.x + 1].rows[cell.y];
					}
					break;
				case Direction.Down:
					if (cell.y > 0) {
						return grid[cell.x].rows[cell.y - 1];
					}
					break;
				case Direction.Left:
					if (cell.x > 0) {
						return grid[cell.x - 1].rows[cell.y];
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
	public class MultiDimensionalString {
		public string[] rows;
	}

	[System.Serializable]
	public class MultiDimensionalCell {
		public Cell[] rows;
	}
}
