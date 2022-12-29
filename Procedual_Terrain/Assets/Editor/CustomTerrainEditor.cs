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
	SerializedProperty MPDheightMin;
	SerializedProperty MPDheightMax;
	SerializedProperty MPDheightDampenerPower;
	SerializedProperty MPDroughness;
	SerializedProperty smoothAmount;

	GUITableState splatMapTable;
	SerializedProperty splatHeigths;

	GUITableState perlinParameterTable;
	SerializedProperty perlinParameters;

	bool showRandom = false;
	bool showLoadHeights = false;
	bool showPerlinNoise = false;
	bool showMultiplePerlin = false;
	bool showVoronoi = false;
	bool showMPD = false;
	bool showSmooth = false;
	bool showSplatMaps = false;
	bool showHeights = false;

	Texture2D hmTexture;

	private void OnEnable()
	{
		randomHeightRange = serializedObject.FindProperty("randomHeightRange");
		heightMapScale = serializedObject.FindProperty("heightMapScale");
		heightMapImage = serializedObject.FindProperty("heightMapImage");
		perlinXScale = serializedObject.FindProperty("perlinXScale");
		perlinYScale = serializedObject.FindProperty("perlinYScale");
		perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
		perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
		perlinOctaves = serializedObject.FindProperty("perlinOctaves");
		perlinPersistance = serializedObject.FindProperty("perlinPersistance");
		perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
		resetTerrain = serializedObject.FindProperty("resetTerrain");

		perlinParameterTable = new GUITableState("perlinParameterTable");
		perlinParameters = serializedObject.FindProperty("perlinParameters");

		voronoiPeaks = serializedObject.FindProperty("voronoiPeaks");
		voronoiDropOff = serializedObject.FindProperty("voronoiDropOff");
		voronoiFallOff = serializedObject.FindProperty("voronoiFallOff");
		voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
		voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
		voronoiType = serializedObject.FindProperty("voronoiType");

		MPDheightMin = serializedObject.FindProperty("MPDheightMin");
		MPDheightMax = serializedObject.FindProperty("MPDheightMax");
		MPDheightDampenerPower = serializedObject.FindProperty("MPDheightDampenerPower");
		MPDroughness = serializedObject.FindProperty("MPDroughness");
		smoothAmount = serializedObject.FindProperty("smoothAmount");

		splatMapTable = new GUITableState("splatMapTable");
		hmTexture = new Texture2D(513, 513, TextureFormat.ARGB32, false);
	}

	Vector2 scrollPos;
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		CustomTerrain terrain = (CustomTerrain)target;

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
			EditorGUILayout.PropertyField(MPDheightMin);
			EditorGUILayout.PropertyField(MPDheightMax);
			EditorGUILayout.PropertyField(MPDheightDampenerPower);
			EditorGUILayout.PropertyField(MPDroughness);
			if (GUILayout.Button("MPD"))
			{
				terrain.MidPointDisplacement();
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

		showSmooth = EditorGUILayout.Foldout(showSmooth, "Smooth");
		if (showSmooth)
		{
			EditorGUILayout.IntSlider(smoothAmount, 1, 10, new GUIContent("smoothAmount"));
			if (GUILayout.Button("Smooth"))
			{
				terrain.Smooth();
			}
		}

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		if (GUILayout.Button("Reset Terrain"))
		{
			terrain.ResetTerrain();
		}

		showHeights = EditorGUILayout.Foldout(showLoadHeights, "Height Map");
		if (showHeights)
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			int hmtSize = (int)(EditorGUIUtility.currentViewWidth - 100);
			GUILayout.Label(hmTexture, GUILayout.Width(hmtSize), GUILayout.Height(hmtSize));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Refresh", GUILayout.Width(hmtSize)))
			{
				float[,] heightMap = terrain.terrainData.GetHeights(0, 0,
					terrain.terrainData.heightmapResolution,
					terrain.terrainData.heightmapResolution);
				for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
				{
					for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
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
		serializedObject.ApplyModifiedProperties();
	}

}
