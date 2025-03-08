using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RandomizeIce : EditorWindow
{
    private List<Material> materials = new List<Material>();
    private List<float> materialProbabilities = new List<float>();
    private float minX = -3f, maxX = 3f, minY = -3f, maxY = 3f, minZ = -3f, maxZ = 3f;

    [MenuItem("Tools/Randomize Ice Crystals")]
    public static void ShowWindow()
    {
        GetWindow<RandomizeIce>("Crystal Randomizer");
    }

    void OnEnable()
    {
        LoadMaterials();
    }

    void OnGUI()
    {
        GUILayout.Label("Randomize Ice Crystals", EditorStyles.boldLabel);

        if (materials.Count == 0)
        {
            GUILayout.Label("No materials found in the specified folder.");
        }
        else
        {
            for (int i = 0; i < materials.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(materials[i].name, GUILayout.Width(200));
                materialProbabilities[i] = EditorGUILayout.FloatField("Probability", materialProbabilities[i]);
                GUILayout.Label($"{(materialProbabilities[i] / GetTotalProbability() * 100):F2}%");
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Label("Rotation Bounds", EditorStyles.boldLabel);
        minX = EditorGUILayout.FloatField("Min X Rotation", minX);
        maxX = EditorGUILayout.FloatField("Max X Rotation", maxX);
        minY = EditorGUILayout.FloatField("Min Y Rotation", minY);
        maxY = EditorGUILayout.FloatField("Max Y Rotation", maxY);
        minZ = EditorGUILayout.FloatField("Min Z Rotation", minZ);
        maxZ = EditorGUILayout.FloatField("Max Z Rotation", maxZ);

        if (GUILayout.Button("Apply Randomization"))
        {
            NormalizeProbabilities();
            ApplyRandomization();
        }
    }

    void LoadMaterials()
    {
        materials.Clear();
        materialProbabilities.Clear();

        string path = "Assets/SineVFX/TranslucentCrystals/Resources/Materials";
        string[] materialPaths = Directory.GetFiles(path, "*.mat", SearchOption.TopDirectoryOnly);

        foreach (string matPath in materialPaths)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (mat != null)
            {
                materials.Add(mat);
                materialProbabilities.Add(1f); // Default probability
            }
        }
    }

    void ApplyRandomization()
    {
        GameObject[] iceObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> filteredIceObjects = new List<GameObject>();

        foreach (GameObject obj in iceObjects)
        {
            if (obj.name.StartsWith("Ice"))
            {
                filteredIceObjects.Add(obj);
            }
        }

        if (filteredIceObjects.Count == 0)
        {
            Debug.LogWarning("No Ice objects found in the scene!");
            return;
        }

        int crystalCount = 0;

        foreach (GameObject iceObject in filteredIceObjects)
        {
            foreach (Transform wallTransform in iceObject.transform)
            {
                if (wallTransform.name.Contains("wall"))
                {
                    foreach (Transform crystal in wallTransform)
                    {
                        if (crystal.TryGetComponent(out MeshRenderer renderer))
                        {
                            Vector3 originalRotation = crystal.rotation.eulerAngles;
                            Vector3 closestStandardRotation = GetClosestStandardRotation(originalRotation);

                            float randomXRotation = RandomNormal(minX, maxX) + closestStandardRotation.x;
                            float randomYRotation = RandomNormal(minY, maxY) + closestStandardRotation.y;
                            float randomZRotation = RandomNormal(minZ, maxZ) + closestStandardRotation.z;

                            crystal.rotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);

                            if (materials.Count > 0)
                            {
                                Material chosenMaterial = GetRandomMaterial();
                                renderer.material = chosenMaterial;
                            }

                            crystalCount++;
                        }
                    }
                }
            }
        }

        Debug.Log($"Randomization applied to {crystalCount} crystals!");
    }

    Vector3 GetClosestStandardRotation(Vector3 rotation)
    {
        float[] standardAngles = { -360, -270, -180, -90, 0, 90, 180, 270, 360 };
        float closestX = GetClosestValue(rotation.x, standardAngles);
        float closestY = GetClosestValue(rotation.y, standardAngles);
        float closestZ = GetClosestValue(rotation.z, standardAngles);
        return new Vector3(closestX, closestY, closestZ);
    }

    float GetClosestValue(float value, float[] options)
    {
        float closest = options[0];
        float minDiff = Mathf.Abs(value - closest);
        foreach (float option in options)
        {
            float diff = Mathf.Abs(value - option);
            if (diff < minDiff)
            {
                closest = option;
                minDiff = diff;
            }
        }
        return closest;
    }

    void NormalizeProbabilities()
    {
        float total = GetTotalProbability();
        if (total > 0)
        {
            for (int i = 0; i < materialProbabilities.Count; i++)
            {
                materialProbabilities[i] /= total;
            }
        }
    }

    float GetTotalProbability()
    {
        float total = 0;
        foreach (float prob in materialProbabilities)
        {
            total += prob;
        }
        return total;
    }

    float RandomNormal(float min, float max)
    {
        float mean = (max + min) / 2f;
        float stdDev = (max - min) / 6f;
        float rand;
        do
        {
            rand = mean + stdDev * (Mathf.Sqrt(-2.0f * Mathf.Log(Random.value)) * Mathf.Sin(2.0f * Mathf.PI * Random.value));
        } while (rand < min || rand > max);
        return rand;
    }

    Material GetRandomMaterial()
    {
        float randomPoint = Random.value;
        for (int i = 0; i < materials.Count; i++)
        {
            if (randomPoint < materialProbabilities[i])
            {
                return materials[i];
            }
            randomPoint -= materialProbabilities[i];
        }
        return materials[materials.Count - 1];
    }
}