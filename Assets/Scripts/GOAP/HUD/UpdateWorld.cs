using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    public Text states;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Dictionary<string, int> worldstates = GWorld.Instance.GetWorld().GetStates();
        states.text = "";

        foreach(KeyValuePair<string, int> s in worldstates)
        {
            states.text += s.Key + "," + s.Value + "\n";
        }
    }
}
