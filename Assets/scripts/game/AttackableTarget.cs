using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackableTarget : MonoBehaviour
{
    private bool attacked;
    private Animator animator;
    public int hp;
    public bool alive;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool beingAttacked()
    {
        return attacked;
    }

    public void hitTarget(int power)
    {
        hp -= power;
        if (hp <= 0 && alive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            alive = false;
            AddForce[] forces = GetComponentsInChildren<AddForce>();
            foreach (AddForce force in forces)
            {
                force.explode();
            }
        }
    }

    public void setBeingAttacked()
    {
        if (!attacked) { 
            attacked = true;
            animator.SetTrigger("attack");
        }
    }
}
