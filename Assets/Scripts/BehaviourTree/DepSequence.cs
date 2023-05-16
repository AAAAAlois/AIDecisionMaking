using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DepSequence : Node
{
    BehaviourTree dependancy;
    NavMeshAgent agent;

    public DepSequence(string n, BehaviourTree d, NavMeshAgent a)
    {
        name = n;
        dependancy = d;
        agent = a;
    }

    public override Status Process()
    {
        if(dependancy.Process() == Status.FAILURE)  
        {
            agent.ResetPath();

            foreach(Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;
        }


        Status childstatus = children[currentChild].Process();

        if(childstatus == Status.RUNNING)
        {
            Debug.Log("Running:" + children[currentChild].name);
            return Status.RUNNING;
        }

        if (childstatus == Status.FAILURE)
        {
            Debug.Log("Failure:" + children[currentChild].name);
            return Status.FAILURE;
        }

        if (childstatus == Status.SUCCESS)
        {
            Debug.Log("Success:" + children[currentChild].name);
        }

        currentChild++;
        if(currentChild >= children.Count)  //successfully looped through all the children
        {
            Debug.Log("Success:" + children[currentChild-1].name);
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
