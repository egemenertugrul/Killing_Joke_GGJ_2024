using UnityEngine;

namespace KillingJoke.Core.Utils
{
    // Could not derive from CanvasGroup since its a sealed class.
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupController : MonoBehaviour
    {
        private CanvasGroup cg;
        private bool isVisible;

        public bool IsVisible { get => isVisible; }

        private bool isInteractable;
        public bool IsInteractable
        {
            get => isInteractable; 
            set
            {
                isInteractable = value;
                cg.interactable = isInteractable;
            }
        }

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
            isVisible = cg.alpha > 0;
            isInteractable = cg.interactable;
        }

        public void Show()
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            isVisible = true;
        }

        public void Hide()
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            isVisible = true;
        }
    }
}