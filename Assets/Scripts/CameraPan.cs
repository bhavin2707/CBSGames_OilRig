using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

    Vector3 touchStart;

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, -5, 5), Mathf.Clamp(Camera.main.transform.position.y, 0, 5), -10f);
  //      Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, -150f, 150f), Mathf.Clamp(Camera.main.transform.position.y, 0, 100f), -100f);
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit != null && hit.collider != null)
            {
                touchStart = -Vector3.zero;
                return;
            }
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0) && (touchStart != -Vector3.zero))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
    }

}
