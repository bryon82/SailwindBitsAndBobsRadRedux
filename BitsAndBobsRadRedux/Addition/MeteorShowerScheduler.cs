using UnityEngine;
using static UnityEngine.ParticleSystem;
using static BitsAndBobsRadRedux.BBRR_Plugin;

namespace BitsAndBobsRadRedux
{
    internal class MeteorShowerScheduler : MonoBehaviour
    {        
        private const float BACKGROUND_RATE = 0.00014f;
        private const float BACKGROUND_DECLINATION = 60f;
        private const float BACKGROUND_RA = 350f;
        private const float PARTICLE_MULTIPLIER = 10f;
        private const float RADIUS_FROM_CAMERA = 2500f;

        private const float START_HOUR = 17f;
        private const float END_HOUR = 7f;

        private EmissionModule _emission;
        private ColorOverLifetimeModule _colorOverLifetime;
        private Camera _mainCamera;
        private bool _notified;

        private void Start()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            _emission = particleSystem.emission;
            _colorOverLifetime = particleSystem.colorOverLifetime;
            _mainCamera = Camera.main;
            _notified = false;

            ConfigureParticleSystem();
            MeteorShower.InitializeMeteorShowers();
        }        

        private void LateUpdate()
        {
            var localTime = Sun.sun.localTime;            
            var isNightTime =
                (localTime > START_HOUR && localTime <= 24f) ||
                (localTime >= 0f && localTime < END_HOUR);

            if (!isNightTime)
            {
                _emission.rateOverTime = 0f;
                _notified = false;
                return;
            }

            UpdateMeteorShower();
        }

        private void ConfigureParticleSystem()
        {
            var alphaKeys = new GradientAlphaKey[3];
            alphaKeys[0] = new GradientAlphaKey(0f, 0f);    // Start transparent
            alphaKeys[1] = new GradientAlphaKey(1f, 0.5f);  // Full brightness in middle
            alphaKeys[2] = new GradientAlphaKey(0f, 1f);    // End transparent

            var colorKeys = new GradientColorKey[1];
            colorKeys[0] = new GradientColorKey(Color.white, 0.0f);

            var gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            _colorOverLifetime.color = gradient;
        }

        private void UpdateMeteorShower()
        {
            var currentDay = GameState.day % 365;
            var rate = BACKGROUND_RATE;
            var declination = BACKGROUND_DECLINATION;
            var rightAscension = BACKGROUND_RA;

            foreach (var shower in MeteorShower.MeteorShowers)
            {
                var showerRate = shower.GetRateForDay(currentDay);
                if (showerRate > rate)
                {
                    rate = showerRate;
                    declination = shower.Declination;
                    rightAscension = shower.RightAscension;
                }

                if (!_notified && shower.GetRateForDay(currentDay) > 0f)
                {
                    LogDebug($"{shower.Name} meteor shower tonight: {shower.GetRateForDay(currentDay):0.000} meteors per hour");
                    _notified = true;                    
                }
            }

            _emission.rateOverTime = rate * PARTICLE_MULTIPLIER;

            var relativePosition = GetRelativePosition(declination, rightAscension);
            transform.position = _mainCamera.transform.position + relativePosition;
            transform.LookAt(_mainCamera.transform.position);
        }

        private Vector3 GetRelativePosition(float declination, float rightAscensionDegrees)
        {
            var degToRad = Mathf.Deg2Rad;
            var latitude = _mainCamera.transform.position.x / 9000f + 36f;
            var hourAngle = (Sun.sun.localTime / 24f * 360f) - rightAscensionDegrees;
            if (hourAngle < 0f)            
                hourAngle += 360f;            

            var sinAltitude = 
                Mathf.Sin(declination * degToRad) * Mathf.Sin(latitude * degToRad) +
                Mathf.Cos(declination * degToRad) * Mathf.Cos(latitude * degToRad) *
                Mathf.Cos(hourAngle * degToRad);
            var altitude = Mathf.Asin(sinAltitude) * Mathf.Rad2Deg;

            var cosAzimuth = 
                (Mathf.Sin(declination * degToRad) -
                Mathf.Sin(altitude * degToRad) * Mathf.Sin(latitude * degToRad)) /
                (Mathf.Cos(altitude * degToRad) * Mathf.Cos(latitude * degToRad));
            var azimuth = Mathf.Acos(cosAzimuth) * Mathf.Rad2Deg;
                        
            if (Mathf.Sin(hourAngle * degToRad) >= 0f)            
                azimuth = 360f - azimuth;            

            // Convert to Cartesian
            var x = RADIUS_FROM_CAMERA * Mathf.Cos(altitude * degToRad) * Mathf.Cos((-(azimuth - 90f)) * degToRad);
            var z = RADIUS_FROM_CAMERA * Mathf.Cos(altitude * degToRad) * Mathf.Sin((-(azimuth - 90f)) * degToRad);
            var y = RADIUS_FROM_CAMERA * Mathf.Sin(altitude * degToRad);

            return new Vector3(x, y, z);
        }        
    }
}
