using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] bool isPlaceable;
	[SerializeField] Tower towerPrefab;

	public bool IsPlaceable { get { return isPlaceable;  } }

	GridManager gridManager;
	Pathfinder pathfinder;
	Vector2Int coordinates = new Vector2Int();
	private float width;
	private float height;
	void Awake()
	{
		width = (float)Screen.width / 2.0f;
		height = (float)Screen.height / 2.0f;
		gridManager = FindObjectOfType<GridManager>();
		pathfinder = FindObjectOfType<Pathfinder>();
	}

	void Start()
	{
		if (gridManager != null)
		{
			coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
			if (!isPlaceable)
			{
				gridManager.BlockNode(coordinates);
			}
		}
	}
	private void OnMouseDown()
	{
		if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
		{
			bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
			if (isSuccessful)
			{
				gridManager.BlockNode(coordinates);
				pathfinder.NotifyReceivers();
			}
		}
	}
	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Ended)
			{
				Debug.Log("dlgmlals3 : touch " + touch.position.x + " " + touch.position.y + " coordinate : "
					+ coordinates.x + " " + coordinates.y);
			}
		}
	}

}
