using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;

    void Awake()
    {
        _achievementUI.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && _achievementUI.gameObject.activeSelf == true)
        {
            AchievementManager.Instance.ToggleAchievementUI();
        }
    }

    public void StartButton()
    {
        Time.timeScale = 1;
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
