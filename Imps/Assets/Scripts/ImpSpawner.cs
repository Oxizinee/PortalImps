using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Imp;
    public int amountOfImps = 20;
    public int secondsBetweenImps = 1;
    public int Delay = 0;
    void Start()
    {
        StartCoroutine(SpawnImp());
    }
    private IEnumerator SpawnImp()
    {
        yield return new WaitForSeconds(Delay);
        for (int i = 0; i < amountOfImps; i++)
        {
            Instantiate(Imp, new Vector3(transform.position.x + Random.Range(-7,7), transform.position.y, transform.position.z + Random.Range(-7, 7)), Quaternion.identity);
            yield return new WaitForSeconds(secondsBetweenImps);
        }
        yield return null;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
