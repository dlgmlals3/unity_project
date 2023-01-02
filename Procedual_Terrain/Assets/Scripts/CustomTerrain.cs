using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{
    public Vector2 randomHeightRange = new Vector2(0, 0.1f);
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);
    public bool resetTerrain = true;

    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlinHeightScale = 0.09f;

    // Multiple Perlin
    [System.Serializable]
    public class PerlinParameters
	{
        public float mPerlinXScale = 0.01f;
        public float mPerlinYScale = 0.01f;
        public int mPerlinOctaves = 3;
        public float mPerlinPersistance = 8;
        public float mPerlinHeightScale = 0.09f;
        public int mPerlinOffsetX = 0;
        public int mPerlinOffsetY = 0;
        public bool remove = false;
    }

    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters() // 빈목록 만들기
    };

    [System.Serializable]
    public class SplatHeights
	{
        public Texture2D texture = null;
        public float minHeight = 0.1f;
        public float maxHeight = 0.2f;
        public float minSlope = 0;
        public float maxSlope = 90;
        public Vector2 tileOffset = new Vector2(0, 0);
        public Vector2 tileSize = new Vector2(50, 50);
        public float splatOffset = 0.1f;
        public float splatNoiseXScale = 0.01f;
        public float splatNoiseYScale = 0.01f;
        public float splatNoiseScaler = 0.1f;
        public bool remove = false;
    }

    public List<SplatHeights> splatHeights = new List<SplatHeights>()
    {
        new SplatHeights()
    };

    // VEGETATION ---
    [System.Serializable]
    public class Vegetation
	{
        public GameObject mesh;
        public float minHeight = 0.1f;
        public float maxHeight = 0.2f;
        public float minSlope = 0;
        public float maxSlope = 90;
        public float minScale = 0.5f;
        public float maxScale = 1.0f;
        public Color colour1 = Color.white;
        public Color colour2 = Color.white;
        public Color lightColour = Color.white;
        public float minRotation = 0;
        public float maxRotation = 360;
        public float density = 0.5f;
        public bool remove = false;
    }
    public List<Vegetation> vegetation = new List<Vegetation>()
    {
        new Vegetation()
    };
    public int maxTrees = 5000;
    public int treeSpacing = 5;

    // VORONOI
    public float voronoiFallOff = 0.2f;
    public float voronoiDropOff = 0.6f;
    public float voronoiMinHeight = 0.1f;
    public float voronoiMaxHeight = 0.5f;
    public int voronoiPeaks = 5;
    public enum VoronoiType { Linear = 0, Power = 1, SinPow = 2, Combined = 3 }
    public VoronoiType voronoiType = VoronoiType.Linear;

    // Midpoint Displacement
    public float MPDheightMin = -2f;
    public float MPDheightMax = 2f;
    public float MPDheightDampenerPower = 2.0f;
    public float MPDroughness = 2.0f;

    // Smooth
    public int smoothAmount = 2;


    public Terrain terrain;
    public TerrainData terrainData;
    
    // VEGETATION
    public void PlantVegetation()
	{
        TreePrototype[] newTreePrototypes;
        newTreePrototypes = new TreePrototype[vegetation.Count];
        int tindex = 0;
        foreach (Vegetation t in vegetation)
		{
            newTreePrototypes[tindex] = new TreePrototype();
            newTreePrototypes[tindex].prefab = t.mesh;
            tindex++;
		}
        terrainData.treePrototypes = newTreePrototypes;

        List<TreeInstance> allVegetation = new List<TreeInstance>();
        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
		{
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
			{
                for (int tp = 0; tp < terrainData.treePrototypes.Length; tp++)
				{
                    float thisHeight = terrainData.GetHeight(x, z) / terrainData.size.y;
                    TreeInstance instance = new TreeInstance();
                    
                    var xCoordinate = (x + UnityEngine.Random.Range(-5.0f, 5.0f)) / terrainData.size.x;
                    var zCoordinate = (z + UnityEngine.Random.Range(-5.0f, 5.0f)) / terrainData.size.z;
                    //Debug.Log("x coordinates : " + xCoordinate + " " + zCoordinate);
                     
                    instance.position = new Vector3(
                        xCoordinate,
                        terrainData.GetHeight(x, z) / terrainData.size.y,
                        zCoordinate);
                        
                    instance.rotation = UnityEngine.Random.Range(0, 360);
                    instance.prototypeIndex = tp;
                    instance.color = Color.white;
                    instance.lightmapColor = Color.white;
                    instance.heightScale = 0.95f;
                    instance.widthScale = 0.95f;

                    allVegetation.Add(instance);
                    if (allVegetation.Count >= maxTrees) goto TREESDONE;
				}
			}
		}
    TREESDONE:
        terrainData.treeInstances = allVegetation.ToArray();

	}

    public void AddNewVegetation()
	{
        vegetation.Add(new Vegetation());
	}

    public void RemoveVegetation()
	{
        List<Vegetation> keptVegetation = new List<Vegetation>();
        for (int i = 0; i < vegetation.Count; i++)
		{
            if (!vegetation[i].remove)
			{
                keptVegetation.Add(vegetation[i]);
			}
		}
        if (keptVegetation.Count == 0)
		{
            keptVegetation.Add(vegetation[0]);
		}
        vegetation = keptVegetation;
	}

    public void AddNewSplatHeight()
	{
        splatHeights.Add(new SplatHeights());
	}

    public void RemoveSplatHeight()
	{
        List<SplatHeights> keptSplatHeights = new List<SplatHeights>();
        for (int i = 0; i < splatHeights.Count; i++)
		{
            if (!splatHeights[i].remove)
			{
                keptSplatHeights.Add(splatHeights[i]);
			}
		}
        if (keptSplatHeights.Count == 0)
		{
            keptSplatHeights.Add(splatHeights[0]);
		}
        splatHeights = keptSplatHeights;
	}
    float GetSteepness(float[,] heightmap, int x, int y, int width, int height)
	{
        float h = heightmap[x, y];
        int nx = x + 1;
        int ny = y + 1;

        if (nx > width - 1) nx = x - 1;
        if (ny > height - 1) ny = y - 1;
        float dx = heightmap[nx, y] - h;
        float dy = heightmap[x, ny] - h;
        Vector2 gradient = new Vector2(dx, dy);
        float steep = gradient.magnitude;
        return steep;
	}

	public void SplatMaps()
	{
		TerrainLayer[] terrainLayer;
        terrainLayer = new TerrainLayer[splatHeights.Count];
		int terrainIndex = 0;
		foreach (SplatHeights sh in splatHeights)
		{
			terrainLayer[terrainIndex] = new TerrainLayer();
            terrainLayer[terrainIndex].diffuseTexture = sh.texture;
            terrainLayer[terrainIndex].tileOffset = sh.tileOffset;
            terrainLayer[terrainIndex].tileSize = sh.tileSize;
            terrainLayer[terrainIndex].diffuseTexture.Apply(true);
            terrainIndex++;
		}
        terrainData.terrainLayers = terrainLayer;

        float[,] heightMap = terrainData.GetHeights(0, 0,
            (int)terrainData.heightmapResolution,
            (int)terrainData.heightmapResolution);
        float[,,] splatmapData = new float[
            terrainData.alphamapWidth,
            terrainData.alphamapHeight,
            terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
		{
            for (int x = 0; x < terrainData.alphamapWidth; x++)
			{
                float[] splat = new float[terrainData.alphamapLayers];
                for (int i=0; i < splatHeights.Count; i++)
				{
                    float noise = Mathf.PerlinNoise(x * splatHeights[i].splatNoiseXScale,
                                                      y * splatHeights[i].splatNoiseYScale) 
                                                      * splatHeights[i].splatNoiseScaler;
                    float offset = splatHeights[i].splatOffset + noise;
                    float thisHeightStart = splatHeights[i].minHeight - offset;
                    float thisHeightStop = splatHeights[i].maxHeight + offset;
                    /*float steepness = GetSteepness(heightMap, x, y,
                                        terrainData.heightmapResolution,
                                        terrainData.heightmapResolution);*/
                    float steepness = terrainData.GetSteepness(x / (float)terrainData.alphamapHeight,
                        y / (float)terrainData.alphamapWidth); // x y 반대임.

                    if ((heightMap[x, y] >= thisHeightStart && heightMap[x, y] <= thisHeightStop) &&
                            (steepness >= splatHeights[i].minSlope &&
                            steepness <= splatHeights[i].maxSlope))
					{
                        splat[i] = 1;
					}
				}
                NormalizeVector(splat);
                for (int j = 0; j< splatHeights.Count; j++)
				{
                    splatmapData[x, y, j] = splat[j];
				}
			}
		}
        terrainData.SetAlphamaps(0, 0, splatmapData);
	}

    void NormalizeVector(float[] v)
	{
        float total = 0;
        for (int i = 0; i < v.Length; i++)
		{
            total += v[i];
		}
        for (int i=0; i < v.Length; i++)
		{
            v[i] /= total;
		}
	}

	public void Smooth()
	{
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution,
            terrainData.heightmapResolution);
        float smoothProgress = 0;
        EditorUtility.DisplayProgressBar("Smoothing Terrain", "Progress", smoothProgress);

        for (int s = 0; s < smoothAmount; s++)
		{
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                for (int x = 0; x < terrainData.heightmapResolution; x++)
                {
                    float avgHeight = heightMap[x, z];
                    List<Vector2> neighbours = GenerateNeighbours(new Vector2(x, z),
                        (int)terrainData.heightmapResolution, (int)terrainData.heightmapResolution);
                    foreach (Vector2 n in neighbours)
                    {
                        avgHeight += heightMap[(int)n.x, (int)n.y];
                    }
                    heightMap[x, z] = avgHeight / ((float)neighbours.Count + 1);
                }
            }
            smoothProgress++;
            EditorUtility.DisplayProgressBar("Smoothing Terrain", "Progress", 
                smoothProgress / smoothAmount);
        }
        terrainData.SetHeights(0, 0, heightMap);
        EditorUtility.ClearProgressBar();
	}

    List<Vector2> GenerateNeighbours(Vector2 pos, int width, int height)
	{
        List<Vector2> neighbours = new List<Vector2>();
        for (int y = -1; y < 2; y++)
		{
            for (int x = -1; x < 2; x++)
			{
                if( !(x == 0 && y == 0))
				{
                    Vector2 nPos = new Vector2(Mathf.Clamp(pos.x + x, 0, width - 1),
                        Mathf.Clamp(pos.y + y, 0, height - 1));
                    if (!neighbours.Contains(nPos))
					{
                        neighbours.Add(nPos);
					}
				}
			}
		}
        return neighbours;
	}
    public void MidPointDisplacement()
    {
        float[,] heightMap = GetHeightMap();
        int width = terrainData.heightmapResolution - 1;
        int height = terrainData.heightmapResolution - 1;
        int squareSize = width;
        float heightMin = MPDheightMin;
        float heightMax = MPDheightMax;
        float heightDampener = (float)Mathf.Pow(MPDheightDampenerPower, -1 * MPDroughness);

        int cornerX, cornerY;
        int midX, midY;
        int pmidXL, pmidXR, pmidYU, pmidYD;
        
        while (squareSize > 0)
        {
            for (int x = 0; x < width; x += squareSize)
            {
                for (int y = 0; y < height; y += squareSize)
                {
                    cornerX = (x + squareSize);
                    cornerY = (y + squareSize);

                    midX = (int)(x + squareSize / 2.0f);
                    midY = (int)(y + squareSize / 2.0f);

                    heightMap[midX, midY] = (float)((heightMap[x, y] +
                        heightMap[cornerX, y] +
                        heightMap[x, cornerY] +
                        heightMap[cornerX, cornerY]) / 4.0f +
                        UnityEngine.Random.Range(heightMin, heightMax));
                }
            }

            for (int x = 0; x < width; x += squareSize)
            {
                for (int y = 0; y < height; y += squareSize)
                {
                    cornerX = (x + squareSize);
                    cornerY = (y + squareSize);

                    midX = (int)(x + squareSize / 2.0f);
                    midY = (int)(y + squareSize / 2.0f);

                    pmidXR = (int)(midX + squareSize);
                    pmidYU = (int)(midY + squareSize);
                    pmidXL = (int)(midX - squareSize);
                    pmidYD = (int)(midY - squareSize);

                    if (pmidXL <= 0 || pmidYD <= 0 || pmidXR >= width - 1 || pmidYU >= width - 1) continue;

                    heightMap[midX, y] = (float)((
                        heightMap[midX, midY] +
                        heightMap[x, y] +
                        heightMap[midX, pmidYD] +
                        heightMap[cornerX, y]) / 4.0f +
                        UnityEngine.Random.Range(heightMin, heightMax));

                    heightMap[midX, cornerY] = (float)((
                        heightMap[midX, pmidYU] +
                        heightMap[cornerX, cornerY] +
                        heightMap[midX, midY] +
                        heightMap[x, cornerY]) / 4.0f +
                        UnityEngine.Random.Range(heightMin, heightMax));

                    heightMap[x, midY] = (float)((
                        heightMap[x, cornerY] +
                        heightMap[midX, midY] +
                        heightMap[x, y] +
                        heightMap[pmidXL, midY]) / 4.0f +
                        UnityEngine.Random.Range(heightMin, heightMax));

                    heightMap[cornerX, midY] = (float)((
                        heightMap[cornerX, cornerY] +
                        heightMap[pmidXR, midY] +
                        heightMap[cornerX, y] +
                        heightMap[midX, midY]) / 4.0f +
                        UnityEngine.Random.Range(heightMin, heightMax));
                }
            }
            squareSize = (int)(squareSize / 2.0f);
            heightMin *= heightDampener;
            heightMax *= heightDampener;
        }
        terrainData.SetHeights(0, 0, heightMap);
    }    

    public void Voronoi()
    {
        float[,] heightMap = GetHeightMap();

        for (int p = 0; p < voronoiPeaks; p++)
		{
            Vector3 peak = new Vector3(
                UnityEngine.Random.Range(0, terrainData.heightmapResolution),
                UnityEngine.Random.Range(voronoiMinHeight, voronoiMaxHeight),
                UnityEngine.Random.Range(0, terrainData.heightmapResolution)
            );
            if (heightMap[(int)peak.x, (int)peak.z] < peak.y)
			{
                heightMap[(int)peak.x, (int)peak.z] = peak.y;
            } else
			{
                continue;
			}

            heightMap[(int)peak.x, (int)peak.z] = peak.y;
            Vector2 peakLocation = new Vector2(peak.x, peak.z);
            float maxDistance = Vector2.Distance(new Vector2(0, 0),
                new Vector2(terrainData.heightmapResolution, terrainData.heightmapResolution));

            for (int z = 0; z < terrainData.heightmapResolution; z++)
		    {
                for (int x = 0; x < terrainData.heightmapResolution; x++)
			    {
                    if (!(x == peak.x && z == peak.z))
				    {
                        float distanceToPeak = Vector2.Distance(peakLocation, new Vector2(x, z)) / maxDistance;
                        float h = 0;
                        if (voronoiType == VoronoiType.Combined)
						{
                            h = peak.y - distanceToPeak * voronoiFallOff - Mathf.Pow(distanceToPeak, voronoiDropOff);
                        } else if (voronoiType == VoronoiType.Power)
						{
                            h = peak.y - Mathf.Pow(distanceToPeak, voronoiDropOff) * voronoiFallOff;
                        }
                        else if (voronoiType == VoronoiType.SinPow)
                        {
                            h = peak.y - Mathf.Pow(distanceToPeak * 3, voronoiFallOff) -
                                Mathf.Sin(distanceToPeak * 2 * Mathf.PI) / voronoiDropOff;
                        }
                        else
						{
                            h = peak.y - distanceToPeak * voronoiFallOff;
						}

                        if (heightMap[x, z] < h)
						{
                            heightMap[x, z] = h;
                        }                        
				    }
			    }
		    }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    float [,] GetHeightMap()
	{
        if (!resetTerrain)
		{
            return terrainData.GetHeights(0, 0, (int)terrainData.heightmapResolution,
                (int)terrainData.heightmapResolution);
		} else
		{
            return new float[(int)terrainData.heightmapResolution,
                (int)terrainData.heightmapResolution];
		}
	}
    public void Perlin()
	{
        float[,] heightMap = GetHeightMap();
        for (int z = 0; z < terrainData.heightmapResolution; z++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                heightMap[x, z] += Utils.fBM(
                    (x + perlinOffsetX) * perlinXScale,
                    (z + perlinOffsetY) * perlinYScale,
                    perlinOctaves,
                    perlinPersistance) * perlinHeightScale;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void MultiplePerlinTerrain()
	{
        float[,] heightMap = GetHeightMap();
        for (int z = 0; z < terrainData.heightmapResolution; z++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                foreach (PerlinParameters p in perlinParameters)
				{
                    heightMap[x, z] += Utils.fBM(
                        (x + p.mPerlinOffsetX) * p.mPerlinXScale,
                        (z + p.mPerlinOffsetY) * p.mPerlinYScale,
                        p.mPerlinOctaves,
                        p.mPerlinPersistance) * p.mPerlinHeightScale;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void AddNewPerlin()
	{
        perlinParameters.Add(new PerlinParameters());
	}

    public void RemovePerlin()
	{
        List<PerlinParameters> keptPerlinParameters = new List<PerlinParameters>();
        for (int i=0; i<perlinParameters.Count; i++)
		{
            if (!perlinParameters[i].remove)
			{
                keptPerlinParameters.Add(perlinParameters[i]);
			}
		}
        if (keptPerlinParameters.Count == 0)
		{
            keptPerlinParameters.Add(perlinParameters[0]);
		}
        perlinParameters = keptPerlinParameters;
	}

    public void RandomTerrain()
    {
        float[,] heightMap = GetHeightMap();
        
        for (int x=0; x<terrainData.heightmapResolution; x++)
        {
            for (int z=0; z<terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void LoadTexture()
	{
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += heightMapImage.GetPixel((int)(x * heightMapScale.x), 
                    (int)(z * heightMapScale.z)).grayscale * heightMapScale.y;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
 
    public void ResetTerrain()
	{
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] = 0;
            }
        }
        Debug.Log("width height : " + terrainData.heightmapResolution);
        terrainData.SetHeights(0, 0, heightMap);
    }

	public void OnEnable()
	{
        Debug.Log("Initialising Terrain Data");
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;
	}

	// Start is called before the first frame update
	void Awake()
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        AddTag(tagsProp, "Terrain");
        AddTag(tagsProp, "Cloud");
        AddTag(tagsProp, "Shore");

        tagManager.ApplyModifiedProperties();
        this.gameObject.tag = "Terrain";
    }

    void AddTag(SerializedProperty tagsProp, string newTag)
	{
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
		{
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; break; }
		}
        if (!found)
		{
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
	}
}
