namespace Game.Infrastructure
{
    public readonly struct BreedInfo
    {
        public readonly string Name;
        public readonly string Description;

        public BreedInfo(string name, string desc)
        {
            Name = name; 
            Description = desc;
        }
    }
}