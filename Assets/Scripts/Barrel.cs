using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private bool moveRight = false;


	void Update ()
    {
        if(moveRight)
        {
            transform.localPosition -= Vector3.left * Time.deltaTime * ConveyorBelt.instance.moveSpeed;
        }
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Belt")
        {
     //       transform.GetComponent<Rigidbody2D>().isKinematic = true;
            moveRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            GameManager.instance.moneyEarned += Shipment.instance.PricePerBarrel;
            UIManager.instance.UpdateMoneyText();
            GameObject go = Instantiate(Resources.Load("StageMoneyText_Barrel") as GameObject, UIManager.instance.gameObject.transform);
            StartCoroutine(UIManager.instance.stageRefineIncreaseText("+$" + CalcUtils.FormatNumber(Shipment.instance.PricePerBarrel).ToString(), go));
            StartCoroutine(delayDelete());
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
