using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


    public class StartScreenUI : MonoBehaviour
    {   
    public void StartGameButton_Clicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private UIDocument _document;
   
    // Start is called before the first frame update
    void Awake()
    {
        _document = GetComponent<UIDocument>();
        Button StartGameButton = _document.rootVisualElement.Q<Button>("StartGameButton");
        Button QuitGameButton = _document.rootVisualElement.Q<Button>("ExitGameButton");

        StartGameButton.clicked += StartGameButton_clicked;
        QuitGameButton.clicked += QuitGameButton_clicked;
    }
    
    private void QuitGameButton_clicked()
    {
        Application.Quit();
    }

    private void StartGameButton_clicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   
}
