using System;
using UnityEngine;

public static class GlobalUtility
{
    public static ArabLetter GetArabLetterFromString(string input)
    {
        if (Enum.TryParse<ArabLetter>(input, true, out ArabLetter letter))
        {
            Debug.Log($"Parsed enum: {letter}");
        }

        Debug.LogWarning($"'{input}' is not a valid ArabLetter");
        return letter;
    }
}