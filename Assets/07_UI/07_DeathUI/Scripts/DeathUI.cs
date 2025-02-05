using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    private TextMeshProUGUI _reasonToDeath;

    void Awake()
    {
        _reasonToDeath = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetContentText(string reason)
    {
        _reasonToDeath.text = reason;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }
}
