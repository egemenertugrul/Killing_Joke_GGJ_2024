using UnityEngine;

namespace KillingJoke.Core
{
    public class Joker : BaseEntity<Joker.Attributes>, IHMDHighlightable
    {
        private Outline outline;

        private void Awake()
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 0;
        }

        public override void Initialize(Attributes attributes)
        {
            this.attributes = attributes;
        }

        private bool isAlive = true;
        private Attributes attributes;

        public bool IsAlive { get => isAlive; }

        public void Forgive()
        {
            if (!isAlive)
                return;
            Debug.Log($"Forgiven Joker {attributes.ID}");
            outline.OutlineColor = Color.green;
        }

        public void Kill()
        {
            Debug.Log($"Killed Joker {attributes.ID}");
            isAlive = false;

            outline.OutlineColor = Color.red;

            // TODO: Dummy
            //Destroy(GetComponentInChildren<MeshRenderer>());
        }

        public class Attributes : BaseAttributes
        {
            public static int Count = 0;
            public int ID;
            public Color color;

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

        public void Highlight()
        {
            this.outline.OutlineWidth = 4;

        }
        public void Unhighlight()
        {
            this.outline.OutlineWidth = 0;
        }
    }
}