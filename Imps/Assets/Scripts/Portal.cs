using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public float ImpAmount = 0;
    public int MinPercantageToWin = 50;
    public GameObject UiText;
    public GameObject UiImpAmountText;
    public AudioSource EnterPortalAudio;
    public AudioSource WinLevelAudio;
    public AudioSource LoseLevelAudio;
    public Image ProgressBar;
    public GameObject HP;
    [SerializeField]private float _timerBeforeNewScreen = 0;
    public int PlayerHealth;
   private ImpSpawner[] _impSpawners;

    private float _allImpsInALevel = 0, _levelTime, _minImpsToWin;
    private bool _endingSoundPlayed = false;

    private Text _textMeshPro, _impAmountText, _hpText;
    [SerializeField] private float _timer = 150;
    private GameObject[] _impsPresent;
    private PlayerProgress _playerProgress;

    private void Start()
    {
        _impSpawners = GameObject.FindObjectsOfType<ImpSpawner>();
        _textMeshPro = UiText.GetComponent<Text>();
        _playerProgress = GameObject.FindFirstObjectByType<PlayerProgress>();
        _levelTime = _timer;
        _impAmountText = UiImpAmountText.GetComponent<Text>();
        _hpText = HP.GetComponent<Text>();

        foreach (ImpSpawner spawner in _impSpawners)
        {
            _allImpsInALevel += spawner.amountOfImps;
        }

        _minImpsToWin = (_allImpsInALevel * Percent(MinPercantageToWin));
        PlayerHealth = (int)_minImpsToWin + 1;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            ImpAmount++;
            EnterPortalAudio.Play();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Imp" && !other.gameObject.GetComponent<ImpMovement>().IsBeingHeld)
        {
            ImpAmount++;
            EnterPortalAudio.Play();
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        transform.Rotate(30 * Time.deltaTime, 0, 0);
        _textMeshPro.text = $"{Mathf.Clamp(_timer, 0, _timer).ToString("F2")}";
        _impsPresent = GameObject.FindGameObjectsWithTag("Imp");
        _impAmountText.text = $"{ImpAmount}/{_minImpsToWin}";
        _hpText.text = $"{Mathf.Clamp(PlayerHealth, 0, PlayerHealth)}";

        if (_timer <= 0)
        {
            _timer = 0;
            WinLoseBehaviour();
        }

        if (_timer < Percent(80) * _levelTime)
        {
            if (_impsPresent.Length == 0)
            {
                _timer = 0;
                WinLoseBehaviour();
            }
        }

        if(PlayerHealth == 0) 
        {
            WinLoseBehaviour();
        }

        ProgressBarBehaviour();
        ActivateTimerColorChange();
    }

    private void ActivateTimerColorChange()
    {
        if (_timer <= Percent(40) * _levelTime)
        {
            UiText.GetComponent<UIColorChange>().ChangeUIColor();
        }
    }

    private float Percent(float number)
    {
        float i = number / 100;
        return i;
    }
    private void WinLoseBehaviour()
    {
        if (ImpAmount < _minImpsToWin) //lose
        {
            if (!_endingSoundPlayed)
            {
                LoseLevelAudio.Play();
                _endingSoundPlayed = true;
            }

            WaitBeforeNextScreen(SceneManager.GetActiveScene().buildIndex, LoseLevelAudio.clip.length);
        }
        else if (ImpAmount >= _minImpsToWin) //win
        {
            _playerProgress.Level1Completed = true;

            if (!_endingSoundPlayed)
            {
                WinLevelAudio.Play();
                _endingSoundPlayed = true;
            }
           
            WaitBeforeNextScreen(4, WinLevelAudio.clip.length);
        }
    }

    private void WaitBeforeNextScreen(int sceneIndex, float audioClipLength)
    {
        _timerBeforeNewScreen += Time.deltaTime;

        if (_timerBeforeNewScreen > audioClipLength)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void ProgressBarBehaviour()
    {
        ProgressBar.fillAmount = ImpAmount / _allImpsInALevel;
    }
  
}
