using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaysOverRadius : MonoBehaviour
{
    Camera cam = null;
    public LayerMask mask;
    public int numberOfRays;
    [Range(0,1000)]
    public float length;
    [Range(0, 180)]
    public float delta;
    SpringJoint2D activeSpring;
    LineRenderer activeLine;
    
    void Start()
    {
        cam = Camera.main;
        activeLine = GetComponent<LineRenderer>();
        activeLine.enabled = false;
        
    }
    public void findAndDrawLine(List<Vector2> points)
    {
        Vector2 p = Vector2.zero;
        if (points.Count > 0)
            p = points[0];
        foreach(Vector2 point in points)
        {
            
            float proposedDistance = Vector2.Distance(transform.localPosition, point);
            float currentDistance = Vector2.Distance(transform.localPosition, p);            
            if (proposedDistance > currentDistance)
            {
                p = point;
            }
            
        }
        if (p != Vector2.zero)
        {
            Object.Destroy(activeSpring);

            activeLine.SetPosition(0, transform.position);
            activeLine.SetPosition(1, p);
            GetComponent<Rigidbody2D>().AddForce(p);
            activeLine.enabled = true;
            //SpringJoint2D spring = Instantiate<SpringJoint2D>(sp2d, transform);
            SpringJoint2D spring = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;

            spring.connectedAnchor = p;
            spring.autoConfigureConnectedAnchor = false;
            spring.autoConfigureDistance = false;
            spring.distance = .5f;
            spring.frequency = 3;
            activeSpring = spring;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            List<Vector2> points = findPoints(mousePosition);
            if (points.Count >0)
                findAndDrawLine(points);
        }
        if (activeSpring != null)
        {
            if (Vector2.Distance(activeSpring.connectedAnchor, transform.position) < 1f)
            {
                activeLine.enabled = false;
            }
            else
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                activeLine.SetPosition(0, transform.position);
            }
        }
      
    }

    public List<Vector2> findPoints(Vector3 vector)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 direction = vector - transform.position;
        Debug.DrawRay(transform.position, vector - transform.position, Color.red,5000);
        for (int i = 1;i< 5; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(5*i, Vector3.forward);
            Vector2 rotatedDirection = rotation * direction;
            float dist = (Vector2.Distance(transform.position, rotatedDirection));
            RaycastHit2D ray = Physics2D.Raycast(transform.position, rotatedDirection, dist, mask);

            if (ray.collider != null)
            {
                points.Add(ray.point);
            }

            Debug.DrawRay(transform.position, rotatedDirection, Color.green,5000);
        }
        return points;
    }
}

