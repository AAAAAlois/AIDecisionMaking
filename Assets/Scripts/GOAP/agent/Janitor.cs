using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : GAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("clean", 1, false);
        goals.Add(s1, 3);

    }

   
}
