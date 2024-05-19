using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectionUI : MonoBehaviour
{
    private UIDocument _document;
    private PlayerProgress _playerProgress;
    private Button _lockedLevel2Button;
    private Button _level1Button;
    private Button _level2Button;
    private AudioSource _clickButtonSound;

    void Awake()
    {
        _clickButtonSound = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();
        _level1Button = _document.rootVisualElement.Q<Button>("Level1Button");
        _level2Button = _document.rootVisualElement.Q<Button>("Level2Button");

        _lockedLevel2Button = _document.rootVisualElement.Q<Button>("Level2LockedButton");


        _level1Button.clicked += Level1Button_clicked;
        _level2Button.clicked += Level2Button_clicked;
    }


    private void Level2Button_clicked()
    {
        if (_playerProgress.Level1Completed)
        {
            _clickButtonSound.Play();
            SceneManager.LoadScene("Level_2");
        }
    }

    private void Level1Button_clicked()
    {
        _clickButtonSound.Play();
        SceneManager.LoadScene("Level_1");
    }

    private void Start()
    {
        _playerProgress = GameObject.FindObjectOfType<PlayerProgress>();

        if (!_playerProgress.Level1Completed)
        {
            _lockedLevel2Button.visible = true;
            _level2Button.visible = false;
        }
        else
        {
            _lockedLevel2Button.visible = false;
            _level2Button.visible = true;
        }
    }
}
