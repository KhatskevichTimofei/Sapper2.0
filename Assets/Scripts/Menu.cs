using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class Menu : MonoBehaviour
{
    public static Difficulty Difficulty;
    private static int _numberOfAttempts;
    [SerializeField] private Text _healthValueTextInHandle;
    [SerializeField] private GameObject _difficultyImageBackground;

    public static int NumberOfAttempts => _numberOfAttempts;

    public void HealthCountChanger(Slider _healthValueSlider)
    {
        _healthValueTextInHandle.text = _healthValueSlider.value.ToString();
        _numberOfAttempts = Convert.ToInt32(_healthValueSlider.value);
    }

    public void StartGame(string gameScene)
    {
        if (_numberOfAttempts == 0)
        {
            _numberOfAttempts = 1;
        }

        SceneManager.LoadScene(gameScene);
    }
    public void DifficultySelection(int difficultyNumber)
    {
        if (difficultyNumber == 0)
        {
            Difficulty = Difficulty.Easy;
        }
        else if (difficultyNumber == 1)
        {
            Difficulty = Difficulty.Normal;
        }
        else if (difficultyNumber == 2)
        {
            Difficulty = Difficulty.Hard;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PositionDifficultyButtonBackground(Button button)
    {
        if (!_difficultyImageBackground.activeSelf)
        {
            _difficultyImageBackground.SetActive(true);
        }
        _difficultyImageBackground.transform.position = button.transform.position;
    }
}
