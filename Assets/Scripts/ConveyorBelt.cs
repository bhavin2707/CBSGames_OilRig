using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorBelt : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float moveSpeed = 1;
    public float switchOffSpeed = 2;
    public float currentSpeed = -1;
    public bool isHighSpeed = false;
    public static ConveyorBelt instance;

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

    }

    private void Update()
    {

        if(moveSpeed < 0)
        {
            moveSpeed *= -1;
        }
        if(isHighSpeed)
        {
            switchOffSpeed -= Time.deltaTime;
            if (switchOffSpeed < 0)
            {
                isHighSpeed = false;
                moveSpeed = currentSpeed;
            //    StartCoroutine(delayedSpeed());
            }
        }
        else
        {
            currentSpeed = -1;
        }

    }

    private IEnumerator delayedSpeed()
    {
        yield return new WaitForSeconds(0.4f);
        moveSpeed = currentSpeed;
    }

    public void UpdateSpeed()
    {
        if (moveSpeed <= 2)
        {
            moveSpeed = currentSpeed;
            moveSpeed += 0.02f;
            currentSpeed = moveSpeed;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(currentSpeed == -1)
        {
            currentSpeed = moveSpeed;
        }
        switchOffSpeed = 2;
        moveSpeed = currentSpeed * 4;
        isHighSpeed = true;
   //     Debug.Log("here");
    }
}