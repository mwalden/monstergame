using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    SpringJoint2D activeSpring;
    LineRenderer activeLine;
    public int allowedLines;
    List<LineRenderer> lines;
    List<SpringJoint2D> springs;
    public GameObject lineRendererPrefab;
    public GameObject springJointPrefab;
    public int armIndex;
    // Start is called before the first frame update
    void Start()
    {
        activeLine = GetComponent<LineRenderer>();
        lines = new List<LineRenderer>();
        springs = new List<SpringJoint2D>();
        for (int i =0; i < allowedLines; i++)
        {
            //SpringJoint2D spring = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
            //LineRenderer line = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            GameObject lineGO = Instantiate(lineRendererPrefab, transform);
            GameObject springGO = Instantiate(springJointPrefab, transform);
            LineRenderer line =lineGO.GetComponent<LineRenderer>();
            SpringJoint2D spring = springGO.GetComponent<SpringJoint2D>();

            spring.autoConfigureConnectedAnchor = false;
            spring.autoConfigureDistance = false;
            spring.distance = .5f;
            spring.frequency = 2;
            spring.breakForce = 4;
            spring.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            line.enabled = false;
            spring.enabled = false;
            lines.Add(line);
            springs.Add(spring);


        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (activeLine.enabled)
        //{
        //    if (Vector2.Distance(activeSpring.connectedAnchor, transform.position) > 1f)
        //    {
        //        activeLine.SetPosition(0, transform.position);
        //    }
            
        //}
        
    }
    public void detach()
    {
        //activeLine.enabled = false;
        //Object.Destroy(activeSpring);
    }

    public void attach(Vector2 position)
    {
        if (armIndex == allowedLines)
        {
            armIndex = 0;
        }
        SpringJoint2D spring = springs[armIndex];// gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        
        LineRenderer line = lines[armIndex];
        //look at setting the force depending on the position of this in relation to the transform.position;
        spring.connectedAnchor = position;
        spring.enabled = true;

        //activeSpring = spring;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, position);
        line.enabled = true;
        armIndex++;
        
    }


}
