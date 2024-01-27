using KillingJoke.Core.Utils;
using UnityEngine;

namespace KillingJoke.Core
{
    public class JokerFactory : Factory<Joker, Joker.Attributes>
    {
        [SerializeField] public GameObject[] JokersPool;
    }
}