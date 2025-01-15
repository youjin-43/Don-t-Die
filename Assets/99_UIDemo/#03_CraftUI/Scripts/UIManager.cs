using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform  _UIParent;
    [SerializeField] GameObject _inventoryPrefab;
    [SerializeField] GameObject _craftPrefab;

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

        Instantiate(_inventoryPrefab, _UIParent);
        Instantiate(_craftPrefab,     _UIParent);
    }
}
