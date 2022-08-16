using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProceduralParameter", menuName = "Procedural/Parameter", order = 0)]

public class ProceduralParam : ScriptableObject
{
	[SerializeField, Range(1, 200)]
	public int width = 1;

	[SerializeField, Range(1, 200)]
	public int height = 1;

	public GameObject[,] grid = null;

	[SerializeField, Range(0.1f, 20f)]
	public float shapeWidth = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	public float shapeHeight = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	public float shapeDepth = 2.0f;

	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomHeight = 10;

	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomWidth = 10;

	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomDepth = 10;


	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomWidthOffset = 10f;

	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomHeightOffset = 10f;

	[SerializeField, Range(0.1f, 20f)]
	public float maxRandomDepthOffset = 10f;

	[SerializeField, Range(0.1f, 20f)]
	public Vector3 marginBetweenShapes = Vector3.zero;

	[SerializeField, Range(1, 100)]
	public int randomSeed = 1;

	[SerializeField, Tooltip("How many procedural materisal")]
	public int proceduralMaterialsToGenerate = 3;

	public Material[] proceduralMaterials = null;

	[SerializeField]
	public Material[] defaultMaterial = null;

	[SerializeField]
	public ShapeTypes shapeType = ShapeTypes.Cube;

	[SerializeField]
	public string shaderName = "Universal Render Pipeline/Lit";

	[SerializeField]
	public bool makeShapesStatic = true;

	[SerializeField]
	public bool shouldGenerateRigidBodies = false;
}
