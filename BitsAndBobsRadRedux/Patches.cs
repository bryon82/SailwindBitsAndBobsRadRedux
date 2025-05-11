using HarmonyLib;
using UnityEngine;
using static BitsAndBobsRadRedux.Configs;

namespace BitsAndBobsRadRedux 
{
    internal class Patches
    {
        [HarmonyPatch(typeof(Sun))]
        private class PlayerLightPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Start")]
            public static void DisablePlayerLight()
            {
                if (!disablePlayerLight.Value) return;

                Light component = Camera.main.GetComponent<Light>();
                component.enabled = false;
            }
        }

        [HarmonyPatch(typeof(SaveablePrefab))]
        private class SaveablePrefabPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Start")]
            public static void PostfixStart(SaveablePrefab __instance)
            {
                if (__instance.gameObject.name.StartsWith("318 tobacco blue"))
                    __instance.gameObject.GetComponent<ShipItemTobacco>().tobaccoType = 5;
            }
        }

        [HarmonyPatch(typeof(PlayerTobacco))]
        private class PlayerTobaccoPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Smoke")]
            public static void PostfixSmoke(PlayerTobacco __instance, int tobaccoType)
            {
                if (tobaccoType == 5)
                {
                    if (!enableBlueTobacco.Value)
                    {
                        __instance.green += Time.deltaTime * 2f;
                        PlayerNeeds.sleep -= Time.deltaTime * 0.11f;
                        return;
                    }

                    __instance.green += Time.deltaTime * 2f * bluePotencyMult.Value * 0.33f;
                    PlayerNeeds.sleep -= Time.deltaTime * 0.11f * bluePotencyMult.Value;
                }                               
            }
        }        
    }
}
