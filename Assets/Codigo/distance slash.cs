using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class distanceslash : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float distanceslashSpeed;
    [SerializeField] Vector2 distanceslashDirection;
    [SerializeField] float destroyDelay;


    //start is called before the first frame update

    void awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //start is called once per frame

 void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDistanceslashspeed(float speed)
    {
        this.distanceslashSpeed = speed;
    }

    public void SetdistanceSlashDirection(Vector2 direction)
    {
        this.distanceslashDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void shoot()

    {
        sprite.flipX = (distanceslash.x < 0);
        rb2d.velocity = distanceslashDirection * distanceslashSpeed;
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject)
    }
}
*/