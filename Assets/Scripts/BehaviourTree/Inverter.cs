using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    public Inverter(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[0].Process();

        if(childstatus == Status.RUNNING)
        {
            Debug.Log("Running:" + children[currentChild].name);
            return Status.RUNNING;
        }

        if (childstatus == Status.FAILURE)
        {
            Debug.Log("Success:" + children[currentChild].name);
            return Status.SUCCESS;
        }

        else
        {
            Debug.Log("Failure:" + children[currentChild].name);
            return Status.FAILURE;
        }

    }
}
