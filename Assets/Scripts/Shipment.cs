using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shipment : MonoBehaviour
{
    public float barrelCapacity = 2;
    public float BarrelCapacity
    {
        get
        {
           return barrelCapacity;
        }
        set
        {
            barrelCapacity = value;
        }
    }
    public float pricePerBarrel = 2;

    public float PricePerBarrel 
    {
        get
        {
            return pricePerBarrel;
        }
        set
        {
            pricePerBarrel = value;
        }
    }

    public float barrelRateOfIncrease = 1.5f;

    public float barrelROI
    {
        get
        {
            return barrelRateOfIncrease;
        }
        set
        {
            barrelRateOfIncrease = value;
        }
    }
    private float barrelMultiplier = 1.15f;

    public float MinBarrelPerMin = 23;
    public float timeBetweenBarrels;
    private float originalTime;
    public float waitTime;

    public Transform barrelSpawn;

    public static Shipment instance;

    public List<GameObject> refineList;
    public GameObject refineObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
    }

    private void Start()
    {


        int count = GameManager.instance.getRefineListCount();

        if (count > 1)
        {

            for (int i = 1; i < count; i++)
            {
                GameObject go = Instantiate(Resources.Load("Refinery_Up") as GameObject);
                go.transform.SetParent(Shipment.instance.refineObject.transform);
                go.GetComponent<RectTransform>().localScale = Shipment.instance.refineList[0].GetComponent<RectTransform>().localScale;
                if (Shipment.instance.refineList.Count == 1)
                    go.GetComponent<RectTransform>().position = new Vector2(Shipment.instance.refineList[i - 1].transform.position.x - 2.75f, Shipment.instance.refineList[i - 1].transform.position.y + 1.59f);
                else
                    go.GetComponent<RectTransform>().position = new Vector2(Shipment.instance.refineList[i - 1].transform.position.x - 1.59f, Shipment.instance.refineList[i - 1].transform.position.y + 0.95f);
                go.GetComponent<Button>().onClick = null;
                // Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].GetComponent<Button>().onClick = null;
                Shipment.instance.refineList.Add(go);
                UIManager.instance.dialog_Box.SetActive(false);
            }
        }

        timeBetweenBarrels = 60 / MinBarrelPerMin;
        originalTime = timeBetweenBarrels;
        waitTime = Time.time + timeBetweenBarrels;
    }

    bool changeWT = false;
    bool updateTime = false;
    private void Update()
    {
        ProduceBarrel();

        if(ConveyorBelt.instance.isHighSpeed)
        {
            if (changeWT == false)
            {
                changeWT = true;
                waitTime = Time.time + timeBetweenBarrels;
            }
            timeBetweenBarrels = originalTime / 4;

            if(!updateTime)
            {
                updateTime = true;
                waitTime = Time.time + timeBetweenBarrels;
            }
        }
        else
        {
            changeWT = false;
            updateTime = false;
            timeBetweenBarrels = originalTime;

        }
    }

    GameObject prevShip;
    private void ProduceBarrel()
    {
        if(Time.time > waitTime)
        {

            if (OilProduction.instance.currentOilValue > barrelCapacity)
            {
                if (prevShip != null && prevShip.transform.position.x < -0.5f)
                    return;
                    //    StartCoroutine(delayAnimExit());
                    GameObject go = Instantiate(Resources.Load("Ship") as GameObject, barrelSpawn.position, Quaternion.identity);
                prevShip = go;
                OilProduction.instance.currentOilValue -= barrelCapacity;
                UIManager.instance.UpdateOilValueText();
            }
            waitTime = Time.time + timeBetweenBarrels;
        }
    }

    public void UpdateRefine()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(barrelRateOfIncrease, barrelMultiplier, barrelCapacity, false);
        barrelCapacity = value[0];
        barrelRateOfIncrease = value[1];
        pricePerBarrel = value[0];
    }

    public float checkForRefine()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(barrelRateOfIncrease, barrelMultiplier, barrelCapacity, false);

        return value[0];
    }

    public float checkForRefineRateOFI()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(barrelRateOfIncrease, barrelMultiplier, barrelCapacity, false);
        return value[1];
    }
    public void UpdateShipment()
    {
       // MinBarrelPerMin += 1;
        timeBetweenBarrels = 60 / MinBarrelPerMin;
        ConveyorBelt.instance.UpdateSpeed();
    }

    private IEnumerator delayAnimExit()
    {
        yield return new WaitForSeconds(0.03f);
        AnimationHandler.instance.pumpAnim.SetBool("play", true);
        yield return new WaitForSeconds(0.27f);
        AnimationHandler.instance.pumpAnim.SetBool("play", false);
    }
}
