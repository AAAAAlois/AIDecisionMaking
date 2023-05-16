using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : GAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("research", 1, false);
        goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("relief", 1, false);
        goals.Add(s2, 3);

        SubGoal s3 = new SubGoal("rested", 1, false);
        goals.Add(s3, 2);

        Invoke("GetTired", Random.Range(15, 20));
        Invoke("NeedRelief", Random.Range(2, 6));
    }

    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke("GetTired", Random.Range(15, 20));
    }

    void NeedRelief()
    {
        beliefs.ModifyState("busting", 0);
        Invoke("NeedRelief", Random.Range(2, 6));
    }
}
