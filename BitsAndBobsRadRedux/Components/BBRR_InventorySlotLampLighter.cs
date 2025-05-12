using HarmonyLib;
using UnityEngine;
using static BitsAndBobsRadRedux.Configs;

namespace BitsAndBobsRadRedux
{
    internal class BBRR_InventorySlotLampLighter : MonoBehaviour
    {
        internal GPButtonInventorySlot InventorySlot { get; set; }

        private void LateUpdate()
        {
            if (!enableActivateInventoryLamp.Value || !InventorySlot.IsLookedAt())
                return; 

            var currentItem = InventorySlot.currentItem;

            if (currentItem is ShipItemLight && (GameInput.GetKeyDown(InputName.Activate) || Input.GetMouseButtonDown(1)))
                currentItem.OnAltActivate();
        }
    }

    [HarmonyPatch(typeof(GPButtonInventorySlot), "Awake")]
    public class AddInventorySlotLampLighter
    {
        public static void Postfix(GPButtonInventorySlot __instance)
        {
            var lampLighter = __instance.gameObject.AddComponent<BBRR_InventorySlotLampLighter>();
            lampLighter.InventorySlot = __instance;
        }
    }
}
