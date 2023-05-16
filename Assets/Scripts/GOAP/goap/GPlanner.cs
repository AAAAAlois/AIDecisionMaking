using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GNode
{
    public GNode parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public GNode(GNode parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;

    }

    public GNode(GNode parent, float cost, Dictionary<string, int> allstates, Dictionary<string, int> beliefstate, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        foreach(KeyValuePair<string, int>b in beliefstate)
        {
            if (!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }
        }
        this.action = action;

    }
}

public class GPlanner
{
   public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beilefstates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<GNode> leaves = new List<GNode>();
        GNode start = new GNode(null, 0, GWorld.Instance.GetWorld().GetStates(), beilefstates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("No plan");
            return null;
        }

        //find the cheapest action
        GNode cheapest = null;
        foreach(GNode leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        //insert the actions to result
        List<GAction> result = new List<GAction>();
        GNode n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }

        return queue;
    }

    private bool BuildGraph(GNode parent, List<GNode> leaves, List<GAction> useableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach(GAction action in useableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in action.effects)    //check if match
                {
                    if (!currentState.ContainsKey(eff.Key))
                    {
                        currentState.Add(eff.Key, eff.Value);
                    }
                }

                GNode node = new GNode(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(useableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }

        return subset;
    }
}
