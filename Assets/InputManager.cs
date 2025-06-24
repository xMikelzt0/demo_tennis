using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
    public Vector2 moveVector;

    public float moveSpeed = 3f;

    public void InputPlayer(InputAction.CallbackContext _context)
    {
        moveVector = _context.ReadValue<Vector2>();
    }

    //// Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(-moveVector.y, 0, moveVector.x);
        movement.Normalize();
        transform.Translate(moveSpeed * movement * Time.deltaTime);
    }

    
    
    
}
