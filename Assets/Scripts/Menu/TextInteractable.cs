using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Menu
{
    public class TextInteractable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color pressedColor;
        [SerializeField] internal UnityEvent methodEvent;
        [SerializeField] private float timeTransition = .5f;

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(ChangeColor(text.color, pressedColor, timeTransition));
            methodEvent.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(ChangeColor(text.color, hoverColor, timeTransition));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(ChangeColor(text.color, normalColor, timeTransition));
        }

        public void ForceState(bool hover)
        {
            if (hover)
            {
                StartCoroutine(ChangeColor(text.color, hoverColor, timeTransition));
            }
            else
            {
                StartCoroutine(ChangeColor(text.color, normalColor, timeTransition));
            }
        }

        /// <summary>
        /// Chnage from one color to another
        /// </summary>
        /// <param name="colorA">Initial Color</param>
        /// <param name="colorB">Final Color</param>
        /// <param name="time">Time of transition</param>
        private IEnumerator ChangeColor(Color colorA, Color colorB, float time)
        {
            Color tranColor = colorA;
            while (tranColor != colorB)
            {
                tranColor = Color.Lerp(tranColor, colorB, time);
                text.color = tranColor;
                yield return null;
            }
        }
    }
}
