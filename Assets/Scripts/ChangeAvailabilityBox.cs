using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeAvailabilityBox : MonoBehaviour
{
    public Image boxImageComp;
    public float cost;
    public MeterManager meterManager;
    public Sprite spriteNormal;
    public Sprite spriteAvailable;
    void Start()
    {
        boxImageComp = GetComponent<Image>();
    }
    
    void Update()
    {
        if (meterManager.currentMeterValue >= cost)
        {
            boxImageComp.sprite = spriteAvailable;
        }
        else
        {
            boxImageComp.sprite = spriteNormal;
        }
    }
}
