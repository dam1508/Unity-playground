using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    [SerializeField] float power = 10F;
    [SerializeField] float lifeTime = 2F;
    [SerializeField] LayerMask terrain;

    private Camera mainCamera;
    private Vector2 startingVector;
    private HingeRope hr;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private PlayerInput pi;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        hr = this.gameObject.GetComponent<HingeRope>();
        pi = this.gameObject.transform.parent.parent.GetComponent<PlayerInput>();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        CalculateVector();
        SetVelocity();
        //Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        DrawLine();
    }

    void DrawLine()
    {
        lineRenderer.SetPosition(0, this.gameObject.transform.position);
        lineRenderer.SetPosition(1, this.gameObject.transform.parent.position);

    }

    private void SetVelocity()
    {
        rb.velocity = startingVector * power;
    }

    private void CalculateVector()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 grapplingGunPos = new Vector2(transform.position.x, transform.position.y);
        startingVector = mousePos - grapplingGunPos;
        startingVector.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            //hr.CreateRope();
            //HingeJoint2D hj = gameObject.transform.parent.parent.gameObject.AddComponent<HingeJoint2D>() as HingeJoint2D;
            //hj.connectedBody = hr.GetLastSegmentRB();
            //SpringJoint2D sj = gameObject.AddComponent<SpringJoint2D>() as SpringJoint2D;
            //sj.connectedBody = gameObject.transform.parent.parent.GetComponent<Rigidbody2D>();
            DistanceJoint2D dj = gameObject.AddComponent<DistanceJoint2D>() as DistanceJoint2D;
            dj.connectedBody = gameObject.transform.parent.parent.GetComponent<Rigidbody2D>();
            dj.maxDistanceOnly = true;
            pi.SetGrappled(true);
        }
    }

    public void DestroyHook()
    {
        Destroy(gameObject);
        pi.SetGrappled(false);
    }
}
