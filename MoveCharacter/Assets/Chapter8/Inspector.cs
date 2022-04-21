using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Custom/Inspector")]
public class Inspector : MonoBehaviour
{
	[Header("My Status")]
	[Tooltip ("my health")]
	public int health;
	public int mana;

	[Space (10f)]
	[SerializeField] private int str;
	[HideInInspector] public int dex;

	[Header("maps")]
	public string location;
	[Range(0.5f, 1.5f)]
	public int zoomPoint;

	[TextArea(3, 5)]
	public string desc2;

	[ContextMenu("initialize")]
	public void initialize()
	{
		health = 50;
		mana = 100;
	}
}
