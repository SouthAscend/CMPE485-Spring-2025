using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMaps
{
    public static List<AgentVariables> rocks = new List<AgentVariables>();
    public static List<AgentVariables> papers = new List<AgentVariables>();
    public static List<AgentVariables> scissors = new List<AgentVariables>();

    public static void InsertAgent(string type, AgentVariables agent)
    {
        List<AgentVariables> targetList = null;
        switch (type.ToLower())
        {
            case "rock":
                targetList = rocks;
                break;
            case "paper":
                targetList = papers;
                break;
            case "scissors":
                targetList = scissors;
                break;
            default:
                Debug.LogError($"Unknown agent type: {type}");
                return;
        }
        // Insert in sorted order by X position
        float x = agent.transform.position.x;
        int index = targetList.FindIndex(a => a.transform.position.x > x);
        if (index == -1)
            targetList.Add(agent);
        else
            targetList.Insert(index, agent);
    }
}
