using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PushBack : MonoBehaviour
{
    public Player Player;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Imp")
        {
            Vector3 dir = other.attachedRigidbody.ClosestPointOnBounds(transform.position) - transform.position;
            dir = -dir.normalized;

            // And finally we add force in the direction of dir and multiply it by force. 
            if (!other.transform.gameObject.GetComponent<ImpMovement>()._isStunned)
            {
                other.transform.position -= dir * Player.pushBackForce * Time.deltaTime;
                if (other.GetComponent<ImpMovement>().RollDice() == 1)
                {
                    other.GetComponent<ImpMovement>().ImpScreamAudio.Play();
                }
            }
        }
    }
}
