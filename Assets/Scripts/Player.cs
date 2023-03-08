using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CapsuleCollider2D cap;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
    float horizontal_value;
    float vertical_value;
    Vector2 ref_velocity = Vector2.zero;

    float jumpForce = 12f;

    [SerializeField] TrailRenderer tr;
    [SerializeField] float moveSpeed_horizontal = 400.0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool grounded = false;
    [SerializeField] bool is_crouching = false;
    [Range(0, 1)] [SerializeField] float smooth_time = 0.5f;
    [SerializeField] float Climb_speed = 100f;
    public bool isLadder = false;
    public bool canClimb = false;
    //public bool ismagnet = false;
    //[SerializeField] float MagnetRange = 10f;
    //public float force = 10f;
    //public float range = 5f;
    public Transform target;

    bool CheckSphere;
    private Vector2 aidepose;
    [SerializeField] GameObject aide;
    float range = 5;
    float grabPower = 100;
    public GameObject aim;

    // Start is called before the first frame update
    void Start()
    {
        cap = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        // aim = GetComponent.RayonAimant<CapsuleCollider2D>();
        //TargetMagnet = GameObject<Aimantable>();
        //Debug.Log(Mathf.Lerp(current, target, 0));
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");
        vertical_value = Input.GetAxis("Vertical");

        if (horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true;

        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));

        if (Input.GetButtonDown("Jump") && grounded && !is_crouching)
        {
            is_jumping = true;
        }
        //if(Input.GetButtonDown("Fire1")&& ??? aim = true)
        //  {
        // ismagnet = true;
        //}

    }
    void FixedUpdate()
    {
        if (is_jumping && grounded && !canClimb)
        {
            is_jumping = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animController.SetBool("Jumping", true);
            grounded = false;
        }

        if (Input.GetButton("Vertical") && grounded) ;
        {
            Crouch();
        }

        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, rb.velocity.y);

        if (canClimb && vertical_value != 0)
        {
            rb.gravityScale = 0f;
            grounded = false;
            target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.deltaTime, vertical_value * Climb_speed * Time.deltaTime);
            Debug.Log("Test");
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);
       
        
        aim.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim.transform.position = new Vector3(aim.transform.position.x, aim.transform.position.y, 0);
        horizontal_value = Input.GetAxis("Horizontal");
        vertical_value = Input.GetAxis("Vertical");
        //flyingMode = Input.GetButtonDown();

        Vector3 direction = (aim.transform.position - transform.position);
        direction = Vector3.Normalize(direction);
        Debug.DrawRay(transform.position, direction * range, Color.magenta, 0, false);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, range, LayerMask.GetMask("Grabbed"));
        for (int i = 0; i < hits.Length; i++)
        {
            Rigidbody2D rbRef = hits[i].transform.gameObject.GetComponent<Rigidbody2D>();
            if (rbRef.mass > 10)
            {
                rb.AddForce(direction * grabPower * Time.fixedDeltaTime);
            }
            else rbRef.AddForce(-direction * grabPower * Time.fixedDeltaTime);
        }

    }
    private void Crouch()
    {
        aidepose = new Vector2(aide.transform.position.x, aide.transform.position.y);
        CheckSphere = Physics2D.OverlapCircle(aidepose, 0.1f);
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && grounded)
        {
            is_crouching = true;
            moveSpeed_horizontal = 200f;
            cap.offset = new Vector2(0.1f, -0.6f);
            cap.size = new Vector2(1.1f, 0.8f);
            cap.direction = CapsuleDirection2D.Horizontal;
            animController.SetBool("Crouching", true);
        }
        else if (CheckSphere == false)
        {
            is_crouching = false;
            moveSpeed_horizontal = 400f;
            cap.offset = new Vector2(0f, -0.35f);
            cap.size = new Vector2(1f, 1.3f);
            cap.direction = CapsuleDirection2D.Vertical;
            animController.SetBool("Crouching", false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
        animController.SetBool("Jumping", false);

        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimb = true;
            grounded = false;
            Debug.Log("enter");
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canClimb = false;
            grounded = true;
            rb.gravityScale = 3f;
            Debug.Log("exit");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animController.SetBool("Jumping", false);
    }

    
}