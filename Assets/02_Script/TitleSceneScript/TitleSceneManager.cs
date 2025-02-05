using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _achievementUI;
    [SerializeField] SettingUI SettingUI;
    [SerializeField] FadeInOut fadeInout; // ?????? ?? 

    void Awake()
    {
        _achievementUI.SetActive(true);
        SettingUI.gameObject.SetActive(true);

        _loading = _loadingText.text;
    }

    [SerializeField] RectTransform _loadingImage;

    public TextMeshProUGUI _loadingText;
    private string _loading;
    private string _dots = "";

    private UnityEngine.AsyncOperation asyncOperation;
    private float _elapsedTime;
    private float _elapsedTimeForText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_achievementUI.gameObject.activeSelf == true)
            {
                AchievementManager.Instance.ToggleAchievementUI();
            }
            if (SettingUI.gameObject.activeSelf == true)
            {
                SettingUI.ToggleSettingUI();
            }
        }

        if (asyncOperation != null)
        {
            fadeInout.FadeIn();
            _loadingImage.gameObject.SetActive(true);

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 5f)
            {
                _elapsedTime = 0;
                asyncOperation.allowSceneActivation = true;
                asyncOperation = null;
            }


        }

        _elapsedTimeForText += Time.deltaTime;

        if (_elapsedTimeForText > 0.1f)
        {
            _elapsedTimeForText = 0;
            _dots += ".";

            if (_dots.Length > 5)
            {
                _dots = "";

            }
            _loadingText.text = _loading + _dots;
        }
    }

    private void Start()
    {
        SoundManager.Instance.Play(AudioType.BGM, AudioClipName.BGM, 0.2f);
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        SoundManager.Instance.FadeVolume(AudioType.BGM, 0.1f);
        //SceneManager.LoadSceneAsync("GameScene");
        fadeInout.FadeOut();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
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

    private IEnumerator LoadSceneCoroutine(string SceneName)
    {
        yield return new WaitForSeconds(0.4f);

        asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
    }

    private IEnumerator Wait(float amount)
    {
        yield return new WaitForSeconds(amount);
    }
}
