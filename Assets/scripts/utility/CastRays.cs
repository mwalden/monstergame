using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastRays : MonoBehaviour
{
    public float length = 5;
    public LayerMask mask;
    public LineRenderer lineRenderer;
    bool updateRope;
    DistanceJoint2D joint;
    Vector2 previousPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("walls");
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector2 position = move(Vector2.left);
            drawRope(position);
            updateRope = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector2 position = move(Vector2.right);
            drawRope(position);
            updateRope = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 velocity = (pos - previousPosition) / Time.deltaTime;
            GetComponent<Rigidbody2D>().velocity = velocity/2;
            joint.enabled = false;
            lineRenderer.enabled = false;
        }
        if (updateRope)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(joint.connectedAnchor, pos) > .1f){
                lineRenderer.SetPosition(0, transform.position);
            }
            else
            {
                updateRope = false;
                lineRenderer.enabled = false;
            }
        }
        previousPosition = transform.position;


    }

    void drawRope(Vector2 position)
    {
        joint.connectedAnchor = position;
        joint.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, position);
        
        lineRenderer.enabled = true;
    }
    Vector2 move(Vector2 direction)
    {
        RaycastHit2D up = Physics2D.Raycast(transform.position, Vector2.up,length,mask);
        RaycastHit2D fortyFive = Physics2D.Raycast(transform.position, (direction + Vector2.up), length, mask);
        //RaycastHit2D left = Physics2D.Raycast(transform.position, direction, length, mask);
        RaycastHit2D twoSeventyFive = Physics2D.Raycast(transform.position, (Vector2.down + direction), length, mask);
        RaycastHit2D down = Physics2D.Raycast(transform.position, Vector2.down, length, mask);
        //Debug.DrawRay(transform.position, Vector2.up * length, Color.green);
        //Debug.DrawRay(transform.position, (direction + Vector2.up) * length, Color.green);
        //Debug.DrawRay(transform.position, direction * length, Color.green);
        //Debug.DrawRay(transform.position, (direction + Vector2.down) * length, Color.green);
        //Debug.DrawRay(transform.position, Vector2.down * length, Color.green);
        Vector2 winnerRay = transform.position ;
        //print(left.point);
        winnerRay = calcuateDistance(up, winnerRay);
        print(winnerRay);
        winnerRay = calcuateDistance(fortyFive, winnerRay);
        //print(winnerRay);
        //winnerRay = calcuateDistance(left, winnerRay);
        //print(winnerRay);
        winnerRay = calcuateDistance(twoSeventyFive, winnerRay);
        //print(winnerRay);
        winnerRay = calcuateDistance(down, winnerRay);
        //Instantiate<GameObject>(destination,winnerRay,Quaternion.identity);
        return winnerRay;

    }

    Vector2 calcuateDistance(RaycastHit2D ray, Vector2 currentPosition)
    {
        if (ray.collider != null)
        {
            print(ray.collider);
            float proposedDistance = Vector2.Distance(transform.position, ray.point);
            float currentDistance = Vector2.Distance(transform.position, currentPosition);
            if (proposedDistance > currentDistance)
            {
                return ray.point;
            }
            
        }
        return currentPosition;
    }
}
