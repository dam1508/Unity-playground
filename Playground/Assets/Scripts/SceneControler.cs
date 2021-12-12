using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneControler : MonoBehaviour
{
    [SerializeField] int mainMenu = 0;
    [SerializeField] int gameplay = 1;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
    public void LoadGameplay()
    {
        SceneManager.LoadScene(gameplay);
    }
}
