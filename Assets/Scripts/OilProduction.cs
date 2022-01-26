using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilProduction : MonoBehaviour
{
    private float pumpedTime = 0;
    private float maxPumpTime = 4f;

    public float OilPumpPerSecond_LevelMax = 1f;

    public float oilMax
    {
        get
        {
            return OilPumpPerSecond_LevelMax;
        }
        set
        {
            OilPumpPerSecond_LevelMax = value;
        }
    }
    private float OilPumpPerSecond;
    public float OilPumpedRateOfIncrease = 1f;

    public float oil_ROI
    {
        get
        {
            return OilPumpedRateOfIncrease;
        }
        set
        {
            OilPumpedRateOfIncrease = value;
        }
    }
    public float OilPumpedMultiplier = 1.15f;

    public float currentOilValue;

    public float waitForSeconds = 1;
    private float waitTime;

    public static OilProduction instance;

    public List<GameObject> rigList;
    public GameObject rigObject;

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
        int count = GameManager.instance.getRigListCount();
        if (count > 1)
        {

            for (int i = 1; i < count; i++)
            {
                GameObject go = Instantiate(Resources.Load("Rig_right") as GameObject);
                go.transform.SetParent(OilProduction.instance.rigObject.transform);
                go.GetComponent<RectTransform>().localScale = OilProduction.instance.rigList[0].GetComponent<RectTransform>().localScale;
                go.GetComponent<RectTransform>().position = new Vector2(OilProduction.instance.rigList[i - 1].transform.position.x + 2, OilProduction.instance.rigList[i - 1].transform.position.y + 1);
                go.GetComponent<Button>().onClick = OilProduction.instance.rigList[i - 1].GetComponent<Button>().onClick;
                go.GetComponent<Image>().color = rigList[0].GetComponent<Image>().color;

                //      OilProduction.instance.rigList[i - 1].GetComponent<Button>().onClick = null;
                OilProduction.instance.rigList.Add(go);
                UIManager.instance.dialog_Box.SetActive(false);
            }
        }
        waitTime = Time.time + waitForSeconds;
    }

    private void Update()
    {
        if (pumpedTime <= 0)
        {
            pumpedTime = 0;
            OilPumpPerSecond = Random.Range(0, OilPumpPerSecond_LevelMax / 8);
            AnimationHandler.instance.speedDownAnimations();
            //   Debug.Log("Below zero");
        }
        else
        {
            OilPumpPerSecond = OilPumpPerSecond_LevelMax;
            pumpedTime -= Time.deltaTime;
            AnimationHandler.instance.speedUpAnimations();
            //   Debug.Log(pumpedTime);
        }

        if (Time.time > waitTime)
        {
            waitTime = Time.time + waitForSeconds;
            if (currentOilValue >= 0)
            {
                currentOilValue += OilPumpPerSecond;
                for(int i =0; i < rigList.Count; i++)
                {
                    GameObject go = Instantiate(Resources.Load("StageMoneyText") as GameObject, rigObject.gameObject.transform);
                    go.transform.position = new Vector3(rigList[i].gameObject.transform.position.x - 1f, rigList[i].gameObject.transform.position.y + 1f,0);
                    StartCoroutine(UIManager.instance.stageRefineIncreaseText("+" + CalcUtils.FormatNumber(OilPumpPerSecond / (rigList.Count - i)), go));
            //        Debug.Log("worked");
                }

                UIManager.instance.UpdateOilValueText();
            }
            else
            {
                currentOilValue = 0;
            }
        }
    }

    public float UpdateRig()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(OilPumpedRateOfIncrease, OilPumpedMultiplier, OilPumpPerSecond_LevelMax, true);
        OilPumpPerSecond_LevelMax = value[0];
        OilPumpedRateOfIncrease = value[1];
        return value[0];
    }

    public float checkForValue()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(OilPumpedRateOfIncrease, OilPumpedMultiplier, OilPumpPerSecond_LevelMax, true);
        return value[0];
    }

    public float checkForOilROI()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(OilPumpedRateOfIncrease, OilPumpedMultiplier, OilPumpPerSecond_LevelMax, true);
        return value[1];
    }
    public void OnPumpPressed()
    {
        /*  if (pumpedTime < maxPumpTime)
          {
              pumpedTime += 0.5f;
          }*/

        currentOilValue += OilPumpPerSecond_LevelMax;
    //    GameObject go = Instantiate(Resources.Load("StageMoneyText") as GameObject, UIManager.instance.gameObject.transform);
   //     StartCoroutine(UIManager.instance.stageRefineIncreaseText("+" + CalcUtils.FormatNumber(OilPumpPerSecond_LevelMax).ToString(), go));
        for (int i = 0; i < rigList.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load("StageMoneyText") as GameObject, rigObject.gameObject.transform);
            go.transform.position = new Vector3(rigList[i].gameObject.transform.position.x - 1f, rigList[i].gameObject.transform.position.y + 1f, 0);
            StartCoroutine(UIManager.instance.stageRefineIncreaseText("+" + CalcUtils.FormatNumber(OilPumpPerSecond_LevelMax / (rigList.Count - i)), go));
        }
    }
}
