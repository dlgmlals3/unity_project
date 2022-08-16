using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProGen : MonoBehaviour
{
	[SerializeField]
	private ProGenThemeScriptableObject theme;

	private void Awake() => Generate();

	private List<GameObject> rooms = new List<GameObject>();

	private int prefabCounter = 0;
	public ProGenThemeScriptableObject Theme => theme;

	public void Generate()
	{
		prefabCounter = 0;

		Clear();

		BuildDataStructure();

		Render();

		if (!theme.keepInsideWalls)
		{
			RemoveInsideWalls();
		}
	}

	public void BuildDataStructure()
	{
		theme.floors = new Floor[theme.numberOfFloors];
		int floorCount = 0;

		int initialRows = theme.rows;
		int initialColumns = theme.columns;

		foreach (Floor floor in theme.floors)
		{
			Room[,] rooms = new Room[initialRows, initialColumns];

			for (int row = 0; row < initialRows; row++)
			{
				for (int column = 0; column < initialColumns; column++)
				{
					var roomPosition = new Vector3(row * theme.cellUnitSize, floorCount, column * theme.cellUnitSize);
					rooms[row, column] = new Room(roomPosition, theme.includeRoof ? (floorCount == theme.floors.Length - 1) : false);
					rooms[row, column].Walls[0] = new Wall(roomPosition, Quaternion.Euler(0, 0, 0));
					rooms[row, column].Walls[1] = new Wall(roomPosition, Quaternion.Euler(0, 90, 0));
					rooms[row, column].Walls[2] = new Wall(roomPosition, Quaternion.Euler(0, 180, 0));
					rooms[row, column].Walls[3] = new Wall(roomPosition, Quaternion.Euler(0, -90, 0));

					if (theme.randomizeRows || theme.randomizeColumns)
					{
						rooms[row, column].HasRoof = true;
					}
				}
			}
			theme.floors[floorCount] = new Floor(floorCount, rooms);
			floorCount++;

			if (theme.randomizeRows)
			{
				initialRows = Random.Range(1, theme.rows);
			}
			if (theme.randomizeColumns)
			{
				initialColumns = Random.Range(1, theme.columns);
			}
		}
	}

	void Render()
	{
		foreach (Floor floor in theme.floors)
		{
			for (int row=0; row<floor.Rows; row++)
			{
				for (int column=0; column<floor.Columns; column++)
				{
					Room room = floor.rooms[row, column];
					room.FloorNumber = floor.FloorNumber;
					GameObject roomGo = new GameObject($"Room_{row}_{column}");
					rooms.Add(roomGo);
					roomGo.transform.parent = transform;

					if (floor.FloorNumber == 0)
					{
						RoomPlacement(Random.Range(0.0f, 1.0f) <= theme.doorPercentChance ? theme.doorPrefab : theme.wallPrefab, room, roomGo);
					}
					else
					{
						if (Random.Range(0.0f, 1.0f) <= theme.windowPercentChance)
						{
							if(theme.randomizeWindowSelection)
							{
								int windowIndex = Random.Range(0, theme.windowPrefabs.Length);
								RoomPlacement(theme.windowPrefabs[windowIndex], room, roomGo);
							} else
							{
								RoomPlacement(theme.windowPrefabs[0], room, roomGo);
							}
						} else
						{
							RoomPlacement(theme.wallPrefab, room, roomGo);
						}
					}	
				}
			}
		}
	}

	private void RoomPlacement(GameObject prefab, Room room, GameObject roomGo)
	{
		SpawnPrefab(prefab, roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
		SpawnPrefab(prefab, roomGo.transform, room.Walls[1].Position, room.Walls[1].Rotation);
		SpawnPrefab(prefab, roomGo.transform, room.Walls[2].Position, room.Walls[2].Rotation);
		SpawnPrefab(prefab, roomGo.transform, room.Walls[3].Position, room.Walls[3].Rotation);
		if (room.HasRoof)
		{
			// Rule if we neeed to randomize roof and we're at the top
			if (theme.randomizeRoofSelection && room.FloorNumber == theme.floors.Count() - 1)
			{
				int roofIndex = Random.Range(0, theme.roofPrefabs.Length);
				SpawnPrefab(theme.roofPrefabs[roofIndex], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
			} else
			{
				SpawnPrefab(theme.roofPrefabs[0], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
			}
		}
	}

	private void SpawnPrefab(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		var gameObject = Instantiate(prefab, position, rotation);
		gameObject.transform.parent = parent;
		gameObject.AddComponent<WallComponent>();
		gameObject.name = $"{gameObject.name}_{prefabCounter}";
		prefabCounter++;
	}

	void RemoveInsideWalls()
	{
		/*var wallComponents = GameObject.FindObjectOfType<WallComponent>();
		var childs = wallComponents.Select(cellUnitSize => c.transform.GetChild(0).position.ToString()).ToList();

		var dupPositions = childs.GroupBy(c => c)
			.Where(c => c.Count() > 1)
			.Select(grp => grp.Key)
			.ToList();

		foreach (WallComponent w in wallComponents)
		{
			var childTransform = w.transform.GetChild(0);
			if (dupPositions.Contatins(childTransform.position.ToString()))
			{
				DestroyImmediate(childTransform.gameObject);
			}
		}*/
	}

	void Clear()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			DestroyImmediate(rooms[i]);
		}
		rooms.Clear();
	}
}
