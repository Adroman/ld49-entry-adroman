using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject InGameUi;
    public GameObject PauseUi;
    public GameObject GameOverUi;

    public UiTowerSelector TowerSelector;
    public InstabilityManager InstabilityManager;
    public WaveManager WaveManager;
    
    public int MenuSceneId;
    public int InGameSceneId;

    private bool _isPaused;

    public void Awake()
    {
        Time.timeScale = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TowerSelector.SelectedTower != null)
            {
                TowerSelector.DeselectTower();
            }
            else if (!_isPaused)
            {
                PauseGame();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))   // Right mouse button
        {
            if (TowerSelector.SelectedTower != null)
            {
                TowerSelector.DeselectTower();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (WaveManager.NextWaveButton.gameObject.activeSelf)
            {
                WaveManager.StartWave();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _isPaused = true;
        InGameUi.SetActive(false);
        PauseUi.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        _isPaused = true;
        InGameUi.SetActive(false);
        PauseUi.SetActive(false);
        GameOverUi.SetActive(true);
    }

    public void UnpauseGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        InGameUi.SetActive(true);
        PauseUi.SetActive(false);
        InstabilityManager.GamePaused();
    }

    public void Restart()
    {
        SceneManager.LoadScene(InGameSceneId);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(MenuSceneId);
        Time.timeScale = 1;
    }
}
