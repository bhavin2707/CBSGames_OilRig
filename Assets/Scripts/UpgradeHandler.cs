using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    public float baseUpgradePrice_Rig = 40;
    public float upgradeRateOfIncrease_Rig = 30;
    public float level_rig = 2;
    public float baseUpgradePrice_Refining = 20;
    public float upgradeRateOfIncrease_Refine = 30;
    public float level_Refine = 2;
    public float baseUpgradePrice_Shipping = 60;
    public float upgradeRateOfIncrease_Shipping = 30;
    public float level_ship = 2;

    public float upgradeMultiplier = 1.2f;

    public static UpgradeHandler instance;

    public bool allUnlocked = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
      
    }

    public void showMaxedRIg()
    {
        UIManager.instance.UpdateDialogBoxtoMAx(baseUpgradePrice_Rig, "Quantity", OilProduction.instance.checkForOilROI(), OilProduction.instance.checkForValue(), (int)level_rig);
        UIManager.instance.btn.interactable = false;

        UIManager.instance.selected = 1;
    }


    public void showMaxeRefine()
    {
        UIManager.instance.UpdateDialogBox(baseUpgradePrice_Refining, "Capacity", Shipment.instance.checkForRefineRateOFI(), Shipment.instance.checkForRefine(), (int)level_Refine);
        UIManager.instance.btn.interactable = false;

        UIManager.instance.selected = 1;
    }

    public void showDialogRIg()
    {
      //  UIManager.instance.btn.interactable = true;

        if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject != OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1])
        {
            showMaxedRIg();
            return;
        }
        UIManager.instance.UpdateDialogBox(baseUpgradePrice_Rig, "Quantity", OilProduction.instance.checkForOilROI(), OilProduction.instance.checkForValue(), (int)level_rig);
        if (GameManager.instance.moneyEarned < baseUpgradePrice_Rig)
        {
            UIManager.instance.btn.interactable = false;
        }
        else
        {
            UIManager.instance.btn.interactable = true;
        }
        if (!allUnlocked)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == GameManager.instance.countryUnlockedIndex)
                if (level_Refine >= 30 && level_rig >= 30 && level_ship >= 30)
                {
                    allUnlocked = true;
                    UIManager.instance.Character.SetActive(true);
                    UIManager.instance.characterDialog.text = "You have just unlocked a new Country!";
                    GameManager.instance.countryUnlockedIndex++;
                    GameManager.instance.RecheckCountry();
                    GameManager.instance.saveEverything();
                }
        }
        UIManager.instance.selected = 1;
    }

   

    public void showDialogRefine()
    {
      //  UIManager.instance.btn.interactable = true;
        UIManager.instance.UpdateDialogBox(baseUpgradePrice_Refining, "Capacity", Shipment.instance.checkForRefineRateOFI(), Shipment.instance.checkForRefine(), (int)level_Refine);
        if (GameManager.instance.moneyEarned < baseUpgradePrice_Refining)
        {
            UIManager.instance.btn.interactable = false;
        }
        else
        {
            UIManager.instance.btn.interactable = true;
        }
        if (!allUnlocked)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == GameManager.instance.countryUnlockedIndex)
                if (level_Refine >= 30 && level_rig >= 30 && level_ship >= 30)
                {
                    allUnlocked = true;
                    UIManager.instance.Character.SetActive(true);
                    UIManager.instance.characterDialog.text = "You have just unlocked a new Country!";
                    GameManager.instance.countryUnlockedIndex++;
                    GameManager.instance.RecheckCountry();
                    GameManager.instance.saveEverything();
                }
        }
        UIManager.instance.selected = 2;
    }

    public void showDialogShipping()
    {
        UIManager.instance.UpdateDialogBox(baseUpgradePrice_Shipping, "Speed", 1f, Shipment.instance.MinBarrelPerMin, (int)level_ship);
        if (GameManager.instance.moneyEarned < baseUpgradePrice_Shipping)
        {
            UIManager.instance.btn.interactable = false;
        }
        else
        {
            UIManager.instance.btn.interactable = true;
        }
        UIManager.instance.selected = 3;
    }


    public void UpgradeMachine()
    {
        float[] value = new float[2];
        switch (UIManager.instance.selected)
        {
            case 1:

                if (level_rig >= 30)
                {
                    UIManager.instance.btn.interactable = false;
                    UIManager.instance.UpdateDialogBox(baseUpgradePrice_Rig, "Quantity", 1f, OilProduction.instance.oilMax, 30);
                    break;
                }
                //This shit is for creating new rigs
                if (level_rig == (OilProduction.instance.rigList.Count * 10))
                {
                    GameObject go = Instantiate(Resources.Load("Rig_right") as GameObject);
                    go.transform.SetParent(OilProduction.instance.rigObject.transform);
                    go.GetComponent<RectTransform>().localScale = OilProduction.instance.rigList[0].GetComponent<RectTransform>().localScale;
                    go.GetComponent<RectTransform>().position = new Vector2(OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1].transform.position.x + 2, OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1].transform.position.y + 1);
                    go.GetComponent<Button>().onClick = OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1].GetComponent<Button>().onClick;
                    go.GetComponent<Image>().color = OilProduction.instance.rigList[0].GetComponent<Image>().color;
                 //   OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1].GetComponent<Button>().onClick = null;
                  //  OilProduction.instance.rigList[OilProduction.instance.rigList.Count - 1].GetComponent<Button>().onClick.AddListener(showMaxedRIg);
                    OilProduction.instance.rigList.Add(go);
                    UIManager.instance.dialog_Box.SetActive(false);
                }

                    GameManager.instance.moneyEarned -= baseUpgradePrice_Rig;
                    OilProduction.instance.UpdateRig();
                    value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Rig, 1.2f, baseUpgradePrice_Rig, true);
                    baseUpgradePrice_Rig = value[0];
                    upgradeRateOfIncrease_Rig = value[1];
                    level_rig += 1;
                    UIManager.instance.UpdateRigText(baseUpgradePrice_Rig, level_rig);

                if (UIManager.instance.dialog_Box.activeInHierarchy == true)
                {
                    UIManager.instance.UpdateDialogBox(baseUpgradePrice_Rig, "Quantity", OilProduction.instance.checkForOilROI(), OilProduction.instance.checkForValue(), (int)level_rig);
                }
               
                    if (GameManager.instance.moneyEarned < baseUpgradePrice_Rig)
                    {
                        UIManager.instance.btn.interactable = false;
                    }
                    else
                    {
                        UIManager.instance.btn.interactable = true;
                    }
                
                break;
            case 2:

                if (level_Refine >= 30)
                {
                    UIManager.instance.btn.interactable = false;
                    UIManager.instance.UpdateDialogBox(baseUpgradePrice_Refining, "Capacity", 1f, Shipment.instance.BarrelCapacity, 30);
                    break;
                }
                //This shit is for creating new rigs
                if (level_Refine == (Shipment.instance.refineList.Count * 10))
                {
                    GameObject go = Instantiate(Resources.Load("Refinery_Up") as GameObject);
                    go.transform.SetParent(Shipment.instance.refineObject.transform);
                    go.GetComponent<RectTransform>().localScale = Shipment.instance.refineList[0].GetComponent<RectTransform>().localScale;
                    if (Shipment.instance.refineList.Count == 1)
                    go.GetComponent<RectTransform>().position = new Vector2(Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].transform.position.x - 2.75f, Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].transform.position.y + 1.59f);
                    else
                        go.GetComponent<RectTransform>().position = new Vector2(Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].transform.position.x - 1.59f, Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].transform.position.y + 0.95f);
                    go.GetComponent<Button>().onClick = null;
                   // Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].GetComponent<Button>().onClick.AddListener(delegate { showMaxedRIg(); });
                  //  Shipment.instance.refineList[Shipment.instance.refineList.Count - 1].GetComponent<Button>().onClick = null;
                    Shipment.instance.refineList.Add(go);
                    UIManager.instance.dialog_Box.SetActive(false);
                }

                GameManager.instance.moneyEarned -= baseUpgradePrice_Refining;
                value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Refine, 1.2f, baseUpgradePrice_Refining, true);
                baseUpgradePrice_Refining = value[0];
                upgradeRateOfIncrease_Refine = value[1];
                level_Refine += 1;
                Shipment.instance.UpdateRefine();
                UIManager.instance.UpdateRefineText(baseUpgradePrice_Refining, level_Refine);
                if (UIManager.instance.dialog_Box.activeInHierarchy == true)
                {
                    UIManager.instance.UpdateDialogBox(baseUpgradePrice_Refining, "Capacity", Shipment.instance.checkForRefineRateOFI(), Shipment.instance.checkForRefine(), (int)level_Refine);
                }
              
                if (GameManager.instance.moneyEarned < baseUpgradePrice_Refining)
                {
                    UIManager.instance.btn.interactable = false;
                }
                else
                {
                    UIManager.instance.btn.interactable = true;
                }
                break;
            case 3:
                if (level_ship >= 30)
                {
                    UIManager.instance.btn.interactable = false;
                    UIManager.instance.UpdateDialogBox(baseUpgradePrice_Shipping, "Speed",1f, Shipment.instance.MinBarrelPerMin, 100);
                    break;
                }
                GameManager.instance.moneyEarned -= baseUpgradePrice_Shipping;
                value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Shipping, 1.2f, baseUpgradePrice_Shipping, true);
                baseUpgradePrice_Shipping = value[0];
                upgradeRateOfIncrease_Shipping = value[1];
                level_ship += 1;
                Shipment.instance.UpdateShipment();
                UIManager.instance.UpdateShipmentText(baseUpgradePrice_Shipping, level_ship);
                UIManager.instance.UpdateDialogBox(baseUpgradePrice_Shipping, "Speed", 1f, Shipment.instance.MinBarrelPerMin, (int)level_ship);
                if (GameManager.instance.moneyEarned < baseUpgradePrice_Shipping)
                {
                    UIManager.instance.btn.interactable = false;
                }
                else
                {
                    UIManager.instance.btn.interactable = true;
                }
                UIManager.instance.selected = 3;
                break;
            default:
                break;
        }
        UIManager.instance.UpdateMoneyText();
        if (!allUnlocked)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == GameManager.instance.countryUnlockedIndex)
                if (level_Refine >= 30 && level_rig >= 30 && level_ship >= 30)
                {
                    allUnlocked = true;
                    UIManager.instance.Character.SetActive(true);
                    UIManager.instance.characterDialog.text = "You have just unlocked a new Country!";
                    GameManager.instance.countryUnlockedIndex++;
                    GameManager.instance.RecheckCountry();
                    GameManager.instance.saveEverything();
                }
        }
    }
    public void OnRigUpgrade()
    {
        float[] value = new float[2];
        level_rig += 1;
        OilProduction.instance.UpdateRig();
        value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Rig, 1.2f, baseUpgradePrice_Rig, true);
        baseUpgradePrice_Rig = value[0];
        upgradeRateOfIncrease_Rig = value[1];
        UIManager.instance.UpdateRigText(baseUpgradePrice_Rig, level_rig);
    }

    public void OnRefineUpgrade()   
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Refine, 1.2f, baseUpgradePrice_Refining,true);
        baseUpgradePrice_Refining = value[0];
        upgradeRateOfIncrease_Refine = value[1];
        level_Refine += 1;
        Shipment.instance.UpdateRefine();
        UIManager.instance.UpdateRefineText(baseUpgradePrice_Refining, level_Refine);
    }

    public void OnShippingUpgrade()
    {
        float[] value = new float[2];
        value = GameManager.instance.LevelUpObject(upgradeRateOfIncrease_Shipping, 1.2f, baseUpgradePrice_Shipping,true);
        baseUpgradePrice_Shipping = value[0];
        upgradeRateOfIncrease_Shipping = value[1];
        level_ship += 1;
        Shipment.instance.UpdateShipment();
        UIManager.instance.UpdateShipmentText(baseUpgradePrice_Shipping, level_ship);
    }

}
