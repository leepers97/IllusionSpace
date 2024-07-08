using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport_HCH : MonoBehaviour
{
    public Transform player;
    public Transform reciever;

    BoxCollider col;

    bool isPlayerOverlapping = false;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
            
            if(dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = reciever.position + positionOffset;

                isPlayerOverlapping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOverlapping = false;
        }
    }
}
