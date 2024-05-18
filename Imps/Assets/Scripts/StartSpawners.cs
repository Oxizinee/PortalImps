using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawners : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawner1;
    public GameObject spawner2;

    private LevelManager _levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawner1.SetActive(true);
        }
    }
    void Start()
    {
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelManager.LevelTimer <= 100)
        {
            spawner2.SetActive(true);
        }
    }
}
