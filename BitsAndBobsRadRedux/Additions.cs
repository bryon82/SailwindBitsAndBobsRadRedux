using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BitsAndBobsRadRedux.BBRR_Plugin;

namespace BitsAndBobsRadRedux
{
    internal class Additions
    {
        public static GameObject MeteorShowerGO { get; private set; }
        public static GameObject VolcanoSteamGO { get; private set; }
        public static List<GameObject> DCGateLampGOs { get; private set; } = new List<GameObject>();

        internal static void AddDCGateLights()
        {
            var positions = new Vector3[2]
            {            
                new Vector3(0f, 3.35f, 3.1f),
                new Vector3(0f, -3.35f, 3.1f)
            };
            
            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            var parent = scenery.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.Equals("east_gate (4)"));
            var lamp = scenery.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("east_street_rope (1)")).GetChild(0);
            foreach (var position in positions)
            {
                var gateLamp = Object.Instantiate(lamp.gameObject, parent);
                gateLamp.transform.localScale = new Vector3(1.33f, 1.33f, 1.33f);
                gateLamp.transform.localPosition = position;
                gateLamp.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                gateLamp.GetComponent<Light>().range = 50f;
                gateLamp.GetComponent<Light>().intensity = 1.8f;
                DCGateLampGOs.Add(gateLamp);
            }
            LogDebug("Dragon Cliffs gate lamps added");
        }

        internal static IEnumerator AddMeteorShowers()
        {
            yield return new WaitUntil(() => AssetLoader.AssetsLoaded);

            if (AssetLoader.MeteorShower == null)
            {
                LogError("MeteorShower prefab not loaded");
                yield break;
            }
            
            var meteorShowers = Object.Instantiate(AssetLoader.MeteorShower, Camera.main.transform.position, AssetLoader.MeteorShower.transform.rotation);
            meteorShowers.AddComponent<MeteorShowerScheduler>();
            MeteorShowerGO = meteorShowers;
            LogDebug("Meteor showers added");
        }

        internal static IEnumerator AddMaleficSteam()
        {
            yield return new WaitUntil(() => AssetLoader.AssetsLoaded);

            if (AssetLoader.VolcanoSteam == null)
            {
                LogError("VolcanoSteam prefab not loaded");
                yield break;
            }            

            var parent = Refs.islands[17].transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("Cube_001"));
            var volcanoSteam = Object.Instantiate(AssetLoader.VolcanoSteam, parent);
            volcanoSteam.transform.localPosition = new Vector3(17.5f, 19f, 235f);
            volcanoSteam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            volcanoSteam.AddComponent<VolcanoSteamAdjuster>();
            VolcanoSteamGO = volcanoSteam;
            LogDebug("Volcano steam added");
        }
    }
}
