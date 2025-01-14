using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public  static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public UI_Craft ui;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnOff()
    {
        if(ui.gameObject.activeSelf == false)
        {
            ui.gameObject.SetActive(true);
        }
        else
        {
            ui.gameObject.SetActive(false);
        }
    }
}
