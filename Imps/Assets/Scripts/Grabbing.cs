using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Grabbing : MonoBehaviour
{
    public GameObject Player;
    public Transform ShootingPlace;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Imp" && Player.GetComponent<Player>().IsHoldingButton && Player.GetComponent<Player>().Imp == null)
        {
            other.GetComponent<ImpMovement>().IsBeingHeld = true;  
            Player.GetComponent<Player>().IsHoldingImp = true;
            Player.GetComponent<Player>().Imp = other.gameObject;
            Destroy(other.gameObject.GetComponent<NavMeshAgent>()); 
            other.gameObject.transform.position = ShootingPlace.transform.position;
            other.transform.parent = Player.transform;
        }
    }
}
