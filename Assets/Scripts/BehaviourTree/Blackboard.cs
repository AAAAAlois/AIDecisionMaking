using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackboard : MonoBehaviour
{
    static Blackboard instance;
    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if(blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        instance = blackboards[0];
                        return instance;
                    }
                }

                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }

        set
        {
            instance = value as Blackboard;
        }
    }

    public float timeOfDay;
    public TextMeshProUGUI clock;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("UpdateClock");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            timeOfDay++;
            if(timeOfDay > 23)
            {
                timeOfDay = 0;
            }
            clock.text = timeOfDay + ":00";
            yield return new WaitForSeconds(6);
        }
    }
}
