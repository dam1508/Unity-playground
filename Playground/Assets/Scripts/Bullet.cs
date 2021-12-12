using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bc.enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }
}
