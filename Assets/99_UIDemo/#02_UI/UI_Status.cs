using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [SerializeField] Image TimeImage;
    [SerializeField] Image HPImage;
    [SerializeField] Image HungryImage;
    [SerializeField] Image ThirstyImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeImage.fillAmount -= Time.deltaTime / 10;

        if(TimeImage.fillAmount <= 0 )
        {
            TimeImage.fillAmount = 1;
        }


        HungryImage.fillAmount  -= Time.deltaTime / 10;
        ThirstyImage.fillAmount -= Time.deltaTime / 10;

    }

    public void AddHungry()
    {
        HungryImage.fillAmount += 0.2f;
    }
    public void AddThirsty()
    {
        ThirstyImage.fillAmount += 0.2f;
    }
}
