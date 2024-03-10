using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ImpMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Player;

    private GameObject[] _escapes;
    private float _distance;
    private int _closestEscape;
    private CharacterController _characterController;
    private NavMeshAgent _agent;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _escapes = GameObject.FindGameObjectsWithTag("Escape");
        _distance = Vector3.Distance(transform.position, _escapes[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEscape();

        Ground();
    }

    private void FindClosestEscape()
    {
        for (int i = 0; i < _escapes.Length; i++)
        {
            if (Vector3.Distance(transform.position, _escapes[i].transform.position) < _distance)
            {
                _distance = Vector3.Distance(transform.position, _escapes[i].transform.position);
                _closestEscape = i;
            }
        }

        _agent.SetDestination(_escapes[_closestEscape].transform.position);
    }

    private void Ground()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.04f))
        {
        }
        else
        {
            transform.position -= (Vector3.up * 2f) * Time.deltaTime;
        }
    }
}
