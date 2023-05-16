using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject cop;

    [Range(0, 1000)]
    public int money = 100;

    public GameObject diamond;
    public GameObject van;
    public GameObject backDoor;
    public GameObject frontDoor;
    public GameObject painting;

    public GameObject[] arts;

    GameObject pickup;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        Leaf isOpen = new Leaf("Is Open",IsOpen);
        Inverter isClosed = new Inverter("Is Closed");
        isClosed.AddChild(isOpen);

        Leaf hasEnoughMoney = new Leaf("Has Enough Money", HasMoney);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 2);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 1);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Leaf goToBackDoor = new Leaf("Go To BackDoor", GoToBackDoor, 2);
        Leaf goToFrontDoor = new Leaf("Go To FrontDoor", GoToFrontDoor, 1);

        Sequence runAway = new Sequence("Run Away");
        Leaf canSeeCop = new Leaf("Can See Cop", CanSeeCop);
        Leaf fleeFromCop = new Leaf("Flee From Cop", FleeFromCop);

        PSelector openDoor = new PSelector("Open Door");
        RSelector selectObject = new RSelector("Select Object to Steal");
        for (int i = 0; i < arts.Length; i++)
        {
            Leaf goToArt = new Leaf("Go To " + arts[i].name, i, GoToArt);
            selectObject.AddChild(goToArt);
        }

        Selector beThief = new Selector("Be Thief");

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasEnoughMoney);

        Inverter cantSeeCop = new Inverter("Can't See Cop");
        cantSeeCop.AddChild(canSeeCop);

        BehaviourTree stealConditions = new BehaviourTree();
        Sequence conditions = new Sequence("Stealing Conditions");
        conditions.AddChild(isClosed);
        conditions.AddChild(cantSeeCop);
        conditions.AddChild(invertMoney);
        stealConditions.AddChild(conditions);
        DepSequence steal = new DepSequence("Steal Something", stealConditions, agent);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);


        //steal.AddChild(hasEnoughMoney);
        //steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);

        Selector stealWithFallBack = new Selector("Steal With Fall Back");
        stealWithFallBack.AddChild(steal);
        stealWithFallBack.AddChild(goToVan);

        runAway.AddChild(canSeeCop);
        runAway.AddChild(fleeFromCop);

        beThief.AddChild(stealWithFallBack);
        beThief.AddChild(runAway);

        tree.AddChild(beThief);
        //tree.AddChild(steal);

        StartCoroutine("DecreaseMoney");


        tree.PrintTree();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

    }

    IEnumerator DecreaseMoney()
    {
        while (true)
        {
            money = Mathf.Clamp(money - 50, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Node.Status HasMoney()
    {
        if(money < 500)
        {
            return Node.Status.FAILURE;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToDiamond()
    {   
        if(diamond.activeInHierarchy == false)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            Node.Status s = GoToLocation(diamond.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                diamond.transform.parent = this.gameObject.transform;
                pickup = diamond;
            }
            return s;
        }
    }

    public Node.Status GoToPainting()
    {
        if (painting.activeInHierarchy == false)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            Node.Status s = GoToLocation(painting.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                painting.transform.parent = this.gameObject.transform;
                pickup = painting;
            }
            return s;
        }
    }

    public Node.Status GoToArt(int i)
    {
        if (arts[i].activeInHierarchy == false)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            Node.Status s = GoToLocation(arts[i].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                arts[i].transform.parent = this.gameObject.transform;
                pickup = arts[i];
            }
            return s;
        }
    }


    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            //Destroy(diamond);
            if(pickup != null)
            {
                pickup.SetActive(false);
                money += 300;
                pickup = null;
            }

        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }


    public Node.Status CanSeeCop()
    {
        return CanSee(cop.transform.position, "Cop", 10f, 60f);
    }

    public Node.Status FleeFromCop()
    {
        return Flee(cop.transform.position, 10f);
    }
}
