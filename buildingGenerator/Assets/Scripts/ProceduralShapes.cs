using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class ProceduralShapes : MonoBehaviour
{
    [SerializeField]
    private float width = 10.0f;

    [SerializeField]
    private float height = 10.0f;


    [SerializeField]
    private float depth = 10.0f;

    
    //private string randomShaderType = "Standard";
    [SerializeField]
    private string shaderName = "Universal Render Pipeline/Lit";



    [SerializeField]

    private ShapeTypes shapeType = ShapeTypes.Quad;

    private Mesh mesh;



    #region Random Generatino Implementation

    [SerializeField]
    private bool shouldGenerateRandomSizes = false;

    [SerializeField, Range(1, 100)]
    private int randomSeed = 10;
    
    private int prevRandomSeed = 10;

    private bool materialApplied = false;

    [SerializeField, Range(1.0f, 30.0f)]
    private float maxRandomSize = 10.0f;

    #endregion


    void Start()
    {
        Random.InitState(randomSeed);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        ApplyRandomMaterial();

    }
    void Generate()
    {
        if (shapeType == ShapeTypes.Quad)
        {
            mesh = new Quad
            {
                Width = width,
                Height = height
            }.Generate();
        }
        else if (shapeType == ShapeTypes.Cube)
        {
                mesh = new Cube
                {
                    Width = width,
                    Height = height,
                    Depth = depth
                }.Generate();
        }
        GetComponent<MeshFilter>().mesh = mesh;
        if (!materialApplied)
		{
            ApplyRandomMaterial();
            materialApplied = true;
        }
    }

    void GenerateRandomSizes()
	{
        width = Random.Range(1.0f, maxRandomSize);
        height = Random.Range(1.0f, maxRandomSize);
        depth = Random.Range(1.0f, maxRandomSize);
	}


    void ApplyRandomMaterial() => GetComponent<MeshRenderer>().ApplyRandomMaterial(
        shaderName,
        gameObject.name);
    
}


