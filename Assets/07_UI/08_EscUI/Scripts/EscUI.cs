using UnityEngine;
using UnityEngine.SceneManagement;

public class EscUI : MonoBehaviour
{
    public void ReturnToTitle()
    {
        Time.timeScale = 1;

        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void Setting()
    {
        UIManager.Instance.OpenSetting();
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
