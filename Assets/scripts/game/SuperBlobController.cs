using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct ClosestVector
{
    public ClosestVector(bool found){
        this.x = -1;
        this.y = -1;
        Found = found;
    }
    public ClosestVector(float x, float y)
    {
        this.x = x;
        this.y = y;
        Found = true;
    }
    public float x { get; }
    public float y { get; }
    public bool Found { get; set; }

}

//https://www.reddit.com/r/Unity2D/comments/fqosoy/super_hyped_for_carrion_to_come_out_so_im_trying/
public class SuperBlobController : MonoBehaviour
{
    Camera cam = null;
    List<GameObject> subBlobs;
    public LayerMask mask;
    public float maxSpeed;
    public float appliedForce;

    public GameObject primaryBlob;

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
        if (Input.GetKey(KeyCode.Space))
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            //we want to only apply force if one active tentacle is out
            List<Vector2> activeTentacles = getActiveTentaclesExtendedPositions();

            GameObject closestBlob = findClosestBlob(mousePosition);
            
            if (closestBlob == null)
                return;
            primaryBlob = closestBlob;
            List<Vector2> rays = collectAllRaysByBlob(closestBlob, mousePosition,100);
            if (rays.Count > 0 && activeTentacles.Count>0)
            {
                cam.GetComponent<CameraController>().blob = closestBlob;
                ClosestVector closestVector = findClosestPoint(rays, mousePosition, true);
                Vector2 closestPoint = new Vector2(closestVector.x, closestVector.y);

                List<GameObject> lineOfSightBlobs = subBlobs.Where(x => doesBlobHaveLineOfSight(x, closestPoint)).ToList<GameObject>();
                applyForcesOnBlobs(closestBlob, lineOfSightBlobs, closestPoint);
            }

            //we need to make sure any blobs wont be cast if no space is available.

            //List<GameObject> otherBlobs = subBlobs.Where(x => !x.Equals(closestBlob) && doesBlobHaveLineOfSight(x, closestPoint)).ToList<GameObject>();
            List<Vector2> raysForTentacles = collectAllRaysByBlob(closestBlob, mousePosition, 1);
            if (raysForTentacles.Count == 0)
                return;

            ClosestVector closestVectorForTentacles = findClosestPoint(raysForTentacles, mousePosition, false);
            if (closestVectorForTentacles.Found)
            {
                Vector2 closestPoint = new Vector2(closestVectorForTentacles.x, closestVectorForTentacles.y);
                closestBlob.GetComponent<Blob>().attach(closestPoint, maxSpeed);
            }
                
        }
    }

    List<Tentacle> GetTentacles()
    {
        List<Tentacle> tentacles = new List<Tentacle>();
        foreach (GameObject go in subBlobs)
        {
            Blob blob = go.GetComponent<Blob>();
            foreach (LineRenderer line in blob.armArray)
            {
                Tentacle tentacle = line.GetComponent<Tentacle>();
                if (tentacle.tentacleState.Equals(Tentacle.TentacleState.extended))
                {
                    tentacles.Add(tentacle);
                }

            }
        }
        return tentacles;
    }

    List<Vector2> getActiveTentaclesExtendedPositions()
    {
        List<Vector2> lines = new List<Vector2>();
        foreach (GameObject go in subBlobs)
        {
            Blob blob = go.GetComponent<Blob>();
            foreach (LineRenderer line in blob.armArray)
            {
                Tentacle tentacle = line.GetComponent<Tentacle>();
                if (line.enabled && tentacle.tentacleState.Equals(Tentacle.TentacleState.extended))
                {
                    lines.Add(line.GetPosition(1));
                }
                    
            }
        }
        return lines;
    }

    void applyForcesOnBlobs(GameObject primaryBlob, List<GameObject> blobs,Vector2 vector)
    {

        float distance = Vector2.Distance(primaryBlob.transform.position, vector);
        if (distance < 2)
            return;
        Vector2 blobVector = new Vector2(primaryBlob.transform.position.x, primaryBlob.transform.position.y);
        //dot product to find out if there is AT LEAST one extended tentacle
        //that is in the same direction
        Vector2 directionalVector = vector - blobVector;
        List<Tentacle> tentacles = GetTentacles();
        bool hasTentacleInFront = false;
        foreach (Tentacle tentacle in tentacles)
        {
            //GetTentacles() only returns extended tentacles
            Vector2 tentacleExtendedPosition = tentacle.GetComponent<LineRenderer>().GetPosition(1);
            Vector2 blobToTentacleDirection = tentacleExtendedPosition - blobVector;
            float dotProduct = Vector2.Dot(blobToTentacleDirection.normalized, directionalVector.normalized);
            if (dotProduct > .75)
                hasTentacleInFront = true;
        }
        if (!hasTentacleInFront)
        {
            print("no tentacle in front");
            return;
        }
        
        foreach (GameObject go in blobs)
        {            
            Vector2 force = directionalVector * appliedForce;
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            else
               rb.AddForce(force);
                
        }
    }

    bool doesBlobHaveLineOfSight(GameObject blob, Vector3 destination)
    {
        Vector2 direction = destination - blob.transform.position;
        float dist = (Vector2.Distance(blob.transform.position, direction)) + 1;
        RaycastHit2D ray = Physics2D.Raycast(blob.transform.position, direction, dist, mask);
        if (ray.collider != null)
        {
            return Vector2.Distance(ray.point, destination) < .1f;
        }

        return false;
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

    List<Vector2> collectAllRaysByBlob(GameObject blob, Vector2 mousePosition, float distanceMultiplier)
    {
        RaysOverRadius ror = blob.GetComponent<RaysOverRadius>();
        List<Vector2> rays = ror.findPoints(mousePosition,distanceMultiplier);
        return rays;
    }

    //we may have to take into account if its extended or not .
    bool isThisPointTooCloseToAllTentacles(List<Vector2> tentacleLocations, Vector2 point)
    {
        if (tentacleLocations.Count == 0)
            return false;
        bool result = (tentacleLocations.FindAll(tentacleLocation => {
            return Vector2.Distance(tentacleLocation, point) > 3f;
        }).LongCount() != tentacleLocations.Count);
        return result;
    }

    ClosestVector findClosestPoint(List<Vector2> rays, Vector2 mousePosition, bool ignoreTentacleLocation)
    {
        Vector2 closestRay = new Vector2(int.MaxValue,int.MaxValue);
        List<Vector2> activeTentacles = getActiveTentaclesExtendedPositions();
        ClosestVector closest = new ClosestVector(false);
        foreach (Vector2 ray in rays)
        {
            if (!ignoreTentacleLocation && isThisPointTooCloseToAllTentacles(activeTentacles, ray))
                continue;
                
            float currentClosestDistance = Vector2.Distance(closestRay,mousePosition) ;
            float proposedDistance = Vector2.Distance(mousePosition, ray);
            
            if (proposedDistance < currentClosestDistance)
            {
                closestRay = ray;
                closest = new ClosestVector(ray.x,ray.y);
            }
        }
        return closest;
    }
  
}
