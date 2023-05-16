using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopBehaviours : BTAgent
{
    public GameObject[] copDestinations;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        Sequence selectCopDestinations = new Sequence("Select Cop Destinations");
        for (int i = 0; i < copDestinations.Length; i++)
        {
            Leaf goToArt = new Leaf("Go To " + copDestinations[i].name, i, GoToCopDestinations);
            selectCopDestinations.AddChild(goToArt);
        }

        tree.AddChild(selectCopDestinations);
    }

  
    public Node.Status GoToCopDestinations(int i)
    {
        Node.Status s = GoToLocation(copDestinations[i].transform.position);
        return s;
    }
}
