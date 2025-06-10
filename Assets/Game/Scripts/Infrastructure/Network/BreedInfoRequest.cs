using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Game.Infrastructure
{
    public readonly struct BreedInfoRequest : IRequest<BreedInfo>
    {
        private readonly string _id;
        private const string UrlFormat = "https://dogapi.dog/api/v2/breeds/{0}";

        public BreedInfoRequest(string id)
        {
            _id = id;
        }

        public async UniTask<BreedInfo> ExecuteAsync(CancellationToken ct)
        {
            string url = string.Format(UrlFormat, _id);

            using var req = UnityWebRequest.Get(url);
            await req.SendWebRequest().ToUniTask(cancellationToken: ct);

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception(req.error);

            var json = req.downloadHandler.text;
            var root = JObject.Parse(json);
            var attr = root["data"]?["attributes"];

            string name = (string?)attr?["name"] ?? "Unknown";
            string desc = (string?)attr?["description"] ?? "No description.";

            return new BreedInfo(name, desc);
        }
    }
}