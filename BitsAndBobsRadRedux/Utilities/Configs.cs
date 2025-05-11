using BepInEx.Configuration;
using static BitsAndBobsRadRedux.BBRR_Plugin;

namespace BitsAndBobsRadRedux
{
    internal class Configs
    {
        internal static ConfigEntry<bool> addDcGateLamps;
        internal static ConfigEntry<bool> disablePlayerLight;
        internal static ConfigEntry<bool> addMeteorShowers;
        internal static ConfigEntry<bool> enableBlueTobacco;
        internal static ConfigEntry<int> bluePotencyMult;
        internal static ConfigEntry<bool> addMtMaleficSteam;
        internal static ConfigEntry<int> mtMaleficActivity;

        internal static void InitializeConfigs()
        {
            var config = Instance.Config;

            addDcGateLamps = config.Bind("Settings", "Add DC Gate Lamps", true, "Adds lamps to the gates of Dragon Cliffs.");
            disablePlayerLight = config.Bind("Settings", "Disable Ambient Player Light", true, "Disables ambient player light.");
            addMeteorShowers = config.Bind("Settings", "Add Meteor Showers", true, "Adds occasional shooting stars and northern hemisphere meteor showers on their respective days (day 1 = 1 jan, day 366 = 1 jan, etc.)");
            enableBlueTobacco = config.Bind("Settings", "Enable Blue Tobacco Potency", true, "Makes blue tobacco more potent.");
            bluePotencyMult = config.Bind("Settings", "Blue Tobacco Potency Multiplier", 6, "The potency multiplier of blue tobacco as compared to green tobacco. Default is 6.");
            addMtMaleficSteam = config.Bind("Settings", "Add Mt. Malefic Steam", true, "Adds steam being emitted from Mt. Malefic");
            mtMaleficActivity = config.Bind("Settings", "Mt. Malefic Activity", 1, "Mt. Malefic activity level: 0 = occasional emissions, 1 = more frequent and longer duration emissions, 2 = constant emission");
        }
    }
}
