using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public Text damageText;
    public float lifeTime = 1;
    public float moveSpeed = 1;
    public float placementeJitter = .5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementeJitter, placementeJitter), Random.Range(-placementeJitter, placementeJitter), 0);

    }

    public void MissDamage()
    {
        damageText.text = "MISS";
        transform.position += new Vector3(Random.Range(-placementeJitter, placementeJitter), Random.Range(-placementeJitter, placementeJitter), 0);
    }
}
