using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public int ImpAmount = 0;
    public GameObject UiText;
    private Text _textMeshPro;
    private void Start()
    {
       _textMeshPro = UiText.GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Imp")
        {
            ImpAmount++;
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(30 * Time.deltaTime,0, 0);
        _textMeshPro.text = ImpAmount.ToString();
    }
}
