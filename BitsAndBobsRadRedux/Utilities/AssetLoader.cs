using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using static BitsAndBobsRadRedux.BBRR_Plugin;

namespace BitsAndBobsRadRedux
{
    internal class AssetLoader
    {
        internal static GameObject MeteorShower { get; private set; }
        internal static GameObject VolcanoSteam { get; private set; }
        internal static bool AssetsLoaded { get; private set; } = false;

        internal static IEnumerator LoadAssets()
        {
            LogDebug($"Loading assets");
            var bundlePath = Path.Combine(Path.GetDirectoryName(Instance.Info.Location), "Assets", "bbrr_assets");
            var assetBundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return assetBundleRequest;
            var assetBundle = assetBundleRequest.assetBundle;
            if (assetBundle == null)
            {
                LogError($"Failed to load {bundlePath}");
                yield break;
            }

            var request = assetBundle.LoadAllAssetsAsync();
            yield return request;

            MeteorShower = request.allAssets.FirstOrDefault(a => a.name.Equals("MeteorShower")) as GameObject;
            VolcanoSteam = request.allAssets.FirstOrDefault(a => a.name.Equals("VolcanoSteam")) as GameObject;

            AssetsLoaded = true;
        }
    }
}
