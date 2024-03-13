using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : MonoBehaviour
{
    public GameObject Player;

    private void OnTriggerStay(Collider other)
    {
        if (Player.GetComponent<Player>().CanStun && other.gameObject.tag == "Imp")
        {
            other.gameObject.GetComponent<ImpMovement>().Stun(Player.GetComponent<Player>().StunDuration);
        }
    }
}
