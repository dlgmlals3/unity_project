using UnityEditor;
using UnityEngine;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]

public class CustomTerrainEditor : Editor
{
	SerializedProperty randomHeightRange;
	SerializedProperty heightMapScale;
	SerializedProperty heightMapImage;
	SerializedProperty perlinXScale;
	SerializedProperty perlinYScale;
	SerializedProperty perlinOffsetX;
	SerializedProperty perlinOffsetY;
	SerializedProperty perlinOctaves;
	SerializedProperty perlinPersistance;
	SerializedProperty perlinHeightScale;
	SerializedProperty resetTerrain;
	SerializedProperty voronoiFallOff;
	SerializedProperty voronoiDropOff;
	SerializedProperty voronoiMinHeight;
	SerializedProperty voronoiMaxHeight;
	SerializedProperty voronoiPeaks;
	SerializedProperty voronoiType;
	SerializedProperty mpdHeightMin;
	SerializedProperty mpdHeightMax;
	SerializedProperty mpdHeightDampenerPower;
	SerializedProperty mpdRoughness;
	SerializedProperty smoothAmount;

	GUITableState splatMapTable;
	SerializedProperty splatHeigths;

	GUITableState perlinParameterTable;
	SerializedProperty perlinParameters;

	GUITableState vegMapTable;
	SerializedProperty vegetation;
	SerializedProperty maxTrees;
	SerializedProperty treeSpacing;


	bool showRandom = false;
	bool showLoadHeights = false;
	bool showPerlinNoise = false;
	bool showMultiplePerlin = false;
	bool showVoronoi = false;
	bool showMPD = false;
	bool showSmooth = false;
	bool showSplatMaps = false;
	bool showHeights = false;
	bool showVeg = false;
	bool testkk = false;

	Texture2D hmTexture;

	private Vector2 scrollPos;

	private void OnEnable()
	{
		resetTerrain = serializedObject.FindProperty("resetTerrain");

		randomHeightRange = serializedObject.FindProperty("randomHeightRange");
		heightMapScale = serializedObject.FindProperty("heightMapScale");
		heightMapImage = serializedObject.FindProperty("heightMapImage");
		
		// PERLIN NOISE
		perlinXScale = serializedObject.FindProperty("perlinXScale");
		perlinYScale = serializedObject.FindProperty("perlinYScale");
		perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
		perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
		perlinOctaves = serializedObject.FindProperty("perlinOctaves");
		perlinPersistance = serializedObject.FindProperty("perlinPersistance");
		perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");

		perlinParameterTable = new GUITableState("perlinParameterTable");
		perlinParameters = serializedObject.FindProperty("perlinParameters");
		voronoiFallOff = serializedObject.FindProperty("voronoiFallOff");
		voronoiDropOff = serializedObject.FindProperty("voronoiDropOff");
		voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
		voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
		voronoiPeaks = serializedObject.FindProperty("voronoiPeaks");
		voronoiType = serializedObject.FindProperty("voronoiType");

		mpdHeightMin = serializedObject.FindProperty("mpdHeightMin");
		mpdHeightMax = serializedObject.FindProperty("mpdHeightMax");
		mpdHeightDampenerPower = serializedObject.FindProperty("mpdHeightDampenerPower");
		mpdRoughness = serializedObject.FindProperty("mpdRoughness");

		smoothAmount = serializedObject.FindProperty("smoothAmount");

		splatMapTable = new GUITableState("splatMapTable");
		splatHeigths = serializedObject.FindProperty("splatHeigths");

		hmTexture = new Texture2D(513, 513, TextureFormat.ARGB32, false);
		
		vegMapTable = new GUITableState("vegMapTable");
		vegetation = serializedObject.FindProperty("vegetation");
		maxTrees = serializedObject.FindProperty("maxTrees");
		treeSpacing = serializedObject.FindProperty("treeSpacing");

		
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		CustomTerrain terrain = (CustomTerrain)target;

		Rect r = EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(r.width),
			GUILayout.Height(r.height));
		EditorGUI.indentLevel++;



