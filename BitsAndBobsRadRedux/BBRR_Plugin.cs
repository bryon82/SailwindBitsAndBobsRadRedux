using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace BitsAndBobsRadRedux
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class BBRR_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.bitsandbobsradredux";
        public const string PLUGIN_NAME = "BitsAndBobsRadRedux";
        public const string PLUGIN_VERSION = "1.0.0";

        private const string OCEAN_SCENE = "the ocean";
        internal const string DRAGON_CLIFFS_SCENE = "island 9 E Dragon Cliffs";

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

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            StartCoroutine(Setup.SetPlayerLight());
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name.Equals(OCEAN_SCENE))
            {
                StartCoroutine(Setup.AddMeteorShowers());
                StartCoroutine(Setup.AddMaleficSteam());
            }
            if (scene.name.Equals(DRAGON_CLIFFS_SCENE))
            {
                Setup.AddDCGateLights();
            }
        }

        private void Update()
        {
            Configs.UpdateConfigs();
        }
    }
}
