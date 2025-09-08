using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HijaiyahHunterMenu
{
    public class LetterButtonUI : MonoBehaviour
    {
        public Image imageComp;
        [ShowInInspector]
        [ReadOnly]
        public ArabLetter Letter { get; private set; }

        public void Setup(Sprite sprite, ArabLetter letter)
        {
            imageComp.sprite = sprite;
            Letter = letter;
        }

        public void OnClick()
        {
            PlayerPrefs.SetString(GosokDevManager.SELECTED_GOSOK_LETTER_KEY, Letter.ToString());
        }
    }
}

