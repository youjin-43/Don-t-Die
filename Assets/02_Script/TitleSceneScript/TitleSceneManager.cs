using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;

    void Awake()
    {
        _achievementUI.SetActive(true);
    }

    private void Start()
    {
        SoundManager.Instance.Play(AudioType.BGM, AudioClipName.BGM, 0.2f);
    }

    public void StartButton()
    {
        SceneManager.LoadSceneAsync("GameScene");
        SoundManager.Instance.FadeVolume(AudioType.BGM, 0.1f);
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
