namespace KillingJoke.Core
{
    public interface IHighlightable
    {
        public void Highlight();
        public void Unhighlight();
    }

    public interface IHMDHighlightable : IHighlightable
    {
    }
}