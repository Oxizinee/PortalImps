using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ImpMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject[] _escapes;
    public Material _ghostMat, _deafultMat;
    public MeshRenderer Renderer;
    public bool _isGrounded, _isStunned, IsBeingHeld;
    public GameObject WalkingParticles;
    public GameObject StunParticles;
    private NavMeshAgent _agent;

    public AudioSource[] ImpSounds;
    public AudioSource ImpScreamAudio;
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        _deafultMat = Renderer.sharedMaterial;
        _agent = GetComponent<NavMeshAgent>();
        _escapes = GameObject.FindGameObjectsWithTag("Escape");
        StunParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrounded && _agent != null)
        {
            if (_isStunned && !IsBeingHeld)
            {
                _agent.isStopped = true;
                WalkingParticles.SetActive(false);
                StunParticles.SetActive(true);
            }
            else if (!_isStunned && !IsBeingHeld)
            {
                _agent.isStopped = false;
                WalkingParticles.SetActive(true);
                StunParticles.SetActive(false);
                FindClosestEscape();
            }
        }

        if (!IsBeingHeld && GetComponent<NavMeshAgent>() == null)
        {
            _agent = gameObject.AddComponent<NavMeshAgent>();
        }

        if (!isGrounded())
        {
            transform.position -= Vector3.up * 2 * Time.deltaTime;
        }
    }

    private void FindClosestEscape()
    {
        if (_isStunned) return;
        if (IsBeingHeld) return;

        Transform closestExit = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject obj in _escapes)
        {
            if (obj == null)
            {
                continue;
            }

            float distance = Vector3.Distance(obj.transform.position, transform.position);

            // Update the closest object if the current object is closer
            if (distance < shortestDistance)
            {
                closestExit = obj.transform;
                shortestDistance = distance;
            }
        }

        if(closestExit != null) 
        {
            _agent.SetDestination(closestExit.position);
        }
        //ChooseTarget();
    }
    public void ChooseTarget()
    {
        float closestTargetDistance = float.MaxValue;
        NavMeshPath Path = null;
        NavMeshPath ShortestPath = null;

        for (int i = 0; i < _escapes.Length; i++)
        {
            if (_escapes[i] == null)
            {
                continue;
            }
            Path = new NavMeshPath();

            NavMesh.SamplePosition(transform.position, out NavMeshHit hitA, 10f, NavMesh.AllAreas);
            NavMesh.SamplePosition(_escapes[i].transform.position, out NavMeshHit hitB, 10f, NavMesh.AllAreas);

            if (NavMesh.CalculatePath(hitA.position, hitB.position, _agent.areaMask, Path))
            {
                //different behaviour for path avaiability 
                //switch (Path.status)
                //{
                //    case NavMeshPathStatus.PathComplete:
                //       // Debug.Log($"{agent.name} will be able to reach {target.name}.");
                //        break;
                //    case NavMeshPathStatus.PathPartial:

                //        Debug.Log($"will only be able to move partway");
                //        break;
                //    default:
                //        _agent.Move(GameObject.FindFirstObjectByType<Player>().transform.position * Time.deltaTime);
                //        Debug.Log($"will not be able to move partway"); 
                //        break;
                //}

                float distance = Vector3.Distance(transform.position, Path.corners[0]);

                for (int j = 1; j < Path.corners.Length; j++)
                {
                    distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    ShortestPath = Path;
                }
            }
        }

        if (ShortestPath != null)
        {
            _agent.SetPath(ShortestPath);
        }
    }

    public void Stun(float stunDuration)
    {
        if(IsBeingHeld) return; 

        _isStunned = true;
        StartCoroutine(StunDuration(stunDuration)); 
    }

    public IEnumerator StunDuration(float stunDuration)
    {
        if (_isStunned)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(stunDuration);
           _isStunned = false;
        }
        yield return null;
    }

    public bool isGrounded()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.8f))
        {
            _isGrounded = true;
            return true;
        }
        else
        {
            _isGrounded = false;
            return false;
        }
    }

    public int RollDice()
    {
        return Random.Range(0, 2);
    }
}
