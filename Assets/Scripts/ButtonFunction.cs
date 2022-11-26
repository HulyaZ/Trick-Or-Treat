using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
   public void Restart()
    {
        SceneManager.LoadSceneAsync(0);
        playerController.gameOverText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