		EditorGUILayout.PropertyField(resetTerrain);
		showRandom = EditorGUILayout.Foldout(showRandom, "Random");
		if (showRandom)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Set Height Between Random Values", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(randomHeightRange);
			if (GUILayout.Button("Random Height"))
			{
				terrain.RandomTerrain();
			}
		}

		showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
		if (showLoadHeights)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(heightMapImage);
			EditorGUILayout.PropertyField(heightMapScale);
			if (GUILayout.Button("Load Texture"))
			{
				terrain.LoadTexture();
			}
		}

		showPerlinNoise = EditorGUILayout.Foldout(showPerlinNoise, "Single Perlin Noise");
		if (showPerlinNoise)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);
			EditorGUILayout.Slider(perlinXScale, 0, 1, new GUIContent("X Scale"));
			EditorGUILayout.Slider(perlinYScale, 0, 1, new GUIContent("Y Scale"));
			EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("Offset X"));
			EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Offset Y"));
			EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
			EditorGUILayout.Slider(perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
			EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));

			if (GUILayout.Button("Perlin"))
			{
				terrain.Perlin();
			}
		}

		showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin Noise");
		if (showMultiplePerlin)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
			perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable,
				serializedObject.FindProperty("perlinParameters"));
			GUILayout.Space(20);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				terrain.AddNewPerlin();
			}
			if (GUILayout.Button("-"))
			{
				terrain.RemovePerlin();
			}
			EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Apply Mutiple Perlin"))
			{
				terrain.MultiplePerlinTerrain();
			}
		}

		showVoronoi = EditorGUILayout.Foldout(showVoronoi, "Voronoi");
		if (showVoronoi)
		{
			EditorGUILayout.IntSlider(voronoiPeaks, 1, 10, new GUIContent("Peak Count"));
			EditorGUILayout.Slider(voronoiFallOff, 0, 10, new GUIContent("voronoiFallOff"));
			EditorGUILayout.Slider(voronoiDropOff, 0, 10, new GUIContent("voronoiDropOff"));
			EditorGUILayout.Slider(voronoiMinHeight, 0, 1, new GUIContent("voronoiMinHeight"));
			EditorGUILayout.Slider(voronoiMaxHeight, 0, 1, new GUIContent("voronoiMaxHeight"));
			EditorGUILayout.PropertyField(voronoiType);

			if (GUILayout.Button("Voronoi"))
			{
				terrain.Voronoi();
			}
		}

		showMPD = EditorGUILayout.Foldout(showMPD, "MPD");
		if (showMPD)
		{
			EditorGUILayout.PropertyField(mpdHeightMin);
			EditorGUILayout.PropertyField(mpdHeightMax);
			EditorGUILayout.PropertyField(mpdHeightDampenerPower);
			EditorGUILayout.PropertyField(mpdRoughness);
			if (GUILayout.Button("MPD"))
			{
				terrain.MidPointDisplacement();
			}

		}

		showSmooth = EditorGUILayout.Foldout(showSmooth, "Smooth");
		if (showSmooth)
		{
			EditorGUILayout.IntSlider(smoothAmount, 1, 10, new GUIContent("smoothAmount"));
			if (GUILayout.Button("Smooth"))
			{
				terrain.Smooth();
			}
		}


		showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
		if (showSplatMaps)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
			splatMapTable = GUITableLayout.DrawTable(splatMapTable,
				serializedObject.FindProperty("splatHeights"));

			GUILayout.Space(20);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				terrain.AddNewSplatHeight();
			}
			if (GUILayout.Button("-"))
			{
				terrain.RemoveSplatHeight();
			}
			EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Apply SplatMaps"))
			{
				terrain.SplatMaps();
			}
		}

		showHeights = EditorGUILayout.Foldout(showHeights, "Height Map");
		if (showHeights)
		{
			GUILayout.Label("showHeights", EditorStyles.boldLabel);
			int hmtSize = (int)(EditorGUIUtility.currentViewWidth - 100);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUILayout.Label(hmTexture, GUILayout.Width(hmtSize), GUILayout.Height(hmtSize));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Refresh", GUILayout.Width(hmtSize)))
			{
				float[,] heightMap = terrain.terrainData.GetHeights(0, 0,
					(int)terrain.terrainData.heightmapResolution,
					(int)terrain.terrainData.heightmapResolution);
				for (int y = 0; y < terrain.terrainData.alphamapHeight; y++)
				{
					for (int x = 0; x < terrain.terrainData.alphamapWidth; x++)
					{
						hmTexture.SetPixel(x, y, new Color(
							heightMap[x, y],
							heightMap[x, y],
							heightMap[x, y], 1));
					}
				}
				hmTexture.Apply();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		if (GUILayout.Button("Reset Terrain"))
		{
			terrain.ResetTerrain();
		}


		showVeg = EditorGUILayout.Foldout(showVeg, "Vegetation");
		if (showVeg)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Label("Vegetation", EditorStyles.boldLabel);
			EditorGUILayout.IntSlider(maxTrees, 0, 10000, new GUIContent("Maximum Trees"));
			EditorGUILayout.IntSlider(treeSpacing, 2, 20, new GUIContent("Trees Spacing"));
			vegMapTable = GUITableLayout.DrawTable(vegMapTable,
				serializedObject.FindProperty("vegetation"));
			GUILayout.Space(20);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				terrain.AddNewVegetation();
			}
			if (GUILayout.Button("-"))
			{
				terrain.RemoveVegetation();
			}
			EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Apply Vegetation"))
			{
				terrain.PlantVegetation();
			}
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		serializedObject.ApplyModifiedProperties();
	}
}

/*




/////////
*/
