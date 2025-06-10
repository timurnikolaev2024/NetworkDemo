namespace Game.Infrastructure
{
    public readonly struct Breed
    {
        public readonly string Id;
        public readonly string Name;

        public Breed(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}