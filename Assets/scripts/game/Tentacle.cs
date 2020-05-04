using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public enum TentacleState
    {
        disabled,
        casting,
        extended
    }
    public TentacleState tentacleState;
    LineRenderer lineRenderer;
    public float maxLength;
    public float allowedDiff;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tentacleState = TentacleState.disabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled)
        {
            
            //disables around corners
            lineRenderer.SetPosition(0, gameObject.transform.parent.position);
            
            Vector3 direction = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
            float dist = (Vector2.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0)));
            //lineRenderer.material.mainTextureScale = new Vector2(dist * 2, 1);
            if (dist > maxLength)
            {
                tentacleState = TentacleState.disabled;
                lineRenderer.enabled = false;
            }

            RaycastHit2D ray = Physics2D.Raycast(lineRenderer.GetPosition(0), direction, dist, mask);

            if (ray.collider != null)
            {
                if (Vector2.Distance(ray.point, lineRenderer.GetPosition(1)) > allowedDiff)
                {
                    tentacleState = TentacleState.disabled;
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    public IEnumerator ExtendTentacle(float time, Vector2 destination)
    {
        tentacleState = TentacleState.casting;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            Vector2 position = Vector2.Lerp(lineRenderer.GetPosition(0), destination, (elapsedTime / time));
            lineRenderer.SetPosition(1, position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tentacleState = TentacleState.extended;
        
    }

}
