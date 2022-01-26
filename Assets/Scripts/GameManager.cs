using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float moneyEarned = 0;
    public int countryUnlockedIndex = 0;
    public bool paidforCountry = false;
    public int CountryUnlockedFully;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        saveEverything();
    }
    private void Start()
    {
        getEverything();
      //  PlayerPrefs.DeleteAll();

        if(UpgradeHandler.instance.level_rig != 1 && UpgradeHandler.instance.level_Refine != 1 && UpgradeHandler.instance.level_ship != 1)
        {
            string pauseTimeString = SharedPrefUtils.GetPauseTime();
            if (pauseTimeString != null)
            {
                lastSavedTimeAndDate = DateTime.Parse(pauseTimeString);
                currentTimeAndDate = DateTime.UtcNow;
                timeDifference = currentTimeAndDate - lastSavedTimeAndDate;


                if ((int)timeDifference.TotalMinutes >= 1 && PlayerPrefs.GetInt("firstTime", 1) != 1)
                {
                    UIManager.instance.ShowOfflineBonusUI(GetOfflineCollection(timeDifference.TotalMinutes));
                }
            }
        }

        UIManager.instance.UpdateOilValueText();
        UIManager.instance.UpdateMoneyText();

        RecheckCountry();

        if(SceneManager.GetActiveScene().buildIndex < CountryUnlockedFully)
        {
            paidforCountry = true;
            RecheckCountry();
        }
    }

    public void RecheckCountry()
    {
        UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < UIManager.instance.countryList.Count; i++)
        {
            for (int j = 0; j <= CountryUnlockedFully; j++)
            {
                UIManager.instance.countryList[j].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }


            if (i <= countryUnlockedIndex)
            {
                UIManager.instance.countryList[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
                UIManager.instance.countryList[i].transform.GetChild(0).gameObject.SetActive(true);
                UIManager.instance.countryList[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                UIManager.instance.countryList[i].GetComponent<UnityEngine.UI.Button>().interactable = false;
                UIManager.instance.countryList[i].transform.GetChild(0).gameObject.SetActive(false);
                UIManager.instance.countryList[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private float GetOfflineCollection(double TotalMinutes)
    {
        float value = 2f;
        value = (float)((Shipment.instance.MinBarrelPerMin * (int)TotalMinutes) * Shipment.instance.PricePerBarrel);
        if (OilProduction.instance.currentOilValue < Shipment.instance.barrelCapacity)
        {
            value /= 7;
        }
        Debug.Log("Offline Calc Bonus: " + value);
        return value;
    }

    public void GetOfflineOil()
    {
        float totalOil = (float)((((int)timeDifference.TotalMinutes) * 60) * UnityEngine.Random.Range(2, OilProduction.instance.oilMax / 8)); 
        float totalReduction = (float)((Shipment.instance.MinBarrelPerMin * (int)timeDifference.TotalMinutes) * Shipment.instance.BarrelCapacity);
        OilProduction.instance.currentOilValue += (totalOil - totalReduction);
    }

    public TimeSpan timeDifference;//Amount of time since the game was last open
    public DateTime currentTimeAndDate;
    public DateTime lastSavedTimeAndDate;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
    //        Debug.Log("App Pause");
            currentTimeAndDate = DateTime.UtcNow;
            SharedPrefUtils.SetPauseTime(DateTime.UtcNow.ToString());
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
      //      Debug.Log("App Focus Lost");
            saveEverything();
            currentTimeAndDate = DateTime.UtcNow;
            SharedPrefUtils.SetPauseTime(DateTime.UtcNow.ToString());

        }
    }


    public float[] LevelUpObject(float rateOfIncrease,float multiplier,float value, bool roundUp)
    {
        if (roundUp)
        {
            float roundedValue = Mathf.Round(RoundToTen(rateOfIncrease));
            value = value + roundedValue;
            rateOfIncrease = rateOfIncrease * multiplier;
       //     Debug.Log("Rate Of Increase = " + rateOfIncrease + " : Total Value = " + value);
            float[] num = new float[2];
            num[0] = value;
            num[1] = rateOfIncrease;
            return num;
        }
        else
        {
            value = value + Mathf.Round(rateOfIncrease);
            rateOfIncrease = rateOfIncrease * multiplier;
  //          Debug.Log("Rate Of Increase = " + rateOfIncrease + " : Total Value = " + value);
            float[] num = new float[2];
            num[0] = value;
            num[1] = rateOfIncrease;
            return num;
        }
    }

    public float RoundToTen(float number)
    {
        if (number % 10 == 5)
            number++;
        //     float smaller = (float)System.Math.Ceiling(number / 10) * 10;
        float smaller = (float)(number / 10) * 10;
        float larger = smaller + 10;
        if(number - smaller > larger - number)
        {
      //      Debug.Log("Larger = " + larger);
        }
        else
        {
       //     Debug.Log("Smaller = " + smaller);
        }
        return (number - smaller > larger - number) ? larger : smaller;
    }


    public void CountryChange(string name)
    {
  
        Debug.Log(GameManager.instance.CountryUnlockedFully);
        if (name == "USA")
        {
            SceneManager.LoadScene("USA");
        }
        else if (name == "Canada")
        {
            if (PlayerPrefs.GetInt("CanadaUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("Canada",10000);
            }
            else
            SceneManager.LoadScene("Canada");
        }
        else if (name == "UK")
        {
            if (PlayerPrefs.GetInt("UKUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("UK", 30000);
            }
            else
            SceneManager.LoadScene("UK");
        }
        else if (name == "Australia")
        {
            if (PlayerPrefs.GetInt("AustraliaUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("Australia", 90000);
            }
            else
            SceneManager.LoadScene("Australia");
        }
        else if (name == "France")
        {
            if (PlayerPrefs.GetInt("FranceUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("France", 270000);
            }
            else
            SceneManager.LoadScene("France");
        }
        else if (name == "Germany")
        {
            if (PlayerPrefs.GetInt("GermanyUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("UAE", 810000);
            }
            else
            SceneManager.LoadScene("Germany");
        }
        else if (name == "UAE")
        {
            if (PlayerPrefs.GetInt("UAEUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("Saudi", 2430000);
            }
            else
            SceneManager.LoadScene("UAE");
        }
        else if (name == "Saudi")
        {
            if (PlayerPrefs.GetInt("SaudiUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("Russia", 7290000);
            }
            else
            SceneManager.LoadScene("Saudi");
        }
        else if (name == "Russia")
        {
            if (PlayerPrefs.GetInt("RussiaUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UIManager.instance.unlockCountryDialogBox("Brazil", 21870000);
            }
            else
            SceneManager.LoadScene("Russia");
        }
        else if (name == "Brazil")
        {
            if (PlayerPrefs.GetInt("BrazilUnlocked", 0) != 1)
            {
                UIManager.instance.countryList[SceneManager.GetActiveScene().buildIndex + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            else
            SceneManager.LoadScene("Brazil");
        }
    }



    //Data Manipulation

    public void saveEverything()
    {
        PlayerPrefs.SetFloat("money", moneyEarned);
        PlayerPrefs.SetInt("CIndex", countryUnlockedIndex);
        PlayerPrefs.SetInt("CIFull", CountryUnlockedFully);
        if (SceneManager.GetActiveScene().name == "USA")
        {
            saveData("_usa");
        }
        else if (SceneManager.GetActiveScene().name == "Canada")
        {
            saveData("_canada");
        }
        else if (SceneManager.GetActiveScene().name == "UK")
        {
            saveData("_uk");
        }
        else if (SceneManager.GetActiveScene().name == "Australia")
        {
            saveData("_australia");
        }
        else if (SceneManager.GetActiveScene().name == "France")
        {
            saveData("_france");
        }
        else if (SceneManager.GetActiveScene().name == "Germany")
        {
            saveData("_germany");
        }
        else if (SceneManager.GetActiveScene().name == "UAE")
        {
            saveData("_uae");
        }
        else if (SceneManager.GetActiveScene().name == "Saudi")
        {
            saveData("_saudi");
        }
        else if (SceneManager.GetActiveScene().name == "Russia")
        {
            saveData("_russia");
        }
        else if (SceneManager.GetActiveScene().name == "Brazil")
        {
            saveData("_brazil");
        }

    }

    private void saveData(string name)
    {
        //OIL
        PlayerPrefs.SetFloat("oilMax" + name, OilProduction.instance.oilMax);
        PlayerPrefs.SetFloat("oilROI" + name, OilProduction.instance.oil_ROI);
        PlayerPrefs.SetFloat("currentOil" + name, OilProduction.instance.currentOilValue);


        //Barrel
        PlayerPrefs.SetFloat("barrelCapacity" + name, Shipment.instance.BarrelCapacity);
        PlayerPrefs.SetFloat("barrelROI" + name, Shipment.instance.barrelROI);
        PlayerPrefs.SetFloat("minimumBarrel" + name, Shipment.instance.MinBarrelPerMin);

        //Upgrade

        PlayerPrefs.SetFloat("baseUpgradePrice_Rig" + name, UpgradeHandler.instance.baseUpgradePrice_Rig);
        PlayerPrefs.SetFloat("upgradeRateOfIncrease_Rig" + name, UpgradeHandler.instance.upgradeRateOfIncrease_Rig);
        PlayerPrefs.SetFloat("level_rig" + name, UpgradeHandler.instance.level_rig);

        PlayerPrefs.SetFloat("baseUpgradePrice_Refining" + name, UpgradeHandler.instance.baseUpgradePrice_Refining);
        PlayerPrefs.SetFloat("upgradeRateOfIncrease_Refine" + name, UpgradeHandler.instance.upgradeRateOfIncrease_Refine);
        PlayerPrefs.SetFloat("level_Refine" + name, UpgradeHandler.instance.level_Refine);

        PlayerPrefs.SetFloat("baseUpgradePrice_Shipping" + name, UpgradeHandler.instance.baseUpgradePrice_Shipping);
        PlayerPrefs.SetFloat("upgradeRateOfIncrease_Shipping" + name, UpgradeHandler.instance.upgradeRateOfIncrease_Shipping);
        PlayerPrefs.SetFloat("level_ship" + name, UpgradeHandler.instance.level_ship);

        PlayerPrefs.SetFloat("currentSpeed" + name, ConveyorBelt.instance.currentSpeed);

        PlayerPrefs.SetInt("rigList" + name, OilProduction.instance.rigList.Count);
        PlayerPrefs.SetInt("refineList" + name, Shipment.instance.refineList.Count);
    }


    public void getEverything()
    {
       moneyEarned = PlayerPrefs.GetFloat("money",0);

        countryUnlockedIndex = PlayerPrefs.GetInt("CIndex", 0);
        CountryUnlockedFully = PlayerPrefs.GetInt("CIFull", 0);
        if (SceneManager.GetActiveScene().name == "USA")
        {
            getData("_usa");
        }
        else if (SceneManager.GetActiveScene().name == "Canada")
        {
            getData("_canada");
        }
        else if (SceneManager.GetActiveScene().name == "UK")
        {
            getData("_uk");
        }
        else if (SceneManager.GetActiveScene().name == "Australia")
        {
            getData("_australia");
        }
        else if (SceneManager.GetActiveScene().name == "France")
        {
            getData("_france");
        }
        else if (SceneManager.GetActiveScene().name == "Germany")
        {
            getData("_germany");
        }
        else if (SceneManager.GetActiveScene().name == "UAE")
        {
            getData("_uae");
        }
        else if (SceneManager.GetActiveScene().name == "Saudi")
        {
            getData("_saudi");
        }
        else if (SceneManager.GetActiveScene().name == "Russia")
        {
            getData("_russia");
        }
        else if (SceneManager.GetActiveScene().name == "Brazil")
        {
            getData("_brazil");
        }
/*

        //OIL
        OilProduction.instance.oilMax = PlayerPrefs.GetFloat("oilMax", 10);
        OilProduction.instance.oil_ROI = PlayerPrefs.GetFloat("oilROI", 10);
        OilProduction.instance.currentOilValue = PlayerPrefs.GetFloat("currentOil",0);

        //Barrel
        Shipment.instance.PricePerBarrel = Shipment.instance.BarrelCapacity = PlayerPrefs.GetFloat("barrelCapacity",4);
        Shipment.instance.barrelROI = PlayerPrefs.GetFloat("barrelROI",3);
        Shipment.instance.MinBarrelPerMin = PlayerPrefs.GetFloat("minimumBarrel",25);

        //Upgrade
        UpgradeHandler.instance.baseUpgradePrice_Rig = PlayerPrefs.GetFloat("baseUpgradePrice_Rig", 40);
        UpgradeHandler.instance.upgradeRateOfIncrease_Rig = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Rig", 30);
        UpgradeHandler.instance.level_rig = PlayerPrefs.GetFloat("level_rig", 2);

        UpgradeHandler.instance.baseUpgradePrice_Refining = PlayerPrefs.GetFloat("baseUpgradePrice_Refining", 20);
        UpgradeHandler.instance.upgradeRateOfIncrease_Refine = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Refine", 30);
        UpgradeHandler.instance.level_Refine = PlayerPrefs.GetFloat("level_Refine", 2);

        UpgradeHandler.instance.baseUpgradePrice_Shipping = PlayerPrefs.GetFloat("baseUpgradePrice_Shipping", 60);
        UpgradeHandler.instance.upgradeRateOfIncrease_Shipping = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Shipping", 30);
        UpgradeHandler.instance.level_ship = PlayerPrefs.GetFloat("level_ship", 2);

        ConveyorBelt.instance.currentSpeed = PlayerPrefs.GetFloat("currentSpeed", 0.5f);

    //    OilProduction.instance.rigList.Count = PlayerPrefs.GetInt("rigList", 1);
    //    Shipment.instance.refineList.Count = PlayerPrefs.GetInt("refineList", 1);*/
    }

    private void getData(string name)
    {
        //OIL
        if (PlayerPrefs.GetFloat("oilMax" + name) != 0)
        OilProduction.instance.oilMax = PlayerPrefs.GetFloat("oilMax" + name);
        if (PlayerPrefs.GetFloat("oilROI" + name) != 0)
            OilProduction.instance.oil_ROI = PlayerPrefs.GetFloat("oilROI" + name);

        if (PlayerPrefs.GetFloat("currentOil" + name) != 0)
            OilProduction.instance.currentOilValue = PlayerPrefs.GetFloat("currentOil" + name);

        //Barrel
        if (PlayerPrefs.GetFloat("barrelCapacity" + name) != 0)
        {
            Shipment.instance.PricePerBarrel = Shipment.instance.BarrelCapacity = PlayerPrefs.GetFloat("barrelCapacity" + name);
            Shipment.instance.barrelROI = PlayerPrefs.GetFloat("barrelROI" + name);
            Shipment.instance.MinBarrelPerMin = PlayerPrefs.GetFloat("minimumBarrel" + name);
        }

        //Upgrade
        if (PlayerPrefs.GetFloat("baseUpgradePrice_Rig" + name) != 0)
        {
            UpgradeHandler.instance.baseUpgradePrice_Rig = PlayerPrefs.GetFloat("baseUpgradePrice_Rig" + name);
            UpgradeHandler.instance.upgradeRateOfIncrease_Rig = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Rig" + name);
        }
        UpgradeHandler.instance.level_rig = PlayerPrefs.GetFloat("level_rig" + name,1);

        if (PlayerPrefs.GetFloat("baseUpgradePrice_Refining" + name) != 0)
        {
            UpgradeHandler.instance.baseUpgradePrice_Refining = PlayerPrefs.GetFloat("baseUpgradePrice_Refining" + name);
            UpgradeHandler.instance.upgradeRateOfIncrease_Refine = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Refine" + name);
        }
        UpgradeHandler.instance.level_Refine = PlayerPrefs.GetFloat("level_Refine" + name,1);


        if (PlayerPrefs.GetFloat("baseUpgradePrice_Shipping" + name) != 0)
        {
            UpgradeHandler.instance.baseUpgradePrice_Shipping = PlayerPrefs.GetFloat("baseUpgradePrice_Shipping" + name);
            UpgradeHandler.instance.upgradeRateOfIncrease_Shipping = PlayerPrefs.GetFloat("upgradeRateOfIncrease_Shipping" + name);
        }
        UpgradeHandler.instance.level_ship = PlayerPrefs.GetFloat("level_ship" + name, 1);

        ConveyorBelt.instance.currentSpeed = PlayerPrefs.GetFloat("currentSpeed" + name, 0.5f);
    }

    public int getRigListCount()
    {
        if (SceneManager.GetActiveScene().name == "USA")
        {
            return PlayerPrefs.GetInt("rigList_usa", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Canada")
        {
            return PlayerPrefs.GetInt("rigList_canada", 1);
        }
        else if (SceneManager.GetActiveScene().name == "UK")
        {
            return PlayerPrefs.GetInt("rigList_uk", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Australia")
        {
            return PlayerPrefs.GetInt("rigList_australia", 1);
        }
        else if (SceneManager.GetActiveScene().name == "France")
        {
            return PlayerPrefs.GetInt("rigList_france", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Germany")
        {
            return PlayerPrefs.GetInt("rigList_germany", 1);
        }
        else if (SceneManager.GetActiveScene().name == "UAE")
        {
            return PlayerPrefs.GetInt("rigList_uae", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Saudi")
        {
            return PlayerPrefs.GetInt("rigList_saudi", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Russia")
        {
            return PlayerPrefs.GetInt("rigList_russia", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Brazil")
        {
            return PlayerPrefs.GetInt("rigList_brazil", 1);
        }

        return 0;
    }

    public int getRefineListCount()
    {
        if (SceneManager.GetActiveScene().name == "USA")
        {
            return PlayerPrefs.GetInt("refineList_usa", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Canada")
        {
            return PlayerPrefs.GetInt("refineList_canada", 1);
        }
        else if (SceneManager.GetActiveScene().name == "UK")
        {
            return PlayerPrefs.GetInt("refineList_uk", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Australia")
        {
            return PlayerPrefs.GetInt("refineList_australia", 1);
        }
        else if (SceneManager.GetActiveScene().name == "France")
        {
            return PlayerPrefs.GetInt("refineList_france", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Germany")
        {
            return PlayerPrefs.GetInt("refineList_germany", 1);
        }
        else if (SceneManager.GetActiveScene().name == "UAE")
        {
            return PlayerPrefs.GetInt("refineList_uae", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Saudi")
        {
            return PlayerPrefs.GetInt("refineList_saudi", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Russia")
        {
            return PlayerPrefs.GetInt("refineList_russia", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Brazil")
        {
            return PlayerPrefs.GetInt("refineList_brazil", 1);
        }

        return 0;
    }

}
