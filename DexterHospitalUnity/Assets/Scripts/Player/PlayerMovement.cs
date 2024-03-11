using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

    private Animator animator;

    private float inputx, inputy;

    private float stopx, stopy;

   /* [SerializeField]float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

        inputx = Input.GetAxisRaw("Horizontal");
        inputy = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(inputx, inputy).normalized;
        rb.velocity = input * speed;

        if (input != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopx = inputx;
            stopy = inputy;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("InputX", stopx);
        animator.SetFloat("InputY", stopy);
    }
}
