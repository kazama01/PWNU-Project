using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Muslim Hero Database SO", menuName = "Scriptable Objects/Muslim Hero Database SO")]
public class MuslimHeroDatabaseSO : ScriptableObject
{
    public List<MuslimHeroDataSO> muslimHeroDatas;

    public MuslimHeroDataSO GetRandomHero()
    {
        if (muslimHeroDatas == null || muslimHeroDatas.Count == 0)
        {
            Debug.LogWarning("MuslimHeroDatas list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, muslimHeroDatas.Count);
        return muslimHeroDatas[randomIndex];
    }

    public List<MuslimHeroDataSO> GetRandomHeroes(int count, List<MuslimHeroDataSO> excludedHero = null)
    {
        if (muslimHeroDatas == null || muslimHeroDatas.Count == 0)
        {
            Debug.LogWarning("MuslimHeroDatas list is empty or null.");
            return new List<MuslimHeroDataSO>();
        }

        // Create a list of available heroes excluding the ones in the exclusion list
        List<MuslimHeroDataSO> availableHeroes = new List<MuslimHeroDataSO>(muslimHeroDatas);
        if (excludedHero != null && excludedHero.Count > 0)
        {
            availableHeroes.RemoveAll(hero => excludedHero.Contains(hero));
        }

        if (availableHeroes.Count == 0)
        {
            Debug.LogWarning("No heroes available after applying exclusions.");
            return new List<MuslimHeroDataSO>();
        }

        // Ensure we don't request more heroes than are available
        count = Mathf.Min(count, availableHeroes.Count);

        // Select random heroes
        List<MuslimHeroDataSO> randomHeroes = new List<MuslimHeroDataSO>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableHeroes.Count);
            randomHeroes.Add(availableHeroes[randomIndex]);
            availableHeroes.RemoveAt(randomIndex); // Remove to avoid duplicates
        }

        return randomHeroes;
    }
}
