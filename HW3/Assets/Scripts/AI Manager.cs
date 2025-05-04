using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Queue<AgentVariables> agentQueue = new Queue<AgentVariables>();
    public float agentRatio = 1.0f;

    public void AddAgent(AgentVariables agent)
    {
        agentQueue.Enqueue(agent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agentQueue.Count == 0) return;

        // Calculate how many agents to process this frame based on the ratio
        int agentsToProcess = Mathf.CeilToInt(agentQueue.Count * agentRatio);
        agentsToProcess = Mathf.Min(agentsToProcess, agentQueue.Count);

        // Process the calculated number of agents
        for (int i = 0; i < agentsToProcess; i++)
        {
            AgentVariables agent = agentQueue.Dequeue();
            agent.Decide();
            agentQueue.Enqueue(agent); // Add the agent back to the queue for next frame
        }
    }
}
