using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ImpMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Player;

    private CharacterController _characterController;
    private NavMeshAgent _agent;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(Player.transform.position);

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.04f))
        {
        }
        else
        {
            transform.position -= (Vector3.up * 2f) * Time.deltaTime;
        }
    }
}
