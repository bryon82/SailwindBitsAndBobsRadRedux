using UnityEngine;
using static UnityEngine.ParticleSystem;
using static BitsAndBobsRadRedux.Configs;

namespace BitsAndBobsRadRedux
{
    internal class BBRR_VolcanoSteamAdjuster : MonoBehaviour
    {
        private EmissionModule _emission;
        private ForceOverLifetimeModule _forceLifetime;
        private Material _material;
        private Wind _wind;
        private float _timer = 0f;

        private readonly Color32[] _lightingColors = new Color32[4]
        {
            new Color32(0x1D, 0x1C, 0x1B, 0xEE),    // Night
            new Color32(0x26, 0x26, 0x26, 0xEE),    // Dawn start / Dusk end
            new Color32(0xD6, 0xBA, 0x84, 0xEE),    // Dawn end / Dusk start
            new Color32(0xFF, 0xF6, 0xEA, 0xEE)     // Day
        };

        private const float LOW_ACTIVITY_CHANCE = 0.33f;
        private const float HIGH_ACTIVITY_CHANCE = 0.66f;

        private const float PARTICLE_LIFETIME = 90f;
        private const int MAX_PARTICLES = 1000;

        private struct TimeColorTransition
        {
            public float startTime;
            public float endTime;
            public int startColorIndex;
            public int endColorIndex;
        }

        private readonly TimeColorTransition[] _colorTransitions = new TimeColorTransition[]
        {
            // dawn
            new TimeColorTransition { startTime = 4f, endTime = 5f, startColorIndex = 0, endColorIndex = 1 },
            new TimeColorTransition { startTime = 5f, endTime = 6f, startColorIndex = 1, endColorIndex = 2 },

            // day
            new TimeColorTransition { startTime = 6f, endTime = 12f, startColorIndex = 2, endColorIndex = 3 },
            new TimeColorTransition { startTime = 12f, endTime = 18f, startColorIndex = 3, endColorIndex = 2 },

            // dusk
            new TimeColorTransition { startTime = 18f, endTime = 19f, startColorIndex = 2, endColorIndex = 1 },
            new TimeColorTransition { startTime = 19f, endTime = 20f, startColorIndex = 1, endColorIndex = 0 }
        };

        public void Start()
        {
            var particleSystem = GetComponentInChildren<ParticleSystem>();
            _emission = particleSystem.emission;
            _forceLifetime = particleSystem.forceOverLifetime;
            _material = particleSystem.GetComponent<Renderer>().material;
            _material.renderQueue = 2999;

            var main = particleSystem.main;
            main.startLifetime = PARTICLE_LIFETIME;
            main.maxParticles = MAX_PARTICLES;

            _wind = GameObject.Find("wind")?.GetComponent<Wind>();
        }

        public void Update()
        {
            UpdateEmissionState();
            UpdateWindForce();
            UpdateDayNightColor();
        }

        private void UpdateEmissionState()
        {
            if (_timer <= 0f)
            {
                var randomValue = Random.Range(0f, 1f);
                var isActive = true;

                if (mtMaleficActivity.Value == 0)
                {
                    isActive = randomValue < LOW_ACTIVITY_CHANCE;
                    _timer = isActive ? Random.Range(60f, 120f) : Random.Range(300f, 900f);
                }
                else if (mtMaleficActivity.Value == 1)
                {
                    isActive = randomValue < HIGH_ACTIVITY_CHANCE;
                    _timer = isActive ? Random.Range(300f, 600f) : Random.Range(120f, 300f);
                }

                _emission.enabled = isActive;
            }

            _timer -= Time.deltaTime;
        }

        private void UpdateWindForce()
        {
            var windVector = _wind.outCurrentBaseWind * _wind.outCurrentMagnitude;
            _forceLifetime.x = new MinMaxCurve(windVector.x * Time.deltaTime / 18f);
            _forceLifetime.y = new MinMaxCurve((0f - windVector.z) * Time.deltaTime / 18f);
        }

        private void UpdateDayNightColor()
        {
            var localTime = Sun.sun.localTime;
            var currentColor = _lightingColors[0];

            // Day time
            if (localTime >= 4f && localTime < 20f )  
            {
                foreach (var transition in _colorTransitions)
                {
                    if (localTime > transition.startTime && localTime <= transition.endTime)
                    {
                        var startColor = _lightingColors[transition.startColorIndex];
                        var endColor = _lightingColors[transition.endColorIndex];
                        var t = (localTime - transition.startTime) / (transition.endTime - transition.startTime);
                        currentColor = Color.Lerp(startColor, endColor, t);
                        break;
                    }
                }
            }

            _material.SetColor("_Color", currentColor);
        }
    }
}
