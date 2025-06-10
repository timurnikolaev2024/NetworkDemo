using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Game.Infrastructure
{
    public readonly struct WeatherRequest : IRequest<WeatherInfo>
    {
        const string Url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public async UniTask<WeatherInfo> ExecuteAsync(CancellationToken ct)
        {
            using var req = UnityWebRequest.Get(Url);
            req.SetRequestHeader("User-Agent", "Unity-Weather-Client");

            await req.SendWebRequest().ToUniTask(cancellationToken: ct);

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception(req.error);

            var json = req.downloadHandler.text;
            var root = JObject.Parse(json);

            var today = root["properties"]?["periods"]?[0];
            var icon = (string?)today?["icon"] ?? "";
            var temp = (int?)today?["temperature"] ?? 0;

            return new WeatherInfo(icon, temp);
        }
    }
}