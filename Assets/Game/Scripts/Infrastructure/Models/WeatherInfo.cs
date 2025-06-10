namespace Game.Infrastructure
{
    public readonly struct WeatherInfo
    {
        public readonly string IconUrl;
        public readonly int Fahrenheit;

        public WeatherInfo(string icon, int f)
        {
            IconUrl = icon;
            Fahrenheit = f;
        }
    }
}