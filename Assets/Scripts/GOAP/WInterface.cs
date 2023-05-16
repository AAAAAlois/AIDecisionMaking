using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WInterface : MonoBehaviour
{
    GameObject focusObj;
    public GameObject newResourcePrefab;
    Vector3 goalPos;

    public NavMeshSurface surface;
    public GameObject hospital;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray,out hit))
            {
                return;
            }

            goalPos = hit.point;

            focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
        }

        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            focusObj.transform.parent = hospital.transform;
            surface.BuildNavMesh();
            
            GWorld.Instance.GetQueue("toilets").AddResource(focusObj);
            GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);

            focusObj = null;
        }

        else if(focusObj && Input.GetMouseButtonDown(0))
        {
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove))
            {
                return;
            }

            goalPos = hitMove.point;

            focusObj.transform.position = goalPos;
        }
    }
}
