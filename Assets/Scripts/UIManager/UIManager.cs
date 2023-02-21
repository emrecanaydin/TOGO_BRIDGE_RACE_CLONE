using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject gamePlayPanel;
    public GameObject winPanel;
    public GameObject lostPanel;

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
