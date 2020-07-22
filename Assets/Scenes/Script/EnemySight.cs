using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float fov = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    public static Vector3 resetPos = Vector3.back;
    private GameObject player;
    private SphereCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        personalLastSighting = resetPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < fov * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        personalLastSighting = player.transform.position;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }
}