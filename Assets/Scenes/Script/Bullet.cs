using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player >().GetDamage(power);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetDamage(power);
        }

        Destroy(this.gameObject);
    }
}

