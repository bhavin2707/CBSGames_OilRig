using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private bool moveRight = false;

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, GameObject.Find("Exit").gameObject.transform.position, ConveyorBelt.instance.moveSpeed * Time.deltaTime);
        
        if (moveRight)
        {
            //  transform.localPosition -= Vector3.left * Time.deltaTime * ConveyorBelt.instance.moveSpeed;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, GameObject.Find("Exit").gameObject.transform.position, 1f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Belt")
        {
            moveRight = true;
        }





    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            GameManager.instance.moneyEarned += Shipment.instance.PricePerBarrel;
            UIManager.instance.UpdateMoneyText();
            GameObject go = Instantiate(Resources.Load("StageMoneyText_Barrel") as GameObject, UIManager.instance.NI.transform);
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, -1f);
            StartCoroutine(UIManager.instance.stageRefineIncreaseText("+$" + CalcUtils.FormatNumber(Shipment.instance.PricePerBarrel).ToString(), go));
            StartCoroutine(delayDelete());
        }

        if (collision.gameObject.tag == "Ship")
        {
          /*  Debug.Log("Ya");
            if (collision.gameObject != gameObject)
            {
                if(collision.GetComponent<SpriteRenderer>().enabled == false)
                {
                    this.GetComponent<SpriteRenderer>().enabled = true;
                    return;
                }
                else
                {
                    collision.GetComponent<SpriteRenderer>().enabled = false;
                }
            }*/
        }
    }

    private IEnumerator delayMove()
    {
        yield return new WaitForSeconds(0.1f);

    }

    private IEnumerator delayDelete()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
    }
}
