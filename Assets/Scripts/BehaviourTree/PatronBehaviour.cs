using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronBehaviour : BTAgent
{
    public GameObject[] arts;
    public GameObject frontDoor;
    public GameObject home;

    [Range(0, 1000)]
    public int boredom = 0;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        RSelector selectObject = new RSelector("Select Art to View");
        for (int i = 0; i < arts.Length; i++)
        {
            Leaf goToArt = new Leaf("Go To " + arts[i].name, i, GoToArt);
            selectObject.AddChild(goToArt);
        }

        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goHome = new Leaf("Go Home", GoHome);
        Leaf isBored = new Leaf("Is Bored", IsBored);
        Leaf isOpen = new Leaf("Is Open", IsOpen);

        //BehaviourTree patronConditions = new BehaviourTree();
        //patronConditions.AddChild(isBored);

        //DepSequence ViewArt = new DepSequence("Visit Gallery", patronConditions, agent);
        Sequence ViewArt = new Sequence("View Art");
        ViewArt.AddChild(isOpen);
        ViewArt.AddChild(isBored);
        ViewArt.AddChild(goToFrontDoor);

        BehaviourTree whileBored = new BehaviourTree();
        whileBored.AddChild(isBored);

        Loop LookAtPaintings = new Loop("Look At Paitings", whileBored);
        LookAtPaintings.AddChild(selectObject);

        ViewArt.AddChild(LookAtPaintings);
        ViewArt.AddChild(goHome);

        BehaviourTree galleryOpenCondition = new BehaviourTree();
        galleryOpenCondition.AddChild(isOpen);

        DepSequence bePatron = new DepSequence("Be A Patron",galleryOpenCondition, agent);
        bePatron.AddChild(ViewArt);

        Selector viewArtWithFallBack = new Selector("Steal With Fall Back");
        viewArtWithFallBack.AddChild(bePatron);
        viewArtWithFallBack.AddChild(goHome);

        tree.AddChild(viewArtWithFallBack);

        StartCoroutine("IncreaseBoredom");

        tree.PrintTree();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    IEnumerator IncreaseBoredom()
    {
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Node.Status GoToArt(int i)
    {
        if (arts[i].activeInHierarchy == false)
        {
            return Node.Status.FAILURE;
        }

        Node.Status s = GoToLocation(arts[i].transform.position);

        if(s == Node.Status.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 150, 0, 1000);
        }

        return s;
        
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public Node.Status GoHome()
    {
        Node.Status s = GoToLocation(home.transform.position);
        return s;
    }

    public Node.Status IsBored()
    {
        if (boredom < 100)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }

}
