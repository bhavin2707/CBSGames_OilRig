using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideFinger : MonoBehaviour {
    public GameObject finger;
 //   public GameObject can;

    private void Update()
    {
      /*  if (!can.activeInHierarchy)
            finger.SetActive(true);
        else
            finger.SetActive(false);*/

            finger.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y - 80);
    }
}
