using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    private Vector3 destination;
    private bool atDestination = true;
    public float speed;

    public Vector2 center;
    public float radius = 2f;
    public GridManager gm;
    public CreatureManager cm;
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        gm = GameObject.FindObjectOfType<GridManager>();
        cm = GameObject.FindObjectOfType<CreatureManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!atDestination)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                atDestination = true;
                gm.MarkTaken(transform.position.x, transform.position.y);
                cm.MarkDestinationReachedByOne(transform.gameObject);
            }
        }
        //else
        //{
        //    Vector2 centerOffset = center - (Vector2)transform.position;
        //    float t = centerOffset.magnitude / radius;
        //    if (t < 0.9f)
        //        destination = Vector2.zero;
        //    else
        //        destination = centerOffset * t * t;
        //    atDestination = false;
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        //    destination = new Vector3(worldPoint.x,worldPoint.y,transform.position.z);
        //    gm.MarkFree(transform.position.x, transform.position.y);

        //}
    }

    public void setDestination(Vector3 destination)
    {
        gm.MarkFree(transform.position.x, transform.position.y);
        this.destination = destination;
        atDestination = false;
        if (destination.x < transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
}
