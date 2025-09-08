using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ScratchDatabaseSO", menuName = "Scriptable Objects/Scratch Database SO")]
public class ScratchDatabaseSO : SerializedScriptableObject
{
    [PropertyOrder(-1)]
    [SerializeField]
    public Dictionary<ArabLetter, ScratchDataSO> scratchDataDict = new Dictionary<ArabLetter, ScratchDataSO>();

    // ------------------------------------------------------------------------------------------------------------

    [TitleGroup("Helper")]
    [Tooltip("Path should be relative to the Resources folder, e.g., 'Data Gosok'")]
    public string path = "Data Gosok";

    [TitleGroup("Helper")]
    [ShowInInspector, ReadOnly]
    [NonSerialized]
    private List<ScratchDataSO> _scratchDataList;

    [Button, HorizontalGroup("Helper/Buttons")]
    public void Init()
    {
        scratchDataDict.Clear();

        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            if (!scratchDataDict.ContainsKey(letter))
            {
                scratchDataDict.Add(letter, null);
            }
        }

        Debug.Log("Initialized dictionary with all ArabLetter keys.");
    }

    [Button, HorizontalGroup("Helper/Buttons")]
    public void Refresh()
    {
        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            if (!scratchDataDict.ContainsKey(letter))
            {
                scratchDataDict.Add(letter, null);
            }
        }

        Debug.Log("Refreshed dictionary to include all ArabLetter keys.");
    }

    [TitleGroup("Helper")]
    [Button]
    public void GetFromFolder()
    {
        scratchDataDict.Clear();
        _scratchDataList = new List<ScratchDataSO>(Resources.LoadAll<ScratchDataSO>(path));

        Debug.Log($"Found {_scratchDataList.Count} ScratchDataSO assets in Resources/{path}");

        // Log loaded data
        foreach (var data in _scratchDataList)
        {
            Debug.Log($"Loaded: {data.name} - Letter: {data.letter}");
        }

        foreach (ArabLetter letter in System.Enum.GetValues(typeof(ArabLetter)))
        {
            var matchedData = _scratchDataList.Find(x => x.letter == letter);

            if (matchedData != null)
            {
                scratchDataDict[letter] = matchedData;
                Debug.Log($"Mapped {letter} to {matchedData.name}");
            }
            else
            {
                scratchDataDict[letter] = null;
                Debug.LogWarning($"No ScratchDataSO found for letter: {letter}");
            }
        }

        Debug.Log("Finished mapping scratch data.");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    [Button]
    public void EnableReadWrite()
    {
        foreach (var data in scratchDataDict)
        {
            if (data.Value == null)
                continue;

            data.Value.EnableReadWrite();
        }
    }
#endif
}
