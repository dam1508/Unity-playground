using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float hitPoints = 100.0F;
    [SerializeField] Healthbar healthbar;

    float currHealth = 100.0F;
    bool isDead = true;

    [Header ("Movement")]
    [SerializeField] float moveSpeed = 10.0F;
    [SerializeField] float movementSoothing = 1.0F;
    [SerializeField] float jumpPower = 5.0F;
    [SerializeField] bool airControl = false;


    [Header("Terrain Check")]
    [Space(10)]
    [SerializeField] Transform terrain;
    [SerializeField] LayerMask terrainLayer;

    float terrainCheckRadius = .2F;
    bool isGrounded = true;
    bool isGrappled = false;

    [Header("Visual Effects")]
    [Space(10)]
    [SerializeField] GameObject particlesVFX;
    [SerializeField] float VFXTime = 1.0F;
    [SerializeField] Transform landPosition;

    [Header("Camera Settings")]
    [Space(10)]
    [SerializeField] CinemachineVirtualCamera groundedCamera;
    [SerializeField] CinemachineVirtualCamera airCamera;

    Vector2 ref_velocity = Vector2.zero;
    Rigidbody2D rb;
    PlayerInput pi;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        pi = this.gameObject.GetComponent<PlayerInput>();
        healthbar.SetMaxValue(hitPoints);
    }

    private void FixedUpdate()  
    {
        GroundCheck();
    }

    private void Update()
    {
        CameraControl();
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(terrain.position, terrainCheckRadius, terrainLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (wasGrounded == false)
                {
                    LandingVFX();
                }
                isGrounded = true;
            }
        }
    }

    private void CameraControl()
    {
        if (isGrounded)
        {
            groundedCamera.Priority = 1;
            airCamera.Priority = 0;
        }
        else
        {
            groundedCamera.Priority = 0;
            airCamera.Priority = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            TakeDamage(10);
        }
    }
    void TakeDamage(float amount)
    {
        currHealth -= amount;
        //healthbar.SetValue(currHealth);
        if (hitPoints <= 0)
            isDead = true;
    }

    public void Move(float move, bool crouch, bool jump, bool grappled)
    {
        if (isGrounded || airControl)
        {
            Vector2 targetVelocity = new Vector2(moveSpeed * move, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref ref_velocity, movementSoothing);
        }
        if (jump && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jump = false;
        }
        else if (jump && grappled)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpPower);
            jump = false;
            pi.DeattachHook();
        }
    }
    public void LandingVFX()
    {
        GameObject particles = Instantiate(particlesVFX, landPosition.position, Quaternion.identity);
        Destroy(particles, VFXTime);
    }
}
