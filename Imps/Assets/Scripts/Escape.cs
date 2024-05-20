using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escape : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _escapeSound;
    private LevelManager _levelManager;
    public CinemachineImpulseSource ImpulseSource;
    public GameObject Heart;
    public Color color;

    private Image _hpUI; 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            _escapeSound.Play();
            Destroy(other.gameObject);
            ImpulseSource.GenerateImpulse();
            _levelManager.PlayerHealth--;
            Heart.GetComponent<ScalingUI>().ShouldScaleAllTime = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            Heart.GetComponent<ScalingUI>().ShouldScaleAllTime = false;
        }
    }

    private IEnumerator ChangeColor()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            Color.Lerp(_hpUI.color, Color.black, 0.1f * Time.deltaTime);
        }

        yield return null;
    }
    private void Start()
    {
        _escapeSound = GetComponent<AudioSource>();
        _levelManager = GameObject.FindFirstObjectByType<LevelManager>();
        _hpUI = Heart.GetComponent<Image>();

        color = _hpUI.color.linear;
    }
}
