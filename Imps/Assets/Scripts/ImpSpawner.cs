using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Imp;
    public int amountOfImps =20;
    void Start()
    {
        StartCoroutine(SpawnImp());
    }
    private IEnumerator SpawnImp()
    {
        for (int i = 0; i < amountOfImps; i++)
        {
            Instantiate(Imp, new Vector3(transform.position.x + Random.Range(-7,7), transform.position.y, transform.position.z + Random.Range(-7, 7)), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        yield return null;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
