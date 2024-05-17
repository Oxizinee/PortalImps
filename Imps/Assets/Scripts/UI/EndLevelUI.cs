using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

    public class EndLevelUI : MonoBehaviour
    {
    private AudioSource _clickButtonSound;
    private UIDocument _document;
    private PlayerProgress _playerProgress;
   
    // Start is called before the first frame update
    void Awake()
    {
        _clickButtonSound = GetComponent<AudioSource>();
        _playerProgress = GameObject.FindFirstObjectByType<PlayerProgress>();

        _document = GetComponent<UIDocument>();
        Button LevelSelectionButton = _document.rootVisualElement.Q<Button>("LevelSelectionButton");
        Button ReplayLevelButton = _document.rootVisualElement.Q<Button>("ReplayLevel");
        Button QuitGameButton = _document.rootVisualElement.Q<Button>("ExitGameButton");

        LevelSelectionButton.clicked += LevelSelectionButton_clicked;
        ReplayLevelButton.clicked += ReplayLevel_clicked;
        QuitGameButton.clicked += QuitGameButton_clicked;
    }
    private void QuitGameButton_clicked()
    {
        _clickButtonSound.Play();

            Application.Quit();
    }
    private void ReplayLevel_clicked()
    {
        _clickButtonSound.Play();

            if(_playerProgress.Level1Completed)
            {
                SceneManager.LoadScene("Level_1");
            }
            else if (_playerProgress.Level2Completed)
            {
                SceneManager.LoadScene("Level_2");
            }
            else if (_playerProgress.Level3Completed)
            {
                SceneManager.LoadScene("Level_3");
            }
    }
    private void LevelSelectionButton_clicked()
    {
        _clickButtonSound.Play();

            SceneManager.LoadScene("LevelSelectionScreen");
    }

   
}
