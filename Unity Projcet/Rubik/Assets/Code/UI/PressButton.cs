using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Code.UI
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnPress;

        protected bool IsDown = false;

        protected void Update()
        {
            if (IsDown)
            {
                OnPress.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDown = false;
        }
    }
}
