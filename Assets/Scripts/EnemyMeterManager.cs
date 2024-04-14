using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMeterManager : MonoBehaviour
{
    
    public enum Difficulty { Easy, Medium, Hard, Impossible }

    public Difficulty difficulty;
    public GameObject basicCreaturePrefab;
    public GameObject mediumCreaturePrefab;
    public GameObject bigCreaturePrefab;
    public Transform summoningPoint;

    private float meterRate;
    public float currentMeter = 0;
    public enum CreatureType { Basic, Medium, Big }
    public float[] summonCosts = { 20, 100, 250 }; 
    public Queue<CreatureType> summonQueue = new Queue<CreatureType>();

    private void Start()
    {
        difficulty = DifficultyManager.Instance.difficulty;
        InitializeDifficulty();
        InitializeSummonQueue();
        StartCoroutine(MeterIncreaseRoutine());
        StartCoroutine(SummonRoutine());
    }

    private void InitializeDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                meterRate = 2.5f;
                break;
            case Difficulty.Medium:
                meterRate = 5.5f;
                break;
            case Difficulty.Hard:
                meterRate = 7f;
                break;
            case Difficulty.Impossible:
                meterRate = 10f;
                break;
        }
    }

    private void InitializeSummonQueue()
    {
        // Initial predefined sequence
        summonQueue.Enqueue(CreatureType.Basic);
        summonQueue.Enqueue(CreatureType.Basic);
        summonQueue.Enqueue(CreatureType.Medium);
        summonQueue.Enqueue(CreatureType.Basic);
        summonQueue.Enqueue(CreatureType.Medium);
    }

    IEnumerator MeterIncreaseRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            currentMeter += meterRate * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SummonRoutine()
    {
        while (true)
        {
            if (summonQueue.Count == 0)
            {
                float rand = Random.value;
                if (rand < 0.5)
                    summonQueue.Enqueue(CreatureType.Basic);
                else if (rand < 0.8)
                    summonQueue.Enqueue(CreatureType.Medium);
                else
                    summonQueue.Enqueue(CreatureType.Big);
            }

            CreatureType nextCreature = summonQueue.Peek();
            if (currentMeter >= summonCosts[(int)nextCreature])
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 2f));
                summonQueue.Dequeue();
                currentMeter -= summonCosts[(int)nextCreature];
                InstantiateCreature(nextCreature);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void InstantiateCreature(CreatureType creature)
    {
        GameObject prefab = null;
        switch (creature)
        {
            case CreatureType.Basic:
                prefab = basicCreaturePrefab;
                break;
            case CreatureType.Medium:
                prefab = mediumCreaturePrefab;
                break;
            case CreatureType.Big:
                prefab = bigCreaturePrefab;
                break;
        }
        Instantiate(prefab, summoningPoint.position, Quaternion.identity);
    }
}
