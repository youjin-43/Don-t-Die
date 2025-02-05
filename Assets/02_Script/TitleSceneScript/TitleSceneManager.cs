using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;
    [SerializeField] GameObject _settingUI;

    void Awake()
    {
        _achievementUI.SetActive(true);
        _settingUI.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_achievementUI.gameObject.activeSelf == true)
            {
                AchievementManager.Instance.ToggleAchievementUI();
            }
            if(_settingUI.gameObject.activeSelf == true)
            {
                SettingManager.Instance.ToggleSettingUI();
            }
        }
    }

    private void Start()
    {
        SoundManager.Instance.Play(AudioType.BGM, AudioClipName.BGM, 0.2f);
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("GameScene");
        SoundManager.Instance.FadeVolume(AudioType.BGM, 0.1f);
    }

    public void OptionButton()
    {
        SettingManager.Instance.ToggleSettingUI();
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
