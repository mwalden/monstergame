using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float maxLength;
    public float allowedDiff;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled)
        {
            Vector3 direction = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
            float dist = (Vector2.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0)));
            RaycastHit2D ray = Physics2D.Raycast(lineRenderer.GetPosition(0), direction, dist, mask);

            if (ray.collider != null)
            {

                if (Vector2.Distance(ray.point, lineRenderer.GetPosition(1)) > allowedDiff)
                {
                    print("disabling : " + Vector2.Distance(ray.point, lineRenderer.GetPosition(1)));
                    lineRenderer.enabled = false;
                }
                    
            }
        }
        //draw a ray to position(1). if there are no colliders, then keep it. otherwise, remove it. 
        if (Vector2.Distance(lineRenderer.GetPosition(0),lineRenderer.GetPosition(1)) > maxLength)



            lineRenderer.enabled = false;
        else
            lineRenderer.SetPosition(0, gameObject.transform.parent.position);
    }
}
