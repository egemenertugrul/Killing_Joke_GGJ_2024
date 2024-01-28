using UnityEngine;

namespace KillingJoke.Core
{
    public partial class Joker
    {
        public class Attributes : BaseAttributes
        {
            public static int Count = 0;
            public int ID;
            public Color color;
            public bool isMale = true;
            public Attributes(int id, Color color)
            {
                this.ID = id;
                this.color = color;
            }

            public static Attributes GetRandomAttributes()
            {
                return new Attributes(Count++, Random.ColorHSV(0, 1, 1, 1));
            }
        }
    }
}