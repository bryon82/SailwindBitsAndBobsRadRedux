using System.Collections.Generic;
using UnityEngine;

namespace BitsAndBobsRadRedux
{
    internal class MeteorShower
    {
        internal string Name { get; private set; }
        private readonly int _startDay;
        private readonly int _peakDay;
        private readonly int _endDay;
        private readonly float _peakRate;
        internal float Declination { get; private set; }
        internal float RightAscension { get; private set; }

        internal static List<MeteorShower> MeteorShowers { get; private set; }
        private const float SECONDS_IN_AN_HOUR = 3600f;

        internal MeteorShower(
            string name,
            int startDay,
            int peakDay,
            int endDay,
            float peakHourlyRate,
            float declination,
            float rightAscension)
        {
            Name = name;
            _startDay = startDay;
            _peakDay = peakDay;
            _endDay = endDay;
            _peakRate = peakHourlyRate / SECONDS_IN_AN_HOUR;
            Declination = declination;
            RightAscension = rightAscension;
        }

        internal float GetRateForDay(int day)
        {
            if (day < _startDay || day > _endDay)
                return 0f;

            float totalDuration = day <= _peakDay ? _peakDay - _startDay : _endDay - _peakDay;
            var distanceFromPeak = 1f - Mathf.Abs(day - _peakDay) / totalDuration;

            // quadratic bell curve to calculate rate
            return _peakRate * (1f - (1f - distanceFromPeak) * (1f - distanceFromPeak));
        }

        internal static void InitializeMeteorShowers()
        {
            MeteorShowers = new List<MeteorShower>
            {
                // Quadrantids (January)
                new MeteorShower
                (
                    name: "Quadrantids",
                    startDay: 0,
                    peakDay: 3,
                    endDay: 12,
                    peakHourlyRate: 120f,
                    declination: 50f,
                    rightAscension: 232f
                ),

                // Lyrids (April)
                new MeteorShower
                (
                    name: "Lyrids",
                    startDay: 104,
                    peakDay: 112,
                    endDay: 120,
                    peakHourlyRate: 18f,
                    declination: 33f,
                    rightAscension: 272.5f
                ),

                // Arietids (June)
                new MeteorShower
                (
                    name: "Arietids",
                    startDay: 142,
                    peakDay: 158,
                    endDay: 175,
                    peakHourlyRate: 60f,
                    declination: 25f,
                    rightAscension: 45.5f
                ),

                // Perseids (August)
                new MeteorShower
                (
                    name: "Perseids",
                    startDay: 198,
                    peakDay: 224,
                    endDay: 236,
                    peakHourlyRate: 100f,
                    declination: 58f,
                    rightAscension: 48.25f
                ),

                // Orionids (October)
                new MeteorShower
                (
                    name: "Orionids",
                    startDay: 275,
                    peakDay: 295,
                    endDay: 316,
                    peakHourlyRate: 20f,
                    declination: 16f,
                    rightAscension: 96f
                ),

                // Leonids (November)
                new MeteorShower
                (
                    name: "Leonids",
                    startDay: 307,
                    peakDay: 321,
                    endDay: 334,
                    peakHourlyRate: 15f,
                    declination: 21.6f,
                    rightAscension: 154.25f
                ),

                // Geminids (December)
                new MeteorShower
                (
                    name: "Geminids",
                    startDay: 335,
                    peakDay: 346,
                    endDay: 351,
                    peakHourlyRate: 150f,
                    declination: 32f,
                    rightAscension: 108.75f
                )
            };
        }
    }
}
