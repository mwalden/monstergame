using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CreatureManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        Swarming,
        Moving,
        Attacking
    }
    public GameObject destinationPrefab;
    GameObject currentDestination;
    Creature[] creatures;
    public State state = State.Idle;
    bool swarming;
    public Vector2 swarmPoint;
    // Start is called before the first frame update
    void Start()
    {
        creatures = GameObject.FindObjectsOfType<Creature>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setMoveTo();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            state = State.Attacking;
        }
        if (state.Equals(State.Attacking))
        {
            attack();
        }
    }
    public void MarkDestinationReachedByOne(GameObject creature)
    {
        if (state.Equals(State.Attacking))
            return;
        if (!state.Equals(State.Swarming) )
        {
            creature.GetComponent<Creature>().setReachedDestination(true);
            swarmPoint = creature.transform.position;
            state = State.Swarming;
        }
        swarming = true;
    }
    public void setMoveTo()
    {
        //print("Moving");
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 destination = new Vector3(worldPoint.x, worldPoint.y, 0);

        //DestroyImmediate(currentDestination);
        //currentDestination = Instantiate(destinationPrefab, transform);
        //currentDestination.transform.position = destination;
        foreach (Creature creature in creatures)
        {
            creature.GetComponent<NavMeshAgent>().destination = destination;
        }
            

    }

    
    private void attack()
    {
        AttackableTarget[] attackableTargets = FindObjectsOfType<AttackableTarget>();
        bool allDead = true;
        
        foreach (AttackableTarget at in attackableTargets)
        {
            print(at + " : " + at.alive);
            if (at.alive)
            {
                allDead = false;
                break;
            }
        }
        if (allDead)
        {
            print("Returning");
            state = State.Idle;
            return;
        }
        foreach (AttackableTarget at in attackableTargets)
        {
           
            foreach (Creature creature in creatures)
            {
                if (!at.beingAttacked())
                {
                    float distance = Vector2.Distance(creature.transform.position, at.transform.position);
                    if (distance < 3)
                        at.setBeingAttacked();
                }
                else{
                    //move to person
                    if (at.alive)
                    {
                        Vector2 targetToMoveTo = creature.transform.position + (at.transform.position - creature.transform.position).normalized * 5;
                        at.GetComponent<NavMeshAgent>().destination = targetToMoveTo;
                    }
                }
                
                Vector2 destination = at.transform.position;
                creature.GetComponent<NavMeshAgent>().destination = destination;
            }
        }
      
    }
}
