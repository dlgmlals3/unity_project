using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Grid : MonoBehaviour
{
	[SerializeField, Range(1, 200)]
	private int width = 1;
	private int prevWidth = 1;

	[SerializeField, Range(1, 200)]
	private int height = 1;
	private int prevHeight = 1;

	private GameObject[,] grid = null;

	[SerializeField, Range(0.1f, 20f)]
	private float shapeWidth = 2.0f;
	private float prevShapeWidth = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	private float shapeHeight = 2.0f;
	private float prevShapeHeight = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	private float shapeDepth = 2.0f;
	private float prevShapeDepth = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	private float maxRandomHeight = 10;

	[SerializeField, Range(0.1f, 20f)]
	private float maxRandomWidth = 10;

	[SerializeField, Range(1, 100)]
	private int randomSeed = 1;
	private int prevSeed = 1;

	[SerializeField]
	private ShapeTypes shapeType = ShapeTypes.Cube;


	[SerializeField]
	private string shaderName = "Universal Render Pipeline/Lit";

	[SerializeField, Tooltip("How many procedural materisal")]
	private int proceduralMaterialsToGenerate = 3;

	private int prevProceduralMaterialsToGenerate = 3;
	private Material[] proceduralMaterials = null;

	[SerializeField]
	private Material[] defaultMaterial = null;

	#region UI Bindings
	[SerializeField]
	private TextMeshProUGUI numOfShapesText = null;
	[SerializeField]
	private TextMeshProUGUI lengthOfTimeText = null;
	#endregion

	void Start()
	{
		prevWidth = width;
		prevHeight = height;
		UnityEngine.Random.InitState(randomSeed);
		grid = new GameObject[height, width];
		proceduralMaterials = new Material[proceduralMaterialsToGenerate];
		Debug.Log("proceduralMaterialsToGenerate" + proceduralMaterialsToGenerate + " defaultMaterial : " + defaultMaterial);
		if (proceduralMaterialsToGenerate > 0 && defaultMaterial.Length == 0)
		{
			Debug.Log("MeshRendererExtensions.GetRandomMaterials");
			proceduralMaterials = MeshRendererExtensions.GetRandomMaterials(shaderName, proceduralMaterialsToGenerate);
		}

		BuildGrid();
	}

	void Update()
	{
		if (prevSeed != randomSeed)
		{
			BuildGrid();
			prevSeed = randomSeed;
		}
		if (PropertiesChanged())
		{
			ClearAll();
			prevWidth = width;
			prevHeight = height;

			prevShapeWidth = shapeWidth;
			prevShapeHeight = shapeHeight;
			prevShapeDepth = shapeDepth;

			prevProceduralMaterialsToGenerate = proceduralMaterialsToGenerate;
			grid = new GameObject[height, width];
			BuildGrid();
		}
	}
	private bool PropertiesChanged()
	{
		if (prevWidth != width || prevHeight != height || prevShapeWidth != shapeWidth
			|| prevShapeHeight != shapeHeight || prevShapeDepth != shapeDepth ||
			proceduralMaterialsToGenerate != prevProceduralMaterialsToGenerate)
		{
			return true;
		}
		return false;
	}
	private void ClearAll()
	{
		for (int row = 0; row < prevHeight; row++)
		{
			for (int col = 0; col < prevWidth; col++)
			{
				if (grid[row, col] != null)
				{
					DestroyImmediate(grid[row, col]);
				}
			}
		}
	}
	void BuildGrid()
	{
		numOfShapesText.text = $"{width * height}";
		DateTime started = DateTime.Now;

		for (int row = 0; row < height; row++)
		{
			for (int col = 0; col < width; col++)
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
		cell.transform.position =
			new Vector3(shapeWidth * row,
						shapeHeight * UnityEngine.Random.Range(1.0f, maxRandomHeight),
						shapeDepth * col);

		MeshFilter meshFilter = cell.AddComponent<MeshFilter>();
		MeshRenderer renderer = cell.AddComponent<MeshRenderer>();
		Cube cube = new Cube
		{
			Width = shapeWidth,
			Height = shapeHeight,
			Depth = shapeDepth
		};

		meshFilter.mesh = cube.Generate();

		if (proceduralMaterials.Length > 0 && defaultMaterial.Length == 0)
			renderer.material = proceduralMaterials[UnityEngine.Random.Range(0, proceduralMaterialsToGenerate - 1)];
		else
			renderer.material = defaultMaterial[UnityEngine.Random.Range(0, defaultMaterial.Length - 1)];

		yield return null;
	}
}