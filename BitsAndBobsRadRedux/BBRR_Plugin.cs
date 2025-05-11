using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine.SceneManagement;
using static BitsAndBobsRadRedux.Configs;

namespace BitsAndBobsRadRedux
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(ISA_BITS_AND_BOBS_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class BBRR_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.bitsandbobsradredux";
        public const string PLUGIN_NAME = "BitsAndBobsRadRedux";
        public const string PLUGIN_VERSION = "1.0.0";

        public const string ISA_BITS_AND_BOBS_GUID = "ISA_BitsAndBobs";

        internal static BBRR_Plugin Instance { get; private set; }
        private static ManualLogSource s_logger;

        internal static void LogDebug(string message) => s_logger.LogDebug(message);
        internal static void LogInfo(string message) => s_logger.LogInfo(message);
        internal static void LogWarning(string message) => s_logger.LogWarning(message);
        internal static void LogError(string message) => s_logger.LogError(message);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            s_logger = Logger;

            StartCoroutine(AssetLoader.LoadAssets());
            Configs.InitializeConfigs();

            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Equals(ISA_BITS_AND_BOBS_GUID))
                {
                    LogWarning($"{ISA_BITS_AND_BOBS_GUID} found, these mods will confict with each other.");
                    break;
                }
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "the ocean")
            {
                StartCoroutine(Additions.AddMaleficSteam());
                StartCoroutine(Additions.AddMeteorShowers());
            }
            if (scene.name == "island 9 E Dragon Cliffs")
            {
                Additions.AddDCGateLights();
            }
        }

        private void Update()
        {
            Additions.DCGateLampGOs.ForEach(lamp => lamp?.SetActive(addDcGateLamps.Value));
            Additions.MeteorShowerGO?.SetActive(addMeteorShowers.Value);
            Additions.VolcanoSteamGO?.SetActive(addMtMaleficSteam.Value);
        }
    }
}
