using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;
    [SerializeField] SettingUI   SettingUI;

    void Awake()
    {
        _achievementUI.SetActive(true);
        SettingUI.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_achievementUI.gameObject.activeSelf == true)
            {
                AchievementManager.Instance.ToggleAchievementUI();
            }
            if(SettingUI.gameObject.activeSelf == true)
            {
                SettingUI.ToggleSettingUI();
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
    }

    public void OptionButton()
    {
        SettingUI.ToggleSettingUI();
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
