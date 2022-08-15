using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshRendererExtensions
{
    public static void ApplyRandomMaterial(this MeshRenderer renderer, string shaderName, string gameObjectName)
    {
        renderer.material = new Material(Shader.Find(shaderName));
        renderer.material.name = $"{gameObjectName}_material";
        renderer.material.color = GetRandomColor();
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetInt("_Cull", 0); // why standard ?
        renderer.material.SetColor("_EmissionColor", renderer.material.color);
        Debug.Log("ApplyRandomMaterial color " + renderer.material.color);
    }

    public static Material[] GetRandomMaterials(string shaderName, int count)
	{
        Material[] materials = new Material[count];
        for (int i=0; i<count; i++)
		{
            Material randomMaterial = new Material(Shader.Find(shaderName));
            randomMaterial.name = $"{i}_material";
            randomMaterial.color = GetRandomColor();
            randomMaterial.EnableKeyword("_EMISSION");
            randomMaterial.SetInt("_Cull", 0);
            randomMaterial.SetColor("_EmissionColor", randomMaterial.color);
            materials[i] = randomMaterial;
            Debug.Log("GetRandomMaterials" + i);
		}
        return materials;
	}
    public static Color GetRandomColor() => Random.ColorHSV(0, 1f, 1f, 1f, 0.5f, 1f);

}
