using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class GridWithParams : MonoBehaviour
{
    [SerializeField]
    public ProceduralParam parameters = null;

    private GameObject[,] grid = null;

	private Material[] proceduralMaterials = null;

	#region UI Bindings
	[SerializeField]
	private TextMeshProUGUI numOfShapesText = null;
	[SerializeField]
	private TextMeshProUGUI lengthOfTimeText = null;
	#endregion

	[SerializeField]
	public Bounds bounds;

	void OnEnable()
	{
		if (parameters == null)
		{
			Debug.LogError("You must set procedural parameters");
			enabled = false;
		}
		Debug.Log("OnEnable!!!");
	}

	void Reset()
	{
		ClearAll();

		Random.InitState(parameters.randomSeed);
		grid = new GameObject[parameters.height, parameters.width];
		proceduralMaterials = new Material[parameters.proceduralMaterialsToGenerate];
		bounds = new Bounds(Vector3.zero, Vector3.zero);

		if (parameters.proceduralMaterialsToGenerate > 0 && parameters.defaultMaterial.Length == 0)
		{
			proceduralMaterials = MeshRendererExtensions.GetRandomMaterials(parameters.shaderName, parameters.proceduralMaterialsToGenerate);

		}
	}
	void Start() => BuildGrid();
	
	public void BuildGrid()
	{
		Debug.Log("BuildGrid!!!");

		Reset();

		grid = new GameObject[parameters.height, parameters.width];
		
		numOfShapesText.text = $"{parameters.height * parameters.width}";
		
		DateTime started = DateTime.Now;

		for (int row = 0; row < parameters.height; row++)
		{
			for (int col = 0; col < parameters.width; col++)
			{
				StartCoroutine(AddCell(row, col));
			}
		}

		DateTime ended = DateTime.Now;
		TimeSpan diff = ended - started;
		if (diff.Seconds == 0)
			lengthOfTimeText.text = $"{diff.Milliseconds} milliseconds";
		else if (diff.Seconds < 100)
			lengthOfTimeText.text = $"{diff.Seconds}.{diff.Milliseconds}";
		else
			lengthOfTimeText.text = $"{diff.Minutes}.{diff.Seconds}.{diff.Milliseconds}";
	}
	IEnumerator AddCell(int row, int col)
	{
		GameObject cell = null;
		if (grid[row, col] == null)
		{
			cell = new GameObject($"cell_{row}_{col}");
			cell.transform.parent = gameObject.transform;
			grid[row, col] = cell;
		}
		else
		{
			DestroyImmediate(grid[row, col]);
			grid[row, col] = new GameObject($"cell_{row}_{col}");
			cell = grid[row, col];
			cell.transform.parent = gameObject.transform;
		}
		
		cell.isStatic = parameters.makeShapesStatic;
		cell.transform.localPosition =
			Vector3.Scale(new Vector3(
						parameters.shapeWidth * row * Random.Range(1.0f, parameters.maxRandomWidthOffset), 0,
						parameters.shapeHeight * col * Random.Range(1.0f, parameters.maxRandomDepthOffset)),
						parameters.marginBetweenShapes);

		MeshFilter meshFilter = cell.AddComponent<MeshFilter>();
		MeshRenderer renderer = cell.AddComponent<MeshRenderer>();

		Shape shape = null;

		if (parameters.shapeType == ShapeTypes.Cube)
		{
			shape = new Cube
			{
				Width = parameters.shapeWidth * Random.Range(1.0f, parameters.maxRandomWidth),
				Height = parameters.shapeHeight * Random.Range(1.0f, parameters.maxRandomHeight),
				Depth = parameters.shapeDepth * Random.Range(1.0f, parameters.maxRandomDepth)
			};
		} else if (parameters.shapeType == ShapeTypes.Quad)
		{
			shape = new Quad
			{
				Width = parameters.shapeWidth * Random.Range(1.0f, parameters.maxRandomWidth),
				Depth = parameters.shapeDepth * Random.Range(1.0f, parameters.maxRandomDepth)
			};
		}

		meshFilter.mesh = shape.Generate();

		if (proceduralMaterials.Length > 0 && parameters.defaultMaterial.Length == 0)
			renderer.material = proceduralMaterials[UnityEngine.Random.Range(0, parameters.proceduralMaterialsToGenerate - 1)];
		else
			renderer.material = parameters.defaultMaterial[UnityEngine.Random.Range(0, parameters.defaultMaterial.Length - 1)];

		if (parameters.shouldGenerateRigidBodies)
		{
			cell.AddComponent<BoxCollider>();
			cell.AddComponent<Rigidbody>();
		}
		bounds.Encapsulate(renderer.bounds);
		yield return null;
	}


	private void ClearAll()
	{
		if (grid?.Length > 0)
		{
			for (int row = 0; row < grid.GetLength(0); row++)
			{
				for (int col = 0; col < grid.GetLength(1); col++)
				{
					if (grid[row, col] != null)
					{
						DestroyImmediate(grid[row, col]);
					}
				}
			}
		}
		if (transform.childCount > 0)
		{
			foreach(Transform child in transform)
			{
				DestroyImmediate(child.gameObject);
			}
		}
		grid = null;
	}
}
