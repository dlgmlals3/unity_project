using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor : MonoBehaviour
{
	public int FloorNumber { get; private set; }
	[SerializeField]
	public Room[,] rooms;
	/*
	[0, 0, 0]
	[0, 0, 0]
	[0, 0, 0]


	 */
	public Floor(int floorNumber, Room[,] rooms)
	{
		FloorNumber = floorNumber;
		this.rooms = rooms;
	}
}
