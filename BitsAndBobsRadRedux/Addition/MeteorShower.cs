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

        internal MeteorShower(
            string name,
            int startDay,
            int peakDay,
            int endDay,
            float peakRate,
            float declination,
            float rightAscension)
        {
            Name = name;
            _startDay = startDay;
            _peakDay = peakDay;
            _endDay = endDay;
            _peakRate = peakRate;
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
                // Quadrantids (early January)
                new MeteorShower
                (
                    name: "Quadrantids",
                    startDay: 1,
                    peakDay: 4,
                    endDay: 12,
                    peakRate: 0.03069f,
                    declination: 49f,
                    rightAscension: 229.5f
                ),

                // Gamma Ursae Minorids (mid January)
                new MeteorShower
                (
                    name: "Gamma Ursae Minorids",
                    startDay: 12,
                    peakDay: 19,
                    endDay: 22,
                    peakRate: 0.00097f,
                    declination: 67f,
                    rightAscension: 228f
                ),

                // Lyrids (April)
                new MeteorShower
                (
                    name: "Lyrids",
                    startDay: 104,
                    peakDay: 112,
                    endDay: 120,
                    peakRate: 0.005f,
                    declination: 36.4667f,
                    rightAscension: 271.5f
                ),

                // Perseids (August)
                new MeteorShower
                (
                    name: "Perseids",
                    startDay: 198,
                    peakDay: 224,
                    endDay: 236,
                    peakRate: 0.02778f,
                    declination: 58f,
                    rightAscension: 48f
                ),

                // Orionids (October)
                new MeteorShower
                (
                    name: "Orionids",
                    startDay: 275,
                    peakDay: 295,
                    endDay: 316,
                    peakRate: 0.00555f,
                    declination: 16f,
                    rightAscension: 96f
                ),

                // Geminids (mid December)
                new MeteorShower
                (
                    name: "Geminids",
                    startDay: 335,
                    peakDay: 346,
                    endDay: 351,
                    peakRate: 0.03658f,
                    declination: 32f,
                    rightAscension: 108.75f
                ),

                // Quadrantids (end of year, early January)
                new MeteorShower
                (
                    name: "Quadrantids",
                    startDay: 362,
                    peakDay: 365,
                    endDay: 366, // day 1
                    peakRate: 0.00052f,
                    declination: 49f,
                    rightAscension: 229.5f
                )
            };
        }
    }
}
