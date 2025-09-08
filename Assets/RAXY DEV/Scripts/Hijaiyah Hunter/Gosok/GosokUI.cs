using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GosokUI : MonoBehaviour
{
    public RawImage letterRawImageComp;
    public RawImage nameRawImageComp;
    public ScratchController scratcher;

    [Button]
    public void Setup(ScratchDataSO dataSO, ScratchObjectAudio selectedObject)
    {
        letterRawImageComp.texture = dataSO.letterImage.texture;
        nameRawImageComp.texture = selectedObject.nameImage;

        scratcher.enabled = true;
        scratcher.Setup(selectedObject.objectImage);
        GosokDevManager.Instance.AudioSource.PlayOneShot(dataSO.letterAudio);
    }
}
