using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;
    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;

    public Node.Status treeStatus = Node.Status.RUNNING;

    WaitForSeconds waitForSeconds;
    Vector3 rememberedLocation;

    // Start is called before the first frame update
    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();

        waitForSeconds = new WaitForSeconds(Random.Range(0.1f,1f));
        StartCoroutine("Behave");
        
    }

    // Update is called once per frame
    public void Update()
    {
        //if(treeStatus != Node.Status.SUCCESS)
        //{
            //treeStatus = tree.Process();
        //}

    }

    IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }

   
    public Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);

        if(state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if(distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<LockDoor>().isDoorLocked)    //don't need to use collider to check if the door is locked
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
        {
            return s;
        }
    }

    public Node.Status CanSee(Vector3 target, string tag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - this.transform.position;
        float angel = Vector3.Angle(directionToTarget, this.transform.forward);

        if(angel <= maxAngle || directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(this.transform.position, directionToTarget, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag(tag))    //check if hit the wall
                {
                    return Node.Status.SUCCESS;
                }
            }
        }

        return Node.Status.FAILURE;
    }

    public Node.Status Flee(Vector3 location, float distance)
    {
        if (state == ActionState.IDLE)
        {
            rememberedLocation = this.transform.position + (transform.position - location).normalized * distance;   //store the original location cuz the robber's position always changes
        }
        return GoToLocation(rememberedLocation);
    }

    public Node.Status IsOpen()
    {
        if (Blackboard.Instance.timeOfDay < 9 || Blackboard.Instance.timeOfDay > 17)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }

}
