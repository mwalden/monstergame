using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCube : MonoBehaviour
{
    public Color taken = new Color(1, 0, 0, 0.5f);
    public Color free = new Color(0, 1, 0, 0.5f);
    public Color currentColor;
    private void Start()
    {
         currentColor = free;
}
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        Gizmos.color = currentColor;
        Gizmos.DrawCube(transform.position, new Vector3(16, 16, 10));
    }

    public void Taken()
    {
        currentColor = taken;
    }
    public void Free()
    {
        currentColor = free;
    }
   
}
