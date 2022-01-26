using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI oilValueText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI Upgrade_Shipment_Text, Upgrade_Rig_Text, Upgrade_Refine_Text;

    public TextMeshProUGUI Dialog_Price_Text, Dialog_UpgradeName_Text, Dialog_UpgradeROI_Text , Dialog_Value_Text, Dialog_Level_Text;
    public TextMeshProUGUI second_Dialog_UpgradeName_Text, second_Dialog_UpgradeROI_Text, second_Dialog_Value_Text;
    public GameObject dialog_Box;
    public GameObject secondBox;
    public Button btn;
    public GameObject NI;
    public GameObject countryPanel;


    public GameObject Character;
    public TextMeshProUGUI characterDialog;


    public TextMeshProUGUI unlockCountryName, unlockCountryprice_text;
    public GameObject unlockCountryUIBox;

    public int selected = 0;

    public List<GameObject> countryList;
    

    public GameObject m_OfflineBonusUI;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(AddMoney());

    }

    private void Update()
    {
        UpdateMoneyText();
    }

    [SerializeField]
    TextMeshProUGUI m_titleText;

    [SerializeField]
    Button m_collectBonusBtn;

    public float m_collectedBonus;


    public void CloseUI()
    {
        m_OfflineBonusUI.gameObject.SetActive(false);
    }

    public void OnCountrySelect()
    {
        countryPanel.SetActive(true);
        
    }


    public void Init(float offlineCollection)
    {
        m_collectedBonus = offlineCollection;
        m_collectBonusBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$" + CalcUtils.FormatNumber(offlineCollection);
    }

    public void OnClick_CollectBonus()
    {
        CloseUI();
    }

    private IEnumerator AddMoney()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.instance.moneyEarned += m_collectedBonus;
        GameManager.instance.GetOfflineOil();
    }

    public void UpdateOilValueText()
    {
        oilValueText.text = CalcUtils.FormatNumber(OilProduction.instance.currentOilValue).ToString();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$" + CalcUtils.FormatNumber(GameManager.instance.moneyEarned).ToString();
    }

    public void UpdateRigText(float value, float level)
    {
        Upgrade_Rig_Text.text = "Level " + level + "\n" + "$" + value.ToString();
    }

    public void UpdateRefineText(float value, float level)
    {
        Upgrade_Refine_Text.text = "Level " + level + "\n" + "$" + value.ToString();
    }

    public void UpdateShipmentText(float value, float level)
    {
        Upgrade_Shipment_Text.text = "Level " + level + "\n"+ "$" + value.ToString() ;
    }

    public void ShowOfflineBonusUI(float collectedMoney)
    {
        m_OfflineBonusUI.gameObject.SetActive(true);
        Init(collectedMoney);
    }

    public void unlockCountryDialogBox(string Name, int price)
    {
        unlockCountryUIBox.SetActive(true);
        unlockCountryName.text = Name;
        if(GameManager.instance.moneyEarned < price)
        {
            unlockCountryprice_text.gameObject.transform.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            unlockCountryprice_text.gameObject.transform.GetComponentInParent<Button>().interactable = true;
        }
        unlockCountryprice_text.text = "$" + price.ToString();
    }

    public void paidForCountry(int price)
    {
        GameManager.instance.moneyEarned -= price;
        GameManager.instance.paidforCountry = true;
        GameManager.instance.CountryUnlockedFully++;

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "USA")
        {
            PlayerPrefs.SetInt("CanadaUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Canada")
        {
            PlayerPrefs.SetInt("UKUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UK")
        {
            PlayerPrefs.SetInt("AustraliaUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Australia")
        {
            PlayerPrefs.SetInt("FranceUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "France")
        {
            PlayerPrefs.SetInt("GermanyUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Germany")
        {
            PlayerPrefs.SetInt("UAEUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UAE")
        {
            PlayerPrefs.SetInt("SaudiUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Saudi")
        {
            PlayerPrefs.SetInt("RussiaUnlocked", 1);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Russia")
        {
            PlayerPrefs.SetInt("BrazilUnlocked", 1);
        }



        GameManager.instance.RecheckCountry();
        UIManager.instance.unlockCountryUIBox.SetActive(false);
    }

    public void UpdateDialogBox(float price, string UpgradeName, float upgradeROI, float value, int level)
    {
        secondBox.SetActive(false);
        dialog_Box.SetActive(true);
        Dialog_Level_Text.text = "Level " + level;
        switch (UpgradeName)
        {
            case "Quantity":
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString();
                Dialog_Value_Text.text =  CalcUtils.FormatNumber(value).ToString();
                if (level >= 30)
                {
                    Dialog_Level_Text.text = "MAX LEVEL";
                    break;
                }

                if (level == (OilProduction.instance.rigList.Count * 10))
                {
                    Dialog_Price_Text.text = "BUILD NEW RIG FOR $" + CalcUtils.FormatNumber(price).ToString();
                    Dialog_Level_Text.text = "Max Limit";
                }

                break;
            case "Capacity":
                secondBox.SetActive(true);
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString();
                Dialog_Value_Text.text =  CalcUtils.FormatNumber(value).ToString();

                second_Dialog_UpgradeName_Text.text = "Profit";
                second_Dialog_UpgradeROI_Text.text = "+$" + CalcUtils.FormatNumber(upgradeROI).ToString();
                second_Dialog_Value_Text.text = "$" + CalcUtils.FormatNumber(value).ToString();

                if (level >= 30)
                {
                    Dialog_Level_Text.text = "MAX LEVEL";
                    break;
                }

                if (level == (Shipment.instance.refineList.Count * 10))
                {
                    Dialog_Price_Text.fontSize = 13;
                    Dialog_Price_Text.text = "BUILD NEW REFINERY TANK FOR $" + CalcUtils.FormatNumber(price).ToString();
                    Dialog_Level_Text.text = "Max Limit";
                }
                else
                {
                    Dialog_Price_Text.fontSize = 18;
                }
                break;
            case "Speed":
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString() + "%";
                Dialog_Value_Text.text = CalcUtils.FormatNumber(value).ToString();

                if (level >= 30)
                {
                    Dialog_Level_Text.text = "MAX LEVEL";
                }
                break;

            default:

                Dialog_Price_Text.text = "+$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString() + "%";
                Dialog_Value_Text.text = "+" + CalcUtils.FormatNumber(value).ToString();
                break;
        }
    }

    public IEnumerator stageRefineIncreaseText(string texT, GameObject obj)
    {
        obj.GetComponent<Text>().text = texT;
      //  obj.transform.SetParent(this.transform);
        float waitTime = Time.time + 0.5f;
        while(Time.time < waitTime)
        {
            obj.transform.Translate(Vector2.up * 2f * Time.deltaTime);
            yield return 0;
        }

        GameObject.Destroy(obj);
    }

    public void UpdateDialogBoxtoMAx(float price, string UpgradeName, float upgradeROI, float value, int level)
    {
        secondBox.SetActive(false);
        dialog_Box.SetActive(true);
        Dialog_Level_Text.text = "Level " + level;
        switch (UpgradeName)
        {
            case "Quantity":
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_Level_Text.text = "MAXED OUT";
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString();
                Dialog_Value_Text.text = CalcUtils.FormatNumber(value).ToString();
                break;


            case "Capacity":
                secondBox.SetActive(true);
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString();
                Dialog_Value_Text.text = CalcUtils.FormatNumber(value).ToString();

                second_Dialog_UpgradeName_Text.text = "Profit";
                second_Dialog_UpgradeROI_Text.text = "+$" + CalcUtils.FormatNumber(upgradeROI).ToString();
                second_Dialog_Value_Text.text = "$" + CalcUtils.FormatNumber(value).ToString();
                Dialog_Level_Text.text = "MAX LEVEL";
                break;
            case "Speed":
                Dialog_Price_Text.text = "$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString() + "%";
                Dialog_Value_Text.text = CalcUtils.FormatNumber(value).ToString();

                if (level >= 30)
                {
                    Dialog_Level_Text.text = "MAX OUT";
                }
                break;

            default:

                Dialog_Price_Text.text = "+$" + CalcUtils.FormatNumber(price).ToString();
                Dialog_UpgradeName_Text.text = UpgradeName;
                Dialog_UpgradeROI_Text.text = "+" + CalcUtils.FormatNumber(upgradeROI).ToString() + "%";
                Dialog_Value_Text.text = "+" + CalcUtils.FormatNumber(value).ToString();
                break;
        }
    }

}
