using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : GAction
{
    public override bool PrePerform()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        return true;
    }

    public override bool PostPerform()
    {
        Destroy(this.gameObject);
        return true;
    }
}
