using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float speed = 8f;
    public bool isLadder = false;
    public bool isClimbing = false;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isLadder && Input.GetKeyDown(KeyCode.E))
        { 
            isClimbing = true; 
        }

    }

    private void FixedUpdate()
    {
        if (isClimbing == true)
        {
            rb.gravityScale = 0f;
            
            rb.velocity = new Vector2(0, 1 * speed);
        }
        else
        { rb.gravityScale = 4f; }
    }

    public void OntriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = true;
            Debug.Log("beh");
        }
        
        
        else
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    
}
