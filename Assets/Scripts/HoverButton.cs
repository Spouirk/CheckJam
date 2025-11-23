using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundType.SLAP);
    }

    public void PlaySoundOnClick()
    {
        SoundManager.PlaySound(SoundType.SWOOCH);
    }
}
