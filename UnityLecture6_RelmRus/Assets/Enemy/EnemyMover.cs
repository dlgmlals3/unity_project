using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();
    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;
    Ending ending;
    void OnEnable()
    {
        returnToStart();
        RecalculatePath(true);

    }

	void Awake()
	{
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
        ending = FindObjectOfType<Ending>();
    }

    
    void RecalculatePath(bool resetPath)
	{
        Vector2Int coordinate = new Vector2Int();
        if (resetPath)
		{
            coordinate = pathfinder.StartCoordinate;
		} else
		{
            coordinate = gridManager.GetCoordinatesFromPosition(transform.position);
		}
        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinate);
        StartCoroutine(FollowPath());
    }

	private void returnToStart()
	{
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinate);
    }
    private void finishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
        ending.goEnding(enemy.getGold());
    }

    IEnumerator FollowPath()
	{
        for (int i=1; i<path.Count; i++) {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        finishPath();
	}


}
