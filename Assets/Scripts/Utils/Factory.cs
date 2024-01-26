
using UnityEngine;

namespace KillingJoke.Core.Utils
{
    /// <summary>
    /// Base class for a Factory
    /// </summary>
    /// <typeparam name="T">A class derived from BaseEntity</typeparam>
    /// <typeparam name="T2">A class derived from BaseAttributes</typeparam>
    public abstract class Factory<T, T2> : MonoBehaviour where T : BaseEntity<T2> where T2 : BaseAttributes
    {
        /// <summary>
        /// (Optional) The prefab to be instantiated. 
        /// If left unassigned, a new GameObject with specified <see cref="BaseEntity{T}"/> and <see cref="BaseAttributes"/> will be generated.
        /// </summary>
        [SerializeField] protected GameObject prefabToGenerate;

        private delegate T GenerateBehaviourDelegate(T2 attributes);
        GenerateBehaviourDelegate GenerateBehaviour = null;
        
        /// <summary>
        /// Generate entity of <typeparamref name="T"/> from prefab.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private T PrefabGenerateBehaviour(T2 attributes)
        {
            GameObject newGameObject = Instantiate(prefabToGenerate);
            T entityInPrefab = newGameObject.GetComponent<T>();
            if(entityInPrefab == null)
            {
                Debug.LogWarning($"The assigned prefab of {GetType().Name} is missing the corresponding entity: {typeof(T)}. Defaulting to generating a new GameObject.");
                Destroy(newGameObject);
                GenerateBehaviour = ScratchGenerateBehaviour;
                return GenerateBehaviour(attributes);
            }
            entityInPrefab.Initialize(attributes);
            return entityInPrefab;
        }

        /// <summary>
        /// Generate entity of <typeparamref name="T"/> from scratch.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private T ScratchGenerateBehaviour(T2 attributes)
        {
            T newEntity = new GameObject($"{typeof(T)}").AddComponent<T>();
            newEntity.Initialize(attributes);
            return newEntity;
        }

        /// <summary>
        /// Generator function of the <see cref="Factory{T, T2}"/>.
        /// </summary>
        /// <param name="attributes">An attributes class of <typeparamref name="T2"/></param>
        /// <returns></returns>
        public virtual T Generate(T2 attributes)
        {
            return GenerateBehaviour(attributes);
        }

        private void Awake()
        {
            if(prefabToGenerate == null)
            {
                Debug.LogWarning($"{GetType().Name} does not have a prefab assigned. A new GameObject of entity: {typeof(T)}, attributes: {typeof(T2)} will be generated.");
                GenerateBehaviour = ScratchGenerateBehaviour;
                return;
            }
            GenerateBehaviour = PrefabGenerateBehaviour;
        }

    }
}