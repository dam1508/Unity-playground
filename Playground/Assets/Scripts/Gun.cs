using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject tip;
    [SerializeField] GameObject bullet;

    [SerializeField] float damage = 10.0F;
    [SerializeField] float projectileSpeed = 1.0F;

    Vector2 currentAim;
    Camera mainCamera;
    bool facingLeft = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Aim();
        Flip();
    }

    private void Aim()
    {
        Vector2 cursor = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = transform.position;
        currentAim = cursor - pos;
        currentAim.Normalize();
        if (currentAim.x < 0 && currentAim.x != -1)
            facingLeft = true;
        else
            facingLeft = false;
        transform.right = currentAim;
    }

    void Flip()
    {
        Vector3 normalScale = new Vector3(1, 1, 1);
        Vector3 flippedScale = new Vector3(1, -1, 1);
        if (facingLeft)
            transform.localScale = flippedScale;
        else
            transform.localScale = normalScale;
    }

    public void Fire()
    {
        GameObject bulletHandler;
        bulletHandler = Instantiate(bullet, tip.transform.position, tip.transform.rotation) as GameObject;

        bulletHandler.GetComponent<Rigidbody2D>().velocity = currentAim * projectileSpeed;

        Destroy(bulletHandler, 3F);
    }
}
