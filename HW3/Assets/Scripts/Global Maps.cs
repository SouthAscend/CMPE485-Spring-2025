using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMaps
{
    public static LinkedList<AgentVariables> rocks = new LinkedList<AgentVariables>();
    public static LinkedList<AgentVariables> papers = new LinkedList<AgentVariables>();
    public static LinkedList<AgentVariables> scissors = new LinkedList<AgentVariables>();

    // FPS tracking variables
    private static float totalFrameTime = 0f;
    private static int frameCount = 0;

    public static void InitializeSimulation()
    {
        totalFrameTime = 0f;
        frameCount = 0;
    }

    public static void UpdateFrameTime()
    {
        totalFrameTime += Time.deltaTime;
        frameCount++;
        if (totalFrameTime > 60f) {
            SimulationOver();
        }
    }

    public static void InsertAgent(string type, AgentVariables agent)
    {
        LinkedList<AgentVariables> targetList = null;

        switch (type.ToLower())
        {
            case "rock": targetList = rocks; break;
            case "paper": targetList = papers; break;
            case "scissors": targetList = scissors; break;
            default:
                Debug.LogError($"Unknown agent type: {type}");
                return;
        }

        float x = agent.transform.position.x;

        // Find insert position (sorted by X)
        var node = targetList.First;
        while (node != null && node.Value.transform.position.x < x)
        {
            node = node.Next;
        }

        LinkedListNode<AgentVariables> newNode;
        if (node == null)
            newNode = targetList.AddLast(agent);
        else
            newNode = targetList.AddBefore(node, agent);

        agent.nodeInGlobalList = newNode;
    }

    public static LinkedList<AgentVariables> getPreyList(string type)
    {
        switch (type)
        {
            case "rock": return scissors;
            case "paper": return rocks;
            case "scissors": return papers;
            default: return null;
        }
    }

    public static LinkedList<AgentVariables> getPredatorList(string type)
    {
        switch (type)
        {
            case "rock": return papers;
            case "paper": return scissors;
            case "scissors": return rocks;
            default: return null;
        }
    }

    public static void RemoveAgent(AgentVariables agent)
    {
        if (agent.nodeInGlobalList != null)
        {
            var list = GetListByType(agent.type);
            if (list != null)
                list.Remove(agent.nodeInGlobalList);
            agent.nodeInGlobalList = null;
            if (list.Count == 0) {
                SimulationOver();
            }
        }
    }

    public static void UpdateAgentPosition(AgentVariables agent)
    {
        var node = agent.nodeInGlobalList;
        if (node == null) return;

        var list = GetListByType(agent.type);
        if (list == null) return;

        // Bubble left
        while (node.Previous != null &&
               node.Value.transform.position.x < node.Previous.Value.transform.position.x)
        {
            var prev = node.Previous;
            // Swap the Values
            var temp = node.Value;
            node.Value = prev.Value;
            prev.Value = temp;

            // Update their nodeInGlobalList references
            node.Value.nodeInGlobalList = node;
            prev.Value.nodeInGlobalList = prev;
        }

        // Bubble right
        while (node.Next != null &&
               node.Value.transform.position.x > node.Next.Value.transform.position.x)
        {
            var next = node.Next;
            // Swap the Values
            var temp = node.Value;
            node.Value = next.Value;
            next.Value = temp;

            // Update their nodeInGlobalList references
            node.Value.nodeInGlobalList = node;
            next.Value.nodeInGlobalList = next;
        }
    }

    private static LinkedList<AgentVariables> GetListByType(string type)
    {
        switch (type)
        {
            case "rock": return rocks;
            case "paper": return papers;
            case "scissors": return scissors;
            default: return null;
        }
    }

    private static void SimulationOver()
    {
        float averageFPS = frameCount / totalFrameTime;
        Debug.Log($"Simulation Over - Average FPS: {averageFPS:F2}");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
