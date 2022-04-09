using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
	[SerializeField] Vector2Int startCoordinate;
	public Vector2Int StartCoordinate {  get { return startCoordinate;  } }
	[SerializeField] Vector2Int destinationCoordinates;
	public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }
    
	Node startNode;
	Node destinationNode;
	Node currentSearchNode;

	Queue<Node> frontier = new Queue<Node>();
	Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

	Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
	Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
	GridManager gridManager;

	void Awake()
	{
        gridManager = FindObjectOfType<GridManager>();
		if (gridManager != null)
		{
			grid = gridManager.Grid;
			startNode = grid[startCoordinate];
			destinationNode = grid[destinationCoordinates];

		}
	}

	void Start()
    {
		GetNewPath();
	}

	public List<Node> GetNewPath()
	{
		gridManager.ResetNodes();
		BreadFirstSearch(startCoordinate);
		return BuildPath();
	}
	public List<Node> GetNewPath(Vector2Int coordinate)
	{
		gridManager.ResetNodes();
		BreadFirstSearch(coordinate);
		return BuildPath();
	}
	private void ExploreNeightbors()
	{
		List<Node> neighbors = new List<Node>();
		foreach (Vector2Int direction in directions)
		{
			Vector2Int neighborCoords = currentSearchNode.coordinates + direction;
			if (grid.ContainsKey(neighborCoords))
			{
				neighbors.Add(grid[neighborCoords]);
			}
		}

		foreach (Node neighbor in neighbors)
		{
			if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
			{
				neighbor.connectedTo = currentSearchNode;
				reached.Add(neighbor.coordinates, neighbor);
				frontier.Enqueue(neighbor);
			}
		}
	}

	void BreadFirstSearch(Vector2Int coordinate)
	{
		startNode.isWalkable = true;
		destinationNode.isWalkable = true;

		frontier.Clear();
		reached.Clear();

		bool isRunning = true;
		frontier.Enqueue(grid[coordinate]);
		reached.Add(coordinate, grid[coordinate]);

		while(frontier.Count > 0 && isRunning)
		{
			currentSearchNode = frontier.Dequeue();
			currentSearchNode.isExplored = true;
			ExploreNeightbors();
			if (currentSearchNode.coordinates == destinationCoordinates)
			{
				isRunning = false;
			}
		}
	}
	List<Node> BuildPath()
	{
		List<Node> path = new List<Node>();
		Node currentNode = destinationNode;
		path.Add(currentNode);
		currentNode.isPath = true;

		while (currentNode.connectedTo != null)
		{
			currentNode = currentNode.connectedTo;
			path.Add(currentNode);
			currentNode.isPath = true;
		}
		path.Reverse();

		return path;
	}

	public bool WillBlockPath(Vector2Int coordinates)
	{
		if (grid.ContainsKey(coordinates))
		{
			bool previousState = grid[coordinates].isWalkable;
			grid[coordinates].isWalkable = false;
			List<Node> newPath = GetNewPath();
			grid[coordinates].isWalkable = previousState;

			if (newPath.Count <= 1)
			{
				GetNewPath();
				return true;
			}
		}
		return false;
	}

	public void NotifyReceivers()
	{
		BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
	}
}
