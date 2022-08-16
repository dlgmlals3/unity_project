using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
	[SerializeField]
	private Color color = new Color(1, 0, 0, 0.5f);
	[SerializeField]
	private GridWithParams gridWithParams;

	[SerializeField]
	private Vector3 gridOffset = Vector3.zero;

	public enum DrawOptions
	{
		Selection,
		Always
	}
	[SerializeField]
	private DrawOptions Options = DrawOptions.Always;

	private void OnDrawGizmos()
	{
		if (Options == DrawOptions.Always && gridWithParams != null)
		{
			Gizmos.color = color;
			Gizmos.DrawCube(gridWithParams.bounds.center, gridWithParams.bounds.size + gridOffset);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (Options == DrawOptions.Selection && gridWithParams != null)
		{
			Gizmos.color = color;
			Gizmos.DrawCube(gridWithParams.bounds.center, gridWithParams.bounds.size + gridOffset);
		}
	}
}
