using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMagic_Controller : MonoBehaviour
{
    [SerializeField] private int damager;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float xVelocity;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private CharacterStats myStats;

    private void Update()
    {
        if (canMove)
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void SetupMagic(float _speed,CharacterStats _mtStats)
    {
        xVelocity = _speed;
        
        myStats = _mtStats;

        if(xVelocity < 0)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            collision.GetComponent<CharacterStats>()?.TakeDamage(damager);
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void FlipArrow()
    {
        if (!flipped)
        {
            return;
        }

        xVelocity = xVelocity * -1;
        flipped = false;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }

    public void MagicDestory()
    {
        Destroy(gameObject);
    }
    
}
