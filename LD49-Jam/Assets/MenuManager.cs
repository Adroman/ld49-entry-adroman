using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject TitleUI;
    public GameObject TutorialUI;
    public GameObject DifficultyUI;
    
    public int GameScene = 1;

    public AudioLoopManager AudioLoopManager;
    public DifficultyConfigurator SelectedDifficulty;
    
    public void GoToDifficulty()
    {
        TitleUI.SetActive(false);
        TutorialUI.SetActive(false);
        DifficultyUI.SetActive(true);
    }

    public void GoToTitle()
    {
        TitleUI.SetActive(true);
        TutorialUI.SetActive(false);
        DifficultyUI.SetActive(false);
    }

    public void GoToTutorial()
    {
        TitleUI.SetActive(false);
        TutorialUI.SetActive(true);
        DifficultyUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectDifficulty(DifficultyConfiguration difficultyConfiguration)
    {
        SelectedDifficulty.SelectedDifficulty = difficultyConfiguration;
    }
    
    public void StartGame()
    {
        AudioLoopManager.StopMusic();
        SceneManager.LoadScene(GameScene);
    }
}
