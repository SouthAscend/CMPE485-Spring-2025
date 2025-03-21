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
        CalculateRotationBounds();
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

    void CalculateRotationBounds()
    {
        float maxDeviationX = 0f, maxDeviationY = 0f, maxDeviationZ = 0f;

        GameObject[] iceObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> filteredIceObjects = new List<GameObject>();

        foreach (GameObject obj in iceObjects)
        {
            if (obj.name.StartsWith("Ice"))
            {
                filteredIceObjects.Add(obj);
            }
        }

        List<Transform> allCrystals = new List<Transform>();

        foreach (GameObject iceObject in filteredIceObjects)
        {
            foreach (Transform wallTransform in iceObject.transform)
            {
                if (wallTransform.name.Contains("wall"))
                {
                    foreach (Transform crystal in wallTransform)
                    {
                        allCrystals.Add(crystal);
                    }
                }
            }
        }

        if (allCrystals.Count == 0)
        {
            return; // No crystals found, leave existing values
        }

        foreach (Transform crystal in allCrystals)
        {
            Vector3 originalRotation = crystal.rotation.eulerAngles;
            Vector3 closestStandardRotation = GetClosestStandardRotation(originalRotation);

            float deviationX = Mathf.Abs(originalRotation.x - closestStandardRotation.x);
            float deviationY = Mathf.Abs(originalRotation.y - closestStandardRotation.y);
            float deviationZ = Mathf.Abs(originalRotation.z - closestStandardRotation.z);

            maxDeviationX = Mathf.Max(maxDeviationX, deviationX);
            maxDeviationY = Mathf.Max(maxDeviationY, deviationY);
            maxDeviationZ = Mathf.Max(maxDeviationZ, deviationZ);
        }

        // Assign max deviations to min/max (keeping the input fields intact)
        minX = -maxDeviationX;
        maxX = maxDeviationX;
        minY = -maxDeviationY;
        maxY = maxDeviationY;
        minZ = -maxDeviationZ;
        maxZ = maxDeviationZ;
    }


    void LoadMaterials()
    {
        materials.Clear();
        materialProbabilities.Clear();

        string path = "Assets/TranslucentCrystals/Resources/Materials";
        string[] materialPaths = Directory.GetFiles(path, "*.mat", SearchOption.TopDirectoryOnly);
        Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();

        // Load materials
        foreach (string matPath in materialPaths)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (mat != null)
            {
                materials.Add(mat);
                materialCounts[mat] = 0; // Initialize counter
            }
        }

        int totalCrystals = 0;

        // Find all "Ice" objects
        GameObject[] iceObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> filteredIceObjects = new List<GameObject>();

        foreach (GameObject obj in iceObjects)
        {
            if (obj.name.StartsWith("Ice"))
            {
                filteredIceObjects.Add(obj);
            }
        }

        // Iterate through Ice objects, Walls, and Crystals
        foreach (GameObject iceObject in filteredIceObjects)
        {
            foreach (Transform wallTransform in iceObject.transform)
            {
                if (wallTransform.name.Contains("wall")) // Ensure it's a wall
                {
                    foreach (Transform crystal in wallTransform)
                    {
                        if (crystal.TryGetComponent(out MeshRenderer renderer))
                        {
                            Material crystalMat = renderer.sharedMaterial;
                            if (crystalMat != null && materialCounts.ContainsKey(crystalMat))
                            {
                                materialCounts[crystalMat]++;
                                totalCrystals++;
                            }
                        }
                    }
                }
            }
        }

        // Assign probabilities based on usage count
        foreach (Material mat in materials)
        {
            float probability = totalCrystals > 0 ? (float)materialCounts[mat] * 100f / totalCrystals : 0.001f;
            materialProbabilities.Add(probability);
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