using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    Rigidbody2D rb;
    public float timeToFall = 5f;
    float timeLeft;
    bool falling;
    public float yForce;
    
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeToFall;
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (falling)
        {
            timeLeft -= Time.deltaTime;
        }
        if (falling && timeLeft < 0)
        {
            falling = false;
            rb.simulated = false;
            rb.gravityScale = 1;
            rb.velocity = Vector2.zero;
        }
    }

    public void explode()
    {
        
        rb.simulated = true;
        timeLeft = timeToFall;
        print("adding force " + yForce);
        float x = Random.Range(-200f, 200f);
        float y = Random.Range(-50f, 50f);
        rb.AddForce(new Vector2(x, yForce + y));
        falling = true;
    }
}
