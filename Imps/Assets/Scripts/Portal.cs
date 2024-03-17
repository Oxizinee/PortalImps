using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public int ImpAmount = 0;
    public int MinImps = 10;
    public GameObject UiText;
    public GameObject ImpText;
    private Text _textMeshPro;
    private Text _textMeshPro2;
    [SerializeField] private float _timer = 150;
    private void Start()
    {
        _textMeshPro = UiText.GetComponent<Text>();
        _textMeshPro2 = ImpText.GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            ImpAmount++;
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        transform.Rotate(30 * Time.deltaTime,0, 0);
        _textMeshPro.text = $"{_timer.ToString("F2")}";
        _textMeshPro2.text = $"{ImpAmount.ToString()}/{MinImps.ToString()}";

        if (_timer <= 0)
        {
            _timer = 0;
            if(ImpAmount < MinImps)
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
