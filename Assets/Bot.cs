using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    float speed = 20; 
    float force = 13;
    Animator animator;
    public Transform ball;
    public Transform aimTarget;

    public Transform[] targets;

    Manager manager;


    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        manager = GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        targetPosition.z = ball.position.z;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector3 PickTarget() 
    {
        int randomValue = Random.Range(0, targets.Length); 
        return targets[randomValue].position; 
    }

    Shot PickShot() // picks a random shot to be played
    {
        int randomValue = Random.Range(0, 2); 
        if (randomValue == 0) 
            return manager.topSpin;
        else                   
            return manager.flat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) 
        {
            Shot currentShot = PickShot(); 

            Vector3 dir = PickTarget() - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);            
            Vector3 ballDir = ball.position - transform.position; 
            if (ballDir.z >= 0) 
            {
                animator.Play("Forehand"); 
            }
            else
            {
                animator.Play("Backhand"); 
            }

            ball.GetComponent<Ball>().hitter = "Bot";
        }
    }
}
