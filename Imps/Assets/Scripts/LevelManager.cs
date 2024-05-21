using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float ImpAmount = 0;
    public int MinPercantageToWin = 50;
    public GameObject UiText;
    public GameObject UiImpAmountText;
    public AudioSource WinLevelAudio;
    public AudioSource LoseLevelAudio;
    public Image ProgressBar;
    public GameObject HP;
    [SerializeField]private float _timerBeforeNewScreen = 0;
    public int PlayerHealth;
    public AudioSource LevelMusic;
    [SerializeField]private ImpSpawner[] _impSpawners;

    private float _allImpsInALevel = 0, _levelTime, _minImpsToWin;
    private bool _endingSoundPlayed = false;

    private Text _textMeshPro, _impAmountText, _hpText;
    public float LevelTimer = 150;
    private GameObject[] _impsPresent;
    private PlayerProgress _playerProgress;
    void Start()
    {
        _textMeshPro = UiText.GetComponent<Text>();
        _playerProgress = GameObject.FindFirstObjectByType<PlayerProgress>();
        _levelTime = LevelTimer;
        _impAmountText = UiImpAmountText.GetComponent<Text>();
        _hpText = HP.GetComponent<Text>();

        foreach (ImpSpawner spawner in _impSpawners)
        {
            _allImpsInALevel += spawner.amountOfImps;
        }

        _minImpsToWin = (_allImpsInALevel * Percent(MinPercantageToWin));
        PlayerHealth = (int)_minImpsToWin + 1;
    }

    // Update is called once per frame
    void Update()
    {
        LevelTimer -= Time.deltaTime;
        transform.Rotate(30 * Time.deltaTime, 0, 0);
        _textMeshPro.text = $"{Mathf.Clamp(LevelTimer, 0, LevelTimer).ToString("F2")}";
        _impsPresent = GameObject.FindGameObjectsWithTag("Imp");
        _impAmountText.text = $"{ImpAmount}/{_minImpsToWin}";
        _hpText.text = $"{Mathf.Clamp(PlayerHealth, 0, PlayerHealth)}";

        if (LevelTimer <= 0)
        {
            LevelTimer = 0;
            WinLoseBehaviour();
        }

        if (LevelTimer < Percent(80) * _levelTime)
        {
            if (_impsPresent.Length == 0)
            {
                LevelTimer = 0;
                WinLoseBehaviour();
            }
        }

        if (PlayerHealth <= 0)
        {
            WinLoseBehaviour();
        }

        ProgressBarBehaviour();
        ActivateTimerColorChange();
    }
    private void ActivateTimerColorChange()
    {
        if (LevelTimer <= Percent(40) * _levelTime)
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
            LevelMusic.Stop();

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

            LevelMusic.Stop();

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
