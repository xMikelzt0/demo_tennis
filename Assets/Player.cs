using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget;
    float speed = 3f;
    float force = 13;
    bool hitting;
    public Transform ball;

    Vector3 aimTargetInitialPosition;

    Animator animator;

    Manager manager;
    Shot currentShot;
    // Start is called before the first frame update

    [SerializeField] Transform ServeRight;
    [SerializeField] Transform ServeLeft;
    bool ServedRight = true;
    void Start()
    {
        animator = GetComponent<Animator>(); 
        aimTargetInitialPosition = aimTarget.position;
        manager = GetComponent<Manager>();
        currentShot = manager.topSpin;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true; 
            currentShot = manager.topSpin; 
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; 
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; 
            currentShot = manager.flat; 
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hitting = true;
            currentShot = manager.flatServe;
            GetComponent<BoxCollider>().enabled = false;          
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            hitting = true;
            currentShot = manager.kickServe;
            GetComponent<BoxCollider>().enabled = false;

        }

        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyDown(KeyCode.T))
        {
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0, 1, 0.2f);
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            ball.GetComponent<Ball>().hitter = "Player";
            ball.GetComponent<Ball>().playing = true;
        }

        
        //else if (Input.GetKeyUp(KeyCode.T))
        //{
        //    hitting = false;
        //    GetComponent<BoxCollider>().enabled = true;
        //    ball.transform.position = transform.position + new Vector3(0, 1, 0.2f);
        //    Vector3 dir = aimTarget.position - transform.position;
        //    ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
        //}

        if (hitting)  
        {
            aimTarget.Translate(new Vector3(0, 0, h) * speed * 2 * Time.deltaTime); 
        }

        if ((h != 0 || v != 0) && !hitting) 
        {
            transform.Translate(new Vector3(-v, 0, h) * speed * Time.deltaTime); 
        }

        Vector3 ballDir = ball.position - transform.position;
        if (ballDir.z >= 0)
        {
            Debug.Log("Forehand");
        }
        else
        {
            Debug.Log("Backhand");
        }
        Debug.DrawRay(transform.position, ballDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))  
        {
            Vector3 dir = aimTarget.position - transform.position; 
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

            ball.GetComponent<Ball>().hitter = "Player";

            aimTarget.position = aimTargetInitialPosition;

        }
    }

    public void Reset()
    {
        if (ServedRight)
            transform.position = ServeLeft.position;
        else
            transform.position = ServeRight.position;

        ServedRight = !ServedRight;
    }
}
