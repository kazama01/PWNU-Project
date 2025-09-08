using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "HarokatSeekerDatabaseSO", menuName = "Scriptable Objects/Harokat Seeker Database SO")]
public class HarokatSeekerDatabaseSO : SerializedScriptableObject
{
    [PropertyOrder(-1)]
    [SerializeField]
    public Dictionary<ArabLetter, HijaiyahAudioDataSO> harakatDataDict = new Dictionary<ArabLetter, HijaiyahAudioDataSO>();

    // ------------------------------------------------------------------------------------------------------------

    [TitleGroup("Helper")]
    [Tooltip("Path should be relative to the Resources folder, e.g., 'Data Gosok'")]
    public string path = "Data Gosok";

    [TitleGroup("Helper")]
    [ShowInInspector, ReadOnly]
    [NonSerialized]
    private List<HijaiyahAudioDataSO> _hijaiyahDataList;

    [Button, HorizontalGroup("Helper/Buttons")]
    public void Init()
    {
        harakatDataDict.Clear();

        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            if (!harakatDataDict.ContainsKey(letter))
            {
                harakatDataDict.Add(letter, null);
            }
        }

        Debug.Log("Initialized dictionary with all ArabLetter keys.");
    }

    [Button, HorizontalGroup("Helper/Buttons")]
    public void Refresh()
    {
        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            if (!harakatDataDict.ContainsKey(letter))
            {
                harakatDataDict.Add(letter, null);
            }
        }

        Debug.Log("Refreshed dictionary to include all ArabLetter keys.");
    }

    [TitleGroup("Helper")]
    [Button]
    public void GetFromFolder()
    {
        harakatDataDict.Clear();
        _hijaiyahDataList = new List<HijaiyahAudioDataSO>(Resources.LoadAll<HijaiyahAudioDataSO>(path));

        Debug.Log($"Found {_hijaiyahDataList.Count} HijaiyahAudioDataSO assets in Resources/{path}");

        // Log loaded data
        foreach (var data in _hijaiyahDataList)
        {
            Debug.Log($"Loaded: {data.name} - Letter: {data.letter}");
        }

        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            var matchedData = _hijaiyahDataList.Find(x => x.letter == letter);

            if (matchedData != null)
            {
                harakatDataDict[letter] = matchedData;
                Debug.Log($"Mapped {letter} to {matchedData.name}");
            }
            else
            {
                harakatDataDict[letter] = null;
                Debug.LogWarning($"No ScratchDataSO found for letter: {letter}");
            }
        }

        Debug.Log("Finished mapping scratch data.");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
