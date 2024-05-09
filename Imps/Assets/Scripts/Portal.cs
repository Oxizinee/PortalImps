using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public float ImpAmount = 0;
    public int MinPercantageToWin = 50;
    public GameObject UiText;
    public AudioSource EnterPortalAudio;
    public AudioSource WinLevelAudio;
    public Image ProgressBar;

    public float SecondsBeforeNewScreen = 10;
    
   private ImpSpawner[] _impSpawners;

    private float _allImpsInALevel = 0, _levelTime;
    private bool _winSoundPlayed = false;

    private Text _textMeshPro;
    [SerializeField] private float _timer = 150;
    private GameObject[] _impsPresent;
    private PlayerProgress _playerProgress;

    private void Start()
    {
        _impSpawners = GameObject.FindObjectsOfType<ImpSpawner>();
        _textMeshPro = UiText.GetComponent<Text>();
        _playerProgress = GameObject.FindFirstObjectByType<PlayerProgress>();
        _levelTime = _timer;

        foreach (ImpSpawner spawner in _impSpawners)
        {
            _allImpsInALevel += spawner.amountOfImps;
        }
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

    private void Update()
    {
        _timer -= Time.deltaTime;
        transform.Rotate(30 * Time.deltaTime,0, 0);
        _textMeshPro.text = $"{_timer.ToString("F2")}";
        _impsPresent = GameObject.FindGameObjectsWithTag("Imp");

        if (_timer <= 0)
        {
            _timer = 0;
            WinLoseBehaviour();
        }

        if (_timer < _levelTime * (80/100))
        {
            if (_impsPresent.Length == 0)
            {
                _timer = 0;
                WinLoseBehaviour();
            }
        }

        ProgressBarBehaviour();
    }

    private void WinLoseBehaviour()
    {
        if (ImpAmount < _allImpsInALevel * (MinPercantageToWin / 100)) //lose
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (ImpAmount >= _allImpsInALevel * (MinPercantageToWin/100)) //win
        {
            _playerProgress.Level1Completed = true;

            if (!_winSoundPlayed)
            {
                WinLevelAudio.Play();
                _winSoundPlayed = true;
            }
           
            WaitBeforeNextScreen();
            // SceneManager.LoadScene("LevelSelectionScreen");
        }
    }

    private void WaitBeforeNextScreen()
    {
        SecondsBeforeNewScreen -= Time.deltaTime;

        if (SecondsBeforeNewScreen < 0)
        {
            SceneManager.LoadScene("LevelSelectionScreen");
        }
    }

    private void ProgressBarBehaviour()
    {
        ProgressBar.fillAmount = ImpAmount / _allImpsInALevel;
    }
  
}
