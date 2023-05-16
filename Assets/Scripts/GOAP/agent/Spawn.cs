using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //public GameObject patientPrefab;
    public GameObject[] variousPatiensPrefab;
    public int numPatients;
    public bool keepSpawing = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numPatients; i++)
        {
            ChooseAndSpawn();
        }

        if (keepSpawing)
        {
            Invoke("SpawnPatient", 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPatient()
    {
        ChooseAndSpawn();
        Invoke("SpawnPatient", Random.Range(8, 10));
    }

    void ChooseAndSpawn()
    {
        int length = variousPatiensPrefab.Length;
        int rand = Random.Range(0, length);
        Instantiate(variousPatiensPrefab[rand], this.transform.position, Quaternion.identity);
    }
}
