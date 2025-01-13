using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Image itemImage;
    private bool  isActivated = false;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Activate()
    {
        isActivated     = true;
        itemImage.color = Color.green;
    }
    public void Deactivate()
    {
        isActivated = false;
        itemImage.color = Color.white;
    }
}
