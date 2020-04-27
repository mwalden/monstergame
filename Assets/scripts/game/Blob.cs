using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    public LineRenderer lineRendererPrefab;
    public int maxTentacles;
    public int armIndex;
    public LineRenderer[] armArray;
    
    // Start is called before the first frame update
    void Start()
    {
        armArray = new LineRenderer[maxTentacles];
        for (int i = 0; i < maxTentacles; i++)
        {
            LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
            armArray[i] = lineRenderer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void detach()
    {
        //activeLine.enabled = false;
        //Object.Destroy(activeSpring);
    }

    public void attach(Vector2 position,float maxSpeed)
    {
        
        if (armIndex > 2)
            armIndex = 0;

        LineRenderer lineRenderer = armArray[armIndex].GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, position);
        
        armIndex++;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 directionalVector = position - new Vector2(transform.position.x, transform.position.y);
        Vector2 force = directionalVector * 10;
        rb.AddForce(force);
    }


}
