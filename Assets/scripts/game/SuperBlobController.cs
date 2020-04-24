using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.reddit.com/r/Unity2D/comments/fqosoy/super_hyped_for_carrion_to_come_out_so_im_trying/
public class SuperBlobController : MonoBehaviour
{
    Camera cam = null;
    List<GameObject> subBlobs;
    Blob activeBlob;
    void Start()
    {
        subBlobs = new List<GameObject>();
        cam = Camera.main;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                subBlobs.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            //Dictionary<GameObject, List<Vector2>> lines = collectAllLinesByGameObject(mousePosition);
            //findClosetBlob
            //find rays for that blob
            //find closest one ? or furthest ray
            //detach existing spring
            //  create function on blob to detach its spring or decreae its strength to break?
            //attach new spring from closest blob
            //look into the line of sight for this

            GameObject closestBlob = findClosestBlob(mousePosition);
            
            List<Vector2> rays = collectAllRaysByBlob(closestBlob, mousePosition);
            if (rays.Count == 0)
                return;
            if (activeBlob != null)
                activeBlob.detach();
            activeBlob = closestBlob.GetComponent<Blob>();
            Vector2 closestPoint = findClosestPoint(rays, mousePosition);
            
            closestBlob.GetComponent<Blob>().attach(closestPoint);

        }
    }
    GameObject findClosestBlob(Vector2 mousePosition)
    {
        GameObject go = null;
        float currentClosest = float.MaxValue;
        foreach (GameObject blob in subBlobs)
        {
            float distance = Vector2.Distance(blob.transform.position, mousePosition);
            if (distance < currentClosest)
            {
                go = blob;
                currentClosest = distance;
            }
        }
        
        
        return go;
    }

    List<Vector2> collectAllRaysByBlob(GameObject blob, Vector2 mousePosition)
    {
        RaysOverRadius ror = blob.GetComponent<RaysOverRadius>();
        List<Vector2> rays = ror.findPoints(mousePosition);
        return rays;
    }

    Vector2 findClosestPoint(List<Vector2> rays, Vector2 mousePosition)
    {
        Vector2 closestRay = new Vector2(int.MaxValue,int.MaxValue);
        print("Rays : " + rays.Count);
        foreach (Vector2 ray in rays)
        {
            float currentClosestDistance = Vector2.Distance(closestRay,mousePosition) ;
            float proposedDistance = Vector2.Distance(mousePosition, ray);
            if (proposedDistance < currentClosestDistance)
            {
                closestRay = ray;
            }
        }
        return closestRay;
    }
    void findLineOfSightRay()
    {

    }
}
