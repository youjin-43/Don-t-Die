using TMPro;
using UnityEngine;

public class AchievementSlot : MonoBehaviour
{
    private TextMeshProUGUI _newText;
    private TextMeshProUGUI _codeText;
    private TextMeshProUGUI _contentText;

    private string _code;

    void Awake()
    {
        _newText     = transform.GetChild(0).GetComponent<TextMeshProUGUI>();    
        _codeText    = transform.GetChild(1).GetComponent<TextMeshProUGUI>();    
        _contentText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _newText.gameObject.SetActive(false);
    }

    public void SetNewSign()
    {
        _newText.gameObject.SetActive(true);
    }

    public void SetCode(string code)
    {
        _codeText.text = code;
        _code = code;
    }

    public void SetContent(string content)
    {
        _contentText.text = content;
    }

    public void RefreshAchievement(string code, string content)
    {
        _codeText   .text = code;
        _contentText.text = content;
    }
}
