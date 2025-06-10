namespace Game.UI
{
    public interface ITabView
    {
        TabId Id { get; }
        void Hide();
        void Show();
    }
}