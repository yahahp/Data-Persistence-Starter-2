using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public Button StartButton;
    public Button Quit;


    public InputField inputPlayerName;
    public string enteredName;
    public string savedName;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        savedName = null;
        

    }
   public void EnterButton()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
#if UNITY_EDITOR
       
        EditorApplication.ExitPlaymode();
#else
       Application.Quit();
#endif
    }
    public void SetPlayerName()
    {
        enteredName = inputPlayerName.text;
        if (enteredName != null)
        {
            savedName = enteredName;
        }
        Debug.Log(savedName);
    }
}       