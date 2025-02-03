using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;

    void Awake()
    {
        _achievementUI.SetActive(true);
    }

    public void StartButton()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OptionButton()
    {

    }

    public void AchievementButton()
    {
        AchievementManager.Instance.ToggleAchievementUI();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
