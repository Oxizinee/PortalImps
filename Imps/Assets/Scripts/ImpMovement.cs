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
    public bool IsInBounds = true;
    public float MaxSpeed = 4;
    public float BaseSpeed = 3.5f;

    [SerializeField]private bool doOnce = true;
   [SerializeField] private Vector3 _startingPos;
    public AudioSource[] ImpSounds;
    public AudioSource ImpScreamAudio;

   [SerializeField] private const float _exitDistance = 80; 
    void Start()
    {
        _startingPos = gameObject.transform.position;
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

        OutOfBoundsBehaviour();
        AdjustSpeedOnDistance();
        
    }

    private void AdjustSpeedOnDistance()
    {
        if (_isStunned) return;
        if (IsBeingHeld) return;

        if (_agent.remainingDistance > _exitDistance/2)
        {
            _agent.speed = MaxSpeed;
        }
        else
        {
            _agent.speed = BaseSpeed;
        }
    }
    private void OutOfBoundsBehaviour()
    {
        if (!IsBeingHeld && !IsInBounds)
        {
            _agent.Warp(_startingPos);
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
            NavMeshPath path;

            path = new NavMeshPath();

            NavMesh.SamplePosition(transform.position, out NavMeshHit hitA, 10f, NavMesh.AllAreas);
            NavMesh.SamplePosition(closestExit.position, out NavMeshHit hitB, 10f, NavMesh.AllAreas);

            if (NavMesh.CalculatePath(hitA.position, hitB.position, _agent.areaMask, path))
            {
                //different behaviour for path avaiability
                switch (path.status)
                    {
                        case NavMeshPathStatus.PathComplete:
                        _agent.destination = closestExit.position;
                        break;
                        case NavMeshPathStatus.PathPartial:
                        if (doOnce)
                        {
                            _agent.destination = GenerateRandomPos(closestExit.position);
                            doOnce = false;
                        }
                        break;
                    }
            }
           
        }

    }

    private Vector3 GenerateRandomPos(Vector3 StartPos)
    {
        Vector3 randomPoint = StartPos + Random.insideUnitSphere * 10;
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hitC, 1, NavMesh.AllAreas);
        return hitC.position;
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
