using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();

        if(childstatus == Status.RUNNING)
        {
            Debug.Log("Running:" + children[currentChild].name);
            return Status.RUNNING;
        }

        if (childstatus == Status.FAILURE)
        {
            currentChild = 0;
            foreach(Node n in children)
            {
                n.Reset();
            }

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
