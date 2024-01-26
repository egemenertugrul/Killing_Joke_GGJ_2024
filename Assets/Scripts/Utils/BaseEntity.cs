using UnityEngine;

namespace KillingJoke.Core
{
    public abstract class BaseEntity<T> : MonoBehaviour where T : BaseAttributes
    {
        public abstract void Initialize(T attributes);

        protected bool isInitialized = false;
    }
}