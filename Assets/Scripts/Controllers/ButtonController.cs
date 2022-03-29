using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip click, over;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(click != null)
            audioSource.PlayOneShot(click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(over != null)
            audioSource.PlayOneShot(over);
    }

    public void OnPointerExit(PointerEventData eventData) { }
    
}
