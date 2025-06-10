using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Game.Infrastructure
{
    public readonly struct BreedsRequest : IRequest<IReadOnlyList<Breed>>
    {
        const string Url = "https://dogapi.dog/api/v2/breeds?page[size]=10";
        
        public async UniTask<IReadOnlyList<Breed>> ExecuteAsync(CancellationToken ct)
        {
            using var req = UnityWebRequest.Get(Url);
            await req.SendWebRequest().ToUniTask(cancellationToken: ct);

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception(req.error);

            var json = req.downloadHandler.text;
            var root = JObject.Parse(json);
            var list = new List<Breed>(10);

            foreach (var item in root["data"]!)
            {
                var id   = (string?)item["id"] ?? string.Empty;
                var name = (string?)item["attributes"]?["name"] ?? "Unknown";
                list.Add(new Breed(id, name));
            }

            return list;
        }
    }
}