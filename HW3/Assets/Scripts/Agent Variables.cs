using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentVariables : MonoBehaviour
{
    public float speed = 1f;
    public float range = 10f;
    public float predatorBoost = 1.05f;
    public string type = "rock";

    public LinkedListNode<AgentVariables> nodeInGlobalList;

    
    private GameObject mapObject;
    private Vector2 mapMinBound;
    private Vector2 mapMaxBound;
    private SpriteRenderer typeRenderer;
    private SpriteRenderer bgRenderer;
    private Sprite rockSprite;
    private Sprite paperSprite;
    private Sprite scissorsSprite;

    private float ratioBoost = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mapObject = GameObject.Find("Map");
        if (mapObject != null)
        {
            // Get the first tile
            Transform firstTile = mapObject.transform.Find("Tile_0_0");
            
            // Get the last tile by finding the last child
            Transform lastTile = mapObject.transform.GetChild(mapObject.transform.childCount - 1);
            
            if (firstTile != null && lastTile != null)
            {
                // Since tiles are 2 units apart and pivots are at center
                // We need to add/subtract 1 unit to get the actual boundaries
                mapMinBound = firstTile.position - Vector3.one;
                mapMaxBound = lastTile.position + Vector3.one;
            }
            else
            {
                Debug.LogError("First or last tile not found!");
            }
        }
        else
        {
            Debug.LogError("Map GameObject not found!");
        }

        // Get sprite renderers
        typeRenderer = transform.Find("Type")?.GetComponent<SpriteRenderer>();
        bgRenderer = transform.Find("Background")?.GetComponent<SpriteRenderer>();

        // Load sprites
        rockSprite = Resources.Load<Sprite>("Images/Rock");
        paperSprite = Resources.Load<Sprite>("Images/Paper");
        scissorsSprite = Resources.Load<Sprite>("Images/Scissors");

        ratioBoost = 1/(mapObject.GetComponent<AIManager>().agentRatio);
    }

    private void GetEaten(string newType)
    {
        GlobalMaps.RemoveAgent(this);
        type = newType;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (typeRenderer && bgRenderer)
        {
            typeRenderer.color = Color.white;
            switch (type)
            {
                case "rock":
                    typeRenderer.sprite = rockSprite;
                    bgRenderer.color = Color.yellow;
                    GlobalMaps.InsertAgent("rock", this);
                    break;
                case "paper":
                    typeRenderer.sprite = paperSprite;
                    bgRenderer.color = new Color(0.6f, 1f, 0.6f);
                    GlobalMaps.InsertAgent("paper", this);
                    break;
                case "scissors":
                    typeRenderer.sprite = scissorsSprite;
                    bgRenderer.color = new Color(0.6f, 0.8f, 1f);
                    GlobalMaps.InsertAgent("scissors", this);
                    break;
            }
        }
    }

    public void Decide()
    {

        float[] preyMap = InitializeVisionMap("prey", transform.position);
        float[] predatorMap = InitializeVisionMap("predator", transform.position);
        MoveBasedOnVision(preyMap, predatorMap);
        GlobalMaps.UpdateAgentPosition(this);
    }

    private void MoveBasedOnVision(float[] preyMap, float[] predatorMap)
    {
        float maxScore = float.MinValue;
        int maxAngle = 0;
        Vector2 nextPosition = Vector2.zero;

        for (int i = 0; i < 90; i++)
        {
            int angle = i * 4;
            float score = preyMap[i] * predatorBoost + predatorMap[i];
            
            if (score > maxScore || (score == maxScore && Random.Range(0, 45) == 0))
            {
                // Calculate potential movement
                Vector2 moveDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                float moveSpeed = (predatorMap[i] > preyMap[i]) ? speed * predatorBoost : speed;
                moveSpeed *= ratioBoost;
                Vector2 potentialPosition = (Vector2)transform.position + moveDir.normalized * moveSpeed * Time.deltaTime;

                // Only update max score if the potential position is within bounds
                if (IsWithinMapBounds(potentialPosition))
                {
                    maxScore = score;
                    maxAngle = angle;
                    nextPosition = potentialPosition;
                }
            }
        }

        transform.position = nextPosition;
        Eat(nextPosition);
    }

    private void Eat(Vector3 position)
    {
        var preyList = GlobalMaps.getPreyList(type);
        if (preyList == null) return;

        float halfSize = transform.localScale.x / 2.0f;
        float minX = position.x - halfSize - 1.2f;
        float maxX = position.x + halfSize + 1.2f;

        List<AgentVariables> foods = new List<AgentVariables>();

        var node = preyList.First;
        while (node != null)
        {
            var prey = node.Value;
            float preyX = prey.transform.position.x;

            if (preyX < minX)
            {
                node = node.Next;
                continue;
            }

            if (preyX > maxX) break;

            Vector3 preyPos = prey.transform.position;
            float preyHalfSize = prey.transform.localScale.x / 2.0f;

            if (Mathf.Abs(preyPos.y - position.y) <= halfSize + preyHalfSize &&
                Mathf.Abs(preyPos.x - position.x) <= halfSize + preyHalfSize)
            {
                foods.Add(prey);
            }

            node = node.Next;
        }

        foreach (var food in foods)
        {
            food.GetEaten(type);
        }
    }



    private bool IsWithinMapBounds(Vector2 position)
    {
        if (mapObject == null) return true; // If no map, allow movement

        return position.x >= mapMinBound.x &&
               position.x <= mapMaxBound.x &&
               position.y >= mapMinBound.y &&
               position.y <= mapMaxBound.y;
    }

    private float[] InitializeVisionMap(string functionType, Vector2 position)
    {
        float[] visionMap = new float[90];
        LinkedList<AgentVariables> agentList = null;

        switch (functionType)
        {
            case "prey":
                agentList = GlobalMaps.getPreyList(type);
                break;
            case "predator":
                agentList = GlobalMaps.getPredatorList(type);
                break;
        }

        if (agentList == null) return visionMap;

        float minDist = transform.localScale.x; // max influence
        float maxDist = range; // no influence

        var node = agentList.First;
        while (node != null)
        {
            var agent = node.Value;
            Vector2 agentPos = agent.transform.position;

            if (Mathf.Abs(agentPos.y - position.y) > range)
            {
                node = node.Next;
                continue;
            }

            Vector2 toAgent = agentPos - position;
            float dist = toAgent.magnitude;
            if (dist > range || dist < minDist)
            {
                node = node.Next;
                continue;
            }

            float angle = Mathf.Atan2(toAgent.y, toAgent.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            float distFactor = 1f - ((dist - minDist) / (maxDist - minDist)); // 1 near, 0 far

            for (int delta = -40; delta <= 40; delta += 4)
            {
                int targetAngle = (Mathf.RoundToInt((angle + delta) / 4f) + 90) % 90;
                float angleFalloff = 1f - Mathf.Abs(delta) / 40f;
                float influence = distFactor * angleFalloff;
                visionMap[targetAngle] = Mathf.Max(visionMap[targetAngle], influence);
            }

            node = node.Next;
        }

        if (functionType == "predator")
        {
            float[] tempMap = new float[90];
            for (int i = 0; i < 90; i++)
            {
                tempMap[i] = visionMap[89 -i];
            }
            visionMap = tempMap;
        }

        return visionMap;
    }

}
