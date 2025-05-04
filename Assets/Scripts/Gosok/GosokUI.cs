using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GosokUI : MonoBehaviour
{
    public RawImage letterRawImageComp;
    public RawImage nameRawImageComp;
    public ScratchController scratcher;

    [Button]
    public void Setup(ScratchDataSO dataSO)
    {
        ScratchObjectAudio selectedObject = dataSO.GetRandomObject();

        letterRawImageComp.texture = dataSO.letterImage;
        nameRawImageComp.texture = selectedObject.nameImage;
        scratcher.Setup(selectedObject.objectImage);
    }
}
