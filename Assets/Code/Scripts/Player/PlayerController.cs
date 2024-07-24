using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject StepSound;
    public AudioSource Attack1;
    public AudioSource Attack1_1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    
    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Set the movement vector
        movement = new Vector2(moveX, moveY).normalized;
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x == 0)
        {

        }
        // Update animator parameters
        if (movement != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            StepSound.SetActive(true);
            
        }
        else
        {
            animator.SetBool("isWalking", false);
            StepSound.SetActive(false);


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        Attack1.Play();
        StartCoroutine(TimeAttack1());
    }
    IEnumerator TimeAttack1()
    {
        yield return new WaitForSeconds(0.3f);
        Attack1_1.Play();



    }
    void FixedUpdate()
    {
        // Move the player
        rb.velocity = movement * moveSpeed;
    }
}
