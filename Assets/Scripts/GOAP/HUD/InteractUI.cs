using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    public GameObject toiletParent;
    public GameObject cubicleParent;

    public GameObject NursePrefab;
    public GameObject JanitorPrefab;

    public Transform bornPoint;

    public NavMeshSurface surface;

    int maxToilet = 10;
    int maxCubicle = 9;

    int toiletNumber = 0;
    int cubicleNumber = 0;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI satisText;
    int satisNumber;
    float moneyNumber;
    public int moneyRate = 100;

    public GameObject moneyWarn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Dictionary<string, int> worldstates = GWorld.Instance.GetWorld().GetStates();


        foreach (KeyValuePair<string, int> s in worldstates)
        {
            //states.text += s.Key + "," + s.Value + "\n";

            if(s.Key == "FreePuddle")
            {
                satisNumber = 100 - s.Value * 3;
                satisText.text = satisNumber.ToString() + "%";
            }
        }

        moneyNumber += Time.deltaTime * moneyRate * CheckSatisfaction(satisNumber);
        moneyText.text = Mathf.RoundToInt(moneyNumber).ToString();
    }

    public void NursePlusOne()
    {
        if (moneyNumber > 10000)
        {
            moneyNumber -= 10000;
            Instantiate(NursePrefab, bornPoint.position, Quaternion.identity);
        }
        else
        {
            StartCoroutine(ShowAndHideWarn());
            Debug.Log("no money");
        }

    }

    public void JanitorPlusOne()
    {
        if(moneyNumber > 10000)
        {
            moneyNumber -= 10000;
            Instantiate(JanitorPrefab, bornPoint.position, Quaternion.identity);
        }
        else
        {
            StartCoroutine(ShowAndHideWarn());
            Debug.Log("no money");
        }
    }

    public void ToiletPlusOne()
    {
        if (moneyNumber > 10000)
        {
            moneyNumber -= 10000;
            toiletNumber++;
            if (toiletNumber + 3 <= maxToilet)
            {
                GameObject newToilet = toiletParent.transform.GetChild(toiletNumber + 2).gameObject;
                if (newToilet.activeInHierarchy == false)
                {
                    newToilet.SetActive(true);
                    GWorld.Instance.GetQueue("toilets").AddResource(newToilet);
                    GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);

                    newToilet = null;
                }
            }
            else
            {
                StartCoroutine(ShowAndHideWarn());
                Debug.Log("No toilet room");
            }

            surface.BuildNavMesh();
        }
        else
        {
            StartCoroutine(ShowAndHideWarn());
            Debug.Log("no money");
        }

    }

    public void CubiclePlusOne()
    {
        if (moneyNumber > 10000)
        {
            moneyNumber -= 10000;
            cubicleNumber++;
            if (cubicleNumber + 3 <= maxCubicle)
            {
                GameObject newCubicle = cubicleParent.transform.GetChild(cubicleNumber + 2).gameObject;
                if (newCubicle.activeInHierarchy == false)
                {
                    newCubicle.SetActive(true);
                    GWorld.Instance.GetQueue("cubicles").AddResource(newCubicle);
                    GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);

                    newCubicle = null;
                }
            }
            else
            {
                Debug.Log("No Cubicle room");
            }

            surface.BuildNavMesh();
        }
        else
        {
            StartCoroutine(ShowAndHideWarn());
            Debug.Log("no money");
        }

        
    }

    float CheckSatisfaction(int satis)
    {
        if(satis >= 90)
        {
            return 1.0f;
        }
        else if(satis < 90 && satis >= 70)
        {
            return 0.8f;
        }

        return 0.5f;
    }

    IEnumerator ShowAndHideWarn()
    {

        moneyWarn.SetActive(true);

        yield return new WaitForSeconds(2f);

        moneyWarn.SetActive(false);
    }

}
