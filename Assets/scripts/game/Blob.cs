using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    SpringJoint2D activeSpring;
    LineRenderer activeLine;
    // Start is called before the first frame update
    void Start()
    {
        activeLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeLine.enabled)
        {
            print(gameObject);
            if (Vector2.Distance(activeSpring.connectedAnchor, transform.position) > 1f)
            {
                activeLine.SetPosition(0, transform.position);
            }
        }
    }
    public void detach()
    {
        activeLine.enabled = false;
        Object.Destroy(activeSpring);
    }

    public void attach(Vector2 position)
    {
        SpringJoint2D spring = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;

        spring.connectedAnchor = position;
        spring.autoConfigureConnectedAnchor = false;
        spring.autoConfigureDistance = false;
        spring.distance = .5f;
        spring.frequency = 3;
        activeSpring = spring;

        activeLine.SetPosition(0, transform.position);
        activeLine.SetPosition(1, position);
        GetComponent<Rigidbody2D>().AddForce(position);
        activeLine.enabled = true;
        
    }


}
