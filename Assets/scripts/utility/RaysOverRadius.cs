using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaysOverRadius : MonoBehaviour
{
    public LayerMask mask;
    public int numberOfRays;
    [Range(0,10)]
    public float length;
    [Range(0, 180)]
    public float delta;

    public List<Vector2> findPoints(Vector3 vector,float distanceMultipler)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 direction = (vector - transform.position);
        Debug.DrawRay(transform.position, vector - transform.position, Color.red);
        for (int i = 0;i< numberOfRays; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(delta*i, Vector3.forward);
            Vector2 rotatedDirection = rotation * direction;
            float dist = (Vector2.Distance(rotatedDirection,transform.position  ) * distanceMultipler);
            Debug.DrawRay(transform.position, rotatedDirection, Color.green);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, rotatedDirection, dist, mask);

            if (ray.collider != null)
            {
                points.Add(ray.point);
            }

            Quaternion rotationDown = Quaternion.AngleAxis(delta * -i, Vector3.forward);

            Vector2 rotatedDirectionDown = rotationDown * direction;
            
            Vector2 normizedDirectionDown = rotatedDirectionDown.normalized;
            float distDown = (Vector2.Distance(transform.position, rotatedDirectionDown));
            RaycastHit2D rayDown = Physics2D.Raycast(transform.position, normizedDirectionDown, distDown, mask);

            if (rayDown.collider != null)
            {
                points.Add(rayDown.point);
            }



            Debug.DrawRay(transform.position, rotatedDirectionDown, Color.green);
        }
        return points;
    }
}

