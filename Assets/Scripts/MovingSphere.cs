using UnityEngine;
using System.Collections;

public class MovingSphere : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    Vector3 velocity;

    Rect allowedArea = new Rect(-50, -50f, 100f, 100f);
    Rigidbody body;
    Vector3 desiredVelocity;
    bool desiredJump;
    float jumpHeight = 2f;
    public bool onGround;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 2;
    int jumpPhase;
    [SerializeField, Range(0f, 100f)]
    public float maxAcceleration = 10f, maxAirAcceleration = 1f;
    bool desireBomb;
    float bombSpeed = 20f;
    Vector3 bombDirection;
    Camera mainCamera;
    [SerializeField]
    BombFactory bombFactory;
    private void Awake()
    {
        //bombFactory = new BombFactory();
        mainCamera = Camera.main;
        body = GetComponent<Rigidbody>();
    }
    
    void Update()
    {

        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        desiredJump |= Input.GetButtonDown("Jump");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        desireBomb |= Input.GetMouseButtonDown(0);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,mousePosition.y,10));
        bombDirection = new Vector3(mousePosition.x- transform.localPosition.x ,
            mousePosition.y - transform.localPosition.y, mousePosition.z - transform.localPosition.z).normalized;
        if (desireBomb)
        {
            //Debug.Log("bug");
            //Debug.Log(transform.localPosition);
            //Debug.Log(transform.position);
            //Debug.Log(mousePosition);

        }
        //bombDirection = 
        //    new Vector3(bombDirection.x-transform.localPosition.x,bombDirection.y- transform.localPosition.y,
        //   bombDirection.z - transform.localPosition.z).normalized;

    }
    private void FixedUpdate()
    {
        UpdateState();
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }
        if (desireBomb)
        {
            desireBomb = false;
            if (onGround)
            {
            Bomb newBomb = bombFactory.Get();
            Transform t = newBomb.transform;
                //t.SetParent(transform);
            t.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y+1,transform.localPosition.z) ;
            Vector3 bombDesireVelocity = new Vector3(bombSpeed * bombDirection.x,
                    bombSpeed * bombDirection.y, bombSpeed * bombDirection.z);
            newBomb.ThrowBomb(bombDesireVelocity);
            
            }

        }
        body.velocity = transform.TransformDirection(velocity);
        ResetState();
    }
    
    void UpdateState()
    {
        velocity = transform.InverseTransformDirection(body.velocity);
        if (onGround)
        {
            jumpPhase = 0;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void ResetState()
    {
        onGround = false;
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }
    void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);

            }
            velocity.y += jumpSpeed;
        }
    }
    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}