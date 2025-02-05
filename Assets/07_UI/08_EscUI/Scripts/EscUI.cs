using UnityEngine;
using UnityEngine.SceneManagement;

public class EscUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1;

        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void OpenSetting()
    {
        UIManager.Instance._settingUI.ToggleSettingUI();
    }

    public void Resume()
    {
        ToggleEscUI();
    }

    public void ToggleEscUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if(gameObject.activeSelf == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
