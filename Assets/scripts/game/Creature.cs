using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Creature : MonoBehaviour
{
    public enum State
    {
        Moving,
        Idle,
        Attacking
    }
    public float timeBetweenHits = 1f;
    private float timeLeftBeforeHit;
    private AttackableTarget attackTarget;
    Creature.State state;
    public int attackPower = 5;
    //public bool reachedDestination;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftBeforeHit = timeBetweenHits;
        state = Creature.State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (state.Equals(State.Attacking))
        {
            hitTarget();
        }
    }

    private void hitTarget()
    {
        if (attackTarget != null)
        {
            if (timeLeftBeforeHit <= 0)
            {
                attackTarget.hitTarget(attackPower);
                timeLeftBeforeHit = timeBetweenHits;
                if (!attackTarget.alive)
                {
                    state = State.Idle;
                    attackTarget = null;
                }
            }
            else
                timeLeftBeforeHit -= Time.deltaTime;
        }

        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<AttackableTarget>() != null && state != State.Attacking)
        {
            attackTarget = col.gameObject.GetComponent<AttackableTarget>();
            state = State.Attacking;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<AttackableTarget>() )
        {
            state = State.Moving;
        }

    }

    public void setReachedDestination(bool reached)
    {
        //reachedDestination = reached;
    }
    public Creature.State getState()
    {
        return state;
    }
    
}
