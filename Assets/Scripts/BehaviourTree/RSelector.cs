using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{
    bool shuffled = false;

    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        //make sure one Rselector only shuffles once
        if (!shuffled)
        {
            children.Shuffle();
            shuffled = true;
        }

        Status childstatus = children[currentChild].Process();
        if(childstatus == Status.RUNNING)
        {
            Debug.Log("Running:" + children[currentChild].name);
            return Status.RUNNING;
        }

        if(currentChild < children.Count)
        {
            if(childstatus == Status.SUCCESS)
            {
                Debug.Log("Success:" + children[currentChild].name);
                currentChild = 0;
                shuffled = false;
                return Status.SUCCESS;
            }

            currentChild++;
        }

        if(currentChild >= children.Count)
        {
            Debug.Log("Failure:" + children[currentChild-1].name);
            currentChild = 0;
            shuffled = false;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
