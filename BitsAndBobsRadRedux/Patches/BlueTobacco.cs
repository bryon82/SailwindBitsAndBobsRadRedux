using HarmonyLib;
using UnityEngine;
using static BitsAndBobsRadRedux.Configs;

namespace BitsAndBobsRadRedux 
{
    internal class BlueTobacco
    {
        [HarmonyPatch(typeof(SaveablePrefab), "Start")]
        private class SetBlueTobaccoType
        {            
            public static void Postfix(SaveablePrefab __instance)
            {
                if (__instance.gameObject.name.StartsWith("318 tobacco blue"))
                    __instance.gameObject.GetComponent<ShipItemTobacco>().tobaccoType = 5;
            }
        }

        [HarmonyPatch(typeof(PlayerTobacco), "Smoke")]
        private class SetBlueTobaccoPotency
        {
            public static void Postfix(PlayerTobacco __instance, int tobaccoType)
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
