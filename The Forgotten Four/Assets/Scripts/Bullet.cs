using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public bool movesOnX, movesOnY;
    public bool encounteredThePlayer;
    public float xDirection, yDirection;
    public float speed;
    public Transform bulletFather;

    void Start()
    {
        DestroyBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if (!encounteredThePlayer)
        {
            if (movesOnX & !movesOnY)
            {
                transform.position += transform.up * (speed * xDirection) * Time.deltaTime;
            }

            if (!movesOnX & movesOnY)
            {
                transform.position += transform.right * (speed * yDirection) * Time.deltaTime;
            }

            if (movesOnX & movesOnY)
            {
                transform.position += (transform.right * (speed * xDirection) * Time.deltaTime) + (transform.up * (speed * yDirection) * Time.deltaTime);
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
