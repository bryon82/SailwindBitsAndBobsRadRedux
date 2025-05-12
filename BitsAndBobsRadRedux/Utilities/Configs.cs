using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using static BitsAndBobsRadRedux.BBRR_Plugin;

namespace BitsAndBobsRadRedux
{
    internal class Configs
    {
        internal static ConfigEntry<bool> addDcGateLamps;        
        internal static ConfigEntry<bool> addMeteorShowers;        
        internal static ConfigEntry<bool> addMtMaleficSteam;
        internal static ConfigEntry<int> mtMaleficActivity;
        internal static ConfigEntry<bool> enablePlayerLight;
        internal static ConfigEntry<bool> enableActivateInventoryLamp;
        internal static ConfigEntry<bool> enableBlueTobacco;
        internal static ConfigEntry<int> bluePotencyMult;

        internal static void InitializeConfigs()
        {
            var config = Instance.Config;

            addDcGateLamps = config.Bind("Additions", "Add DC Gate Lamps", true, "Adds lamps to the gates of Dragon Cliffs.");
            addMeteorShowers = config.Bind("Additions", "Add Meteor Showers", true, "Adds occasional shooting stars and northern hemisphere meteor showers on their respective days (day 1 = 1 jan, day 366 = 1 jan, etc.)");
            addMtMaleficSteam = config.Bind("Additions", "Add Mt. Malefic Steam", true, "Adds steam being emitted from Mt. Malefic");
            mtMaleficActivity = config.Bind("Additions", "Mt. Malefic Activity", 1, "Mt. Malefic activity level: 0 = occasional emissions, 1 = more frequent and longer duration emissions, 2 = constant emission");
            enablePlayerLight = config.Bind("Options", "Ambient Player Light", false, "Light eminating from the player.");
            enableActivateInventoryLamp = config.Bind("Options", "Activate Lamp In Inventory", true, "The ability to activate the lamp in an inventory slot by right-clicking it (or using other activation button).");
            enableBlueTobacco = config.Bind("Options", "Increase Blue Tobacco Potency", true, "Makes blue tobacco more potent.");
            bluePotencyMult = config.Bind("Options", "Blue Tobacco Potency Multiplier", 6, "The potency multiplier of blue tobacco as compared to green tobacco. Default is 6.");
        }

        internal static void UpdateConfigs()
        {
            Setup.MeteorShowerGO?.SetActive(addMeteorShowers.Value);
            Setup.VolcanoSteamGO?.SetActive(addMtMaleficSteam.Value);

            if (SceneManager.GetSceneByName(DRAGON_CLIFFS_SCENE).isLoaded)
                Setup.DCGateLampGOs?.ForEach(lamp => lamp?.SetActive(addDcGateLamps.Value));

            if (Setup.PlayerLight != null)
                Setup.PlayerLight.enabled = enablePlayerLight.Value;
        }
    }
}
