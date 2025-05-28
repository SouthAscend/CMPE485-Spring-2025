using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMap : MonoBehaviour
{
    [SerializeField] private GameObject agentPrefab;
    // Sprites will be loaded dynamically from Resources/Images

    void Update()
    {
        GlobalMaps.UpdateFrameTime();
    }

    public void Initialize(int value)
    {
        // Get the AI Manager component from the Map object
        AIManager aiManager = GetComponent<AIManager>();
        if (aiManager == null)
        {
            Debug.LogError("AI Manager component not found on Map object!");
            return;
        }

        // Dynamically load sprites
        Sprite rockSprite = Resources.Load<Sprite>("Images/Rock");
        Sprite paperSprite = Resources.Load<Sprite>("Images/Paper");
        Sprite scissorsSprite = Resources.Load<Sprite>("Images/Scissors");
        if (!rockSprite || !paperSprite || !scissorsSprite)
        {
            Debug.LogError("One or more sprites could not be loaded from Resources/Images!");
            return;
        }
        Debug.Log($"InitializeMap.Initialize called with value: {value}");
        Transform tile = transform.Find("Tile");
        if (tile == null)
        {
            Debug.LogError("Tile child not found!");
            return;
        }
        int m = Mathf.CeilToInt(Mathf.Sqrt(3 * value));
        Vector3 basePos = tile.localPosition;
        List<Vector2> tileCenters = new List<Vector2>();
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Vector3 offset = new Vector3(i * 2, j * 2, basePos.z);
                if (i == 0 && j == 0)
                {
                    tile.localPosition = basePos;
                    tile.name = "Tile_0_0";
                }
                else
                {
                    Instantiate(tile.gameObject, basePos + offset, tile.rotation, transform).name = $"Tile_{i}_{j}";
                }
                tileCenters.Add(new Vector2(basePos.x + i * 2, basePos.y + j * 2));
            }
        }
        // Move and resize the Main Camera to fit the map
        Transform mainCamera = transform.Find("Main Camera");
        if (mainCamera != null)
        {
            float centerX = basePos.x + (m - 1) * 2 / 2f;
            float centerY = basePos.y + (m - 1) * 2 / 2f;
            mainCamera.position = new Vector3(centerX, centerY, mainCamera.position.z);
            Camera cam = mainCamera.GetComponent<Camera>();
            if (cam != null && cam.orthographic)
            {
                float mapHalfSize = m * 2 / 2f;
                cam.orthographicSize = Mathf.CeilToInt(mapHalfSize * 1.1f);
            }
        }
        // Prepare shuffled queue for agent placement
        int tileCount = m * m;
        List<int> indices = new List<int>();
        for (int i = 0; i < tileCount; i++) indices.Add(i);
        System.Random rng = new System.Random();
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int swap = rng.Next(i + 1);
            int temp = indices[i];
            indices[i] = indices[swap];
            indices[swap] = temp;
        }
        Queue<int> tileQueue = new Queue<int>(indices);
        // Place 3n agents
        for (int k = 0; k < 3 * value; k++)
        {
            if (tileQueue.Count == 0) break;
            int idx = tileQueue.Dequeue();
            Vector2 center = tileCenters[idx];
            float randX = UnityEngine.Random.Range(-0.4f, 0.4f);
            float randY = UnityEngine.Random.Range(-0.4f, 0.4f);
            Vector3 agentPos = new Vector3(center.x + randX, center.y + randY, 0);
            GameObject agent = Instantiate(agentPrefab, agentPos, Quaternion.identity);
            // Set type and background
            Transform typeChild = agent.transform.Find("Type");
            Transform bgChild = agent.transform.Find("Background");
            SpriteRenderer typeRenderer = typeChild?.GetComponent<SpriteRenderer>();
            SpriteRenderer bgRenderer = bgChild?.GetComponent<SpriteRenderer>();
            AgentVariables vars = agent.GetComponent<AgentVariables>();
            if (k < value)
            {
                if (typeRenderer) typeRenderer.sprite = rockSprite;
                if (bgRenderer) bgRenderer.color = Color.yellow;
                vars.type = "rock";
            }
            else if (k < 2 * value)
            {
                if (typeRenderer) typeRenderer.sprite = paperSprite;
                if (bgRenderer) bgRenderer.color = new Color(0.6f, 1f, 0.6f); // light green
                vars.type = "paper";
            }
            else
            {
                if (typeRenderer) typeRenderer.sprite = scissorsSprite;
                if (bgRenderer) bgRenderer.color = new Color(0.6f, 0.8f, 1f); // light blue
                vars.type = "scissors";
            }
            // Randomize agent variables
            float scale = UnityEngine.Random.Range(0.8f, 1.2f);
            agent.transform.localScale = new Vector3(scale, scale, 1f);
            if (vars != null)
            {
                float x = UnityEngine.Random.Range(0.8f, 1.2f);
                if (x > 1.0f) x *= 1.25f;
                vars.speed = x;
                vars.range = UnityEngine.Random.Range(9f, 11f);
                vars.predatorBoost = UnityEngine.Random.Range(1.0f, 1.1f);
                // Insert into global maps as soon as agent is created
                if (k < value)
                    GlobalMaps.InsertAgent("rock", vars);
                else if (k < 2 * value)
                    GlobalMaps.InsertAgent("paper", vars);
                else
                    GlobalMaps.InsertAgent("scissors", vars);
                
                // Add the agent to the AI Manager's queue
                aiManager.AddAgent(vars);
            }
        }
        // After all agents are created, print the lengths of the global maps
        Debug.Log($"Rocks: {GlobalMaps.rocks.Count}, Papers: {GlobalMaps.papers.Count}, Scissors: {GlobalMaps.scissors.Count}");

        GlobalMaps.InitializeSimulation();
    }
}
