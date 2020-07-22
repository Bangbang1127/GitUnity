using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropeditem : MonoBehaviour
{

    public GameObject WeaponPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision .gameObject .tag =="Player")
        {
            Player.GetInstance().AddWeapon(WeaponPrefab);
            Destroy(this.gameObject);
        }
    }

}
