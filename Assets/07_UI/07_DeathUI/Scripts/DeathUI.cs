using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    private TextMeshProUGUI _reasonToDeath;

    void Awake()
    {
        _reasonToDeath = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
}
