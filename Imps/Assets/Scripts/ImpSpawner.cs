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
    public float Angle = 30;
    public Light Light;
    public float TimeDelay = 0.5f;


    private bool _impsSpawning;
    private float _timer;
    private float _timerRed, _timerBlue;

    void Start()
    {
        StartCoroutine(SpawnImp());
    }
    private IEnumerator SpawnImp()
    {
        _impsSpawning = true;
        yield return new WaitForSeconds(Delay);
        for (int i = 0; i < amountOfImps; i++)
        {
            Instantiate(Imp, new Vector3(transform.position.x + Random.Range(-7,7), transform.position.y, transform.position.z + Random.Range(-7, 7)), Quaternion.identity);
            yield return new WaitForSeconds(secondsBetweenImps);
            if (i == amountOfImps - 1)
            {
                _impsSpawning = false;
            }
        }
        yield return null;  
    }

    // Update is called once per frame
    void Update()
    {
        if (_impsSpawning)
        {
            _timer += Time.deltaTime;

            if (_timer < TimeDelay *  4)
            {
                _timerRed += Time.deltaTime;
                Light.color = Color.Lerp(Color.blue, Color.red, _timerRed);
            }
            else
            {
                _timerBlue += Time.deltaTime;
                Light.color = Color.Lerp(Color.red, Color.blue, _timerBlue);
                if (_timer >  TimeDelay * 8)
                {
                    _timerRed = 0;
                    _timerBlue = 0;
                    _timer = 0;
                }
            }
        }
        else
        {
            Light.color = Color.white;
        }
    }
}
