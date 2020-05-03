using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    public LineRenderer lineRendererPrefab;
    public int maxTentacles;
    public int armIndex;
    public LineRenderer[] armArray;
    public float lastTimeBetweenArms;
    public bool armAllowed;
    public float extensionTime;
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
        if (armIndex >= maxTentacles)
            armIndex = 0;

        LineRenderer lineRenderer = armArray[armIndex];
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, position);
        //Tentacle tentacle = lineRenderer.GetComponent<Tentacle>();
        //StartCoroutine(tentacle.ExtendTentacle(extensionTime, position));
        armIndex++;

    }


}
