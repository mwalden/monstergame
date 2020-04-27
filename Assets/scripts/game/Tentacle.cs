using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float maxLength;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(lineRenderer.GetPosition(0),lineRenderer.GetPosition(1)) > maxLength)
            lineRenderer.enabled = false;
        else
            lineRenderer.SetPosition(0, gameObject.transform.parent.position);
    }
}
