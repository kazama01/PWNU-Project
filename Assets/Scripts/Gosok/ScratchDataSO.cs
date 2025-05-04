using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScratchDataSO", menuName = "Scriptable Objects/Scratch Data SO")]
public class ScratchDataSO : ScriptableObject
{
    public ArabLetter letter;

    public Texture2D letterImage;

    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "objectName")]
    public List<ScratchObjectAudio> objects;

    public ScratchObjectAudio GetRandomObject()
    {
        if (objects == null || objects.Count == 0)
        {
            Debug.LogError("No objects available in the list.");
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, objects.Count);
        return objects[randomIndex];
    }
}

[Serializable]
public class ScratchObjectAudio
{
    public string objectName;

    public Texture2D nameImage;
    public Texture2D objectImage;
    public AudioClip audio;
}

public enum ArabLetter
{
    Alif,
    Ba,
    Ta,
    Tsa,
    Jim,
    Cha,
    Kho,
    Dal,
    Dzal,
    Ro,
    Za,
    Sin,
    Shin,
    Sho,
    Dho,
    Tho,
    Dhod,
    Ain,
    Ghoin,
    Fa,
    Qof,
    Kaf,
    Lam,
    Mim,
    Nun,
    Waw,
    Ha,
    LamAlif,
    Hamzah,
    Ya
}

