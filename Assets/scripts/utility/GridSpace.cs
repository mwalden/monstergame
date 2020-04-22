using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public int x;
    public int y;
    public bool occupied;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{ // if left button pressed...
        //    Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, -Vector2.up);
        //    if (hit.collider != null && hit.collider.gameObject.Equals(gameObject))
        //    {
        //        print(x + " , " + y);
        //    }
        //}
    }
}
