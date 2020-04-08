using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region vars
    public static GameManager instance = null;
    #endregion

    #region unity_funcs
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region transitions
    public void StartGame()
    {
        SceneManager.LoadScene("PlayTestScene1");
    }
    
    #endregion
}
