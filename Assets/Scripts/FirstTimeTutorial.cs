using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirstTimeTutorial : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public GameObject tutorial;
    public string[] text = {"We did it! We finally got a license for managing oil industry.",
        "But Managing Oil industry is harder than it seems. First, we have to pump oil from, Refine it and ship it.",
    "Everyone says you're the perfect person for the job.", "We have to ship lots of oil in order to make our industry best in world.",
    "And, while we're at it(wait for it) we'll get rich!","Let's start by pumping some Oil. Tap on the pump to extract oil from sea" };

    private float firstTime;
    private int tapped = 0;
    public GameObject[] arrow;

    private void Start()
    {
        firstTime = PlayerPrefs.GetInt("firstTime", 1);
    }
    private IEnumerator DelayedIt()
    {
        yield return new WaitForSeconds(0.1f);

    }
    private void Update()
    {
        if (firstTime == 1)
        {
            Camera.main.gameObject.GetComponent<CameraPan>().enabled = false;
            if (tapped == 0)
            {
                tutorial.SetActive(true);
                textBox.text = text[0];
            }
            else if(tapped == 1)
            {
                textBox.text = text[1];
            }
            else if (tapped == 2)
            {
                textBox.text = text[2];
            }
            else if (tapped == 3)
            {
                textBox.text = text[3];
            }
         /*   else if (tapped == 4)
            {
                textBox.text = text[4];
            }
            else if (tapped == 5)
            {
                textBox.text = text[5];
            }*/
            else if(tapped == 4)
            {
                arrow[0].SetActive(true);
                tutorial.SetActive(false);

                if (OilProduction.instance.currentOilValue > 10f)
                {
                    arrow[0].SetActive(false);
                    tapped++;
                }
            }
            else if(tapped == 5)
            {
                arrow[1].SetActive(true);
                tutorial.SetActive(true);
                textBox.text = text[4];
            }
            else if(tapped == 6)
            {
                arrow[1].SetActive(false);
                textBox.text = text[5];
            }
            else if(tapped == 7)
            {
                arrow[2].SetActive(true);
                tutorial.SetActive(false);

                if(GameManager.instance.moneyEarned > 15f)
                {
                    tapped++;
                }
            }
            else if(tapped == 8)
            {
                tutorial.SetActive(true);
                arrow[2].SetActive(false);
                arrow[3].SetActive(true);
                arrow[4].SetActive(true);
                arrow[5].SetActive(true);
                textBox.text = text[6];
            }
            else if(tapped == 9)
            {
                tutorial.SetActive(false);
                arrow[3].SetActive(false);
                arrow[4].SetActive(false);
                arrow[5].SetActive(false);
                if (UpgradeHandler.instance.level_rig > 1 || UpgradeHandler.instance.level_Refine > 1 || UpgradeHandler.instance.level_ship > 1)
                {
                    UIManager.instance.dialog_Box.SetActive(false);
                    tapped++;
                }
            }
            else if(tapped == 10)
            {
                tutorial.SetActive(true);
                textBox.text = text[7];

            }
            else if (tapped == 11)
            {
                tutorial.SetActive(true);
                textBox.text = text[8];
                arrow[6].SetActive(true);
            }
            else if (tapped == 12)
            {

                arrow[6].SetActive(false);
                textBox.text = text[9];


            }
            else if(tapped == 13)
            {
          
                tutorial.SetActive(false);
                Camera.main.gameObject.GetComponent<CameraPan>().enabled = true;
                firstTime = 0;
                PlayerPrefs.SetInt("firstTime", 0);
            }
            

        }
    }

    public void TappedUpdate()
    {
        tapped++;
    }
}
