using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class MeterManager : MonoBehaviour
{
    public Image meterBarFill;
    public TextMeshProUGUI meterText;
    public int currentMeterValue = 0;
    public int maxMeterValue = 500;
    
    public AudioClip positiveSoundEffect;
    public AudioClip negativeSoundEffect;
    public AudioClip summoningSoundEffect;
    private AudioSource audioSource;
    
    public GameObject floatingTextPrefab;
    public Transform canvasTransform;

    public Transform summoningPoint;
    public GameObject basicCreaturePrefab;
    public GameObject mediumCreaturePrefab;
    public GameObject bigCreaturePrefab;

    public enum CreatureType { None, Basic, Medium, Big }
    public int basicCreatureCost = 10;
    public int mediumCreatureCost = 30;
    public int bigCreatureCost = 100;
    
    public event Action<int> OnCreatureSummoned;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMeterDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TrySummonCreature(CreatureType.Basic);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TrySummonCreature(CreatureType.Medium);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TrySummonCreature(CreatureType.Big);
        }
    }

    public void UpdateMeter(int amount, bool isSummon = false)
    {
        currentMeterValue += amount;
        if (currentMeterValue > maxMeterValue)
            currentMeterValue = maxMeterValue;
        else if (currentMeterValue < 0)
            currentMeterValue = 0;

        UpdateMeterDisplay();
        if (amount != 0 && !isSummon)
        {
            ShowFloatingText(amount);
            PlaySoundEffect(amount);
        }
    }
    
    private void UpdateMeterDisplay()
    {
        if (meterBarFill != null)
            meterBarFill.fillAmount = (float)currentMeterValue / maxMeterValue * 5f;
        
        if (meterText != null)
            meterText.text = $"{currentMeterValue}";
    }
    
    public void TrySummonCreature(CreatureType creature)
    {
        if (currentMeterValue >= GetCreatureCost(creature))
        {
            SummonCreature(creature);
        }
        else
        {
            Debug.Log($"Not enough magic to summon {creature} creature.");
        }
    }

    public void SummonCreature(CreatureType creature)
    {
        int cost = GetCreatureCost(creature);
        UpdateMeter(-cost, true);
        InstantiateCreature(creature);
        audioSource.pitch = UnityEngine.Random.Range(1.1f, 1.4f);
        audioSource.PlayOneShot(summoningSoundEffect);
        OnCreatureSummoned?.Invoke(cost);
    }
    
    private int GetCreatureCost(CreatureType creature)
    {
        switch (creature)
        {
            case CreatureType.Basic: return basicCreatureCost;
            case CreatureType.Medium: return mediumCreatureCost;
            case CreatureType.Big: return bigCreatureCost;
            default: return 0;
        }
    }
    
    private void InstantiateCreature(CreatureType creature)
    {
        GameObject prefab = GetCreaturePrefab(creature);
        if (prefab)
        {
            Instantiate(prefab, summoningPoint.position, Quaternion.identity);
        }
    }
    
    private GameObject GetCreaturePrefab(CreatureType creature)
    {
        switch (creature)
        {
            case CreatureType.Basic: return basicCreaturePrefab;
            case CreatureType.Medium: return mediumCreaturePrefab;
            case CreatureType.Big: return bigCreaturePrefab;
            default: return null;
        }
    }
    
    void ShowFloatingText(int amount)
    {
        GameObject textObj = Instantiate(floatingTextPrefab, canvasTransform);
        TextMeshProUGUI textMesh = textObj.GetComponent<TextMeshProUGUI>();
        if (amount > 0)
        {
            textMesh.text = $"+{amount}";
            textMesh.color = Color.green;
        }
        else
        {
            textMesh.text = $"{amount}"; // Amount is already negative
            textMesh.color = Color.red;
        }

        // Positioning right below the meter
        textObj.transform.position = meterBarFill.transform.position + new Vector3(0, -50f, 0);

        // Animation to move up and fade out
        Sequence sequence = DOTween.Sequence();
        sequence.Append(textObj.transform.DOMoveY(textObj.transform.position.y + 50f, 1f).SetEase(Ease.OutQuad));
        sequence.Join(textMesh.DOFade(0, 1f));
        sequence.OnComplete(() => Destroy(textObj));
    }
    
    void PlaySoundEffect(int amount)
    {
        audioSource.pitch = 1f;
        if (amount > 0)
        {
            int effectiveMeter = Mathf.Min(currentMeterValue, 100); 
            audioSource.pitch = 0.9f + 0.008f * effectiveMeter;
            audioSource.PlayOneShot(positiveSoundEffect);
        }

        else if (amount < 0)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(negativeSoundEffect);
        }
        audioSource.pitch = 1f;
    }
}