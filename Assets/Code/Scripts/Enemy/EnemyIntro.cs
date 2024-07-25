using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntro : MonoBehaviour
{
    public static EnemyIntro Singleton;
    public Transform player;
    public bool Walk = false;
    public float speed = 2f;
    public float Distance = 2f;
    public SpriteRenderer spriteRenderer;
    public int countIntro;
    public GameObject DialogueEnemy;
    void Start()
    {
        
    }

    private void Awake()
    {
        
            Singleton = this;
            
       
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Walk)
        {
            if (Vector2.Distance(transform.position, player.position) >= Distance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
                
        }
        if (transform.position.x < player.position.x)
        {
            
            spriteRenderer.flipX = false;
            
        }
        else
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            spriteRenderer.flipX = true;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (countIntro == 1)
            {
                DialogueEnemy.SetActive(true);
            }
            
        }
    }
    public void SetAttackDialogue()
    {
        Walk = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Walk = false;
        }
    }
}
