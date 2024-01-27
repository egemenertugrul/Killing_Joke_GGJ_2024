using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KillingJoke.Core
{
    public class HmdRaycaster : MonoBehaviour
    {
        public UnityEvent<IHMDHighlightable> OnNewHighlight = new UnityEvent<IHMDHighlightable>();

        private const float MaxDistance = 100f;
        RaycastHit[] hits = null;
        private IHMDHighlightable lastHighlighted;

        public IHMDHighlightable LastHighlighted
        {
            get => lastHighlighted; set
            {
                lastHighlighted = value;
                OnNewHighlight.Invoke(lastHighlighted);
            }
        }

        void Start()
        {

        }

        public IHMDHighlightable GetHighlighted()
        {
            return LastHighlighted;
        }

        void FixedUpdate()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            hits = Physics.RaycastAll(ray, MaxDistance, ~0);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            for (int i = 0; i < hits.Length; i++)
            {
                IHMDHighlightable highlightable = hits[i].transform.GetComponent<IHMDHighlightable>();
                if (highlightable != null)
                {
                    LastHighlighted?.Unhighlight();
                    highlightable.Highlight();
                    LastHighlighted = highlightable;
                }
            }
            if(hits.Length == 0)
            {
                LastHighlighted?.Unhighlight();
                LastHighlighted = null;
            }
        }
    }
}