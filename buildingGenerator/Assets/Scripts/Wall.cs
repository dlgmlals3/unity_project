using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public enum WallType
	{
		Normal,
		Blank
	}
	public WallType WallTypeSelected { get; private set; } = WallType.Normal;

	public Vector3 Position { get; private set; }

	public Quaternion Rotation { get; private set; }

	public Wall(Vector3 position, Quaternion rotation, WallType wallType = WallType.Normal)
	{
		Position = position;
		Rotation = rotation;
		WallTypeSelected = wallType;
	}
}
