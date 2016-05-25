﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Rage.GSRAsset
{
    class FilterMedian
    {
        private Dictionary<int, Dictionary<double, double>> signalCoordinates;

        public FilterMedian()
        {
            //super();
        }

        public FilterMedian(Dictionary<int, Dictionary<double, double>> signalCoordinates)
        {
            this.signalCoordinates = signalCoordinates;
        }

        public Dictionary<int, Dictionary<double, double>> GetMedianFilterPoints()
        {
            if (this.signalCoordinates == null) return null;

            Dictionary<int, Dictionary<double, double>> result = new Dictionary<int, Dictionary<double, double>>();
            signalCoordinates = extendSignalCoordinates(signalCoordinates);

            foreach (KeyValuePair<int, Dictionary<double, double>> channelCoordinates in signalCoordinates)
            {
                Dictionary<double, double> resultChannelCoordinates = new Dictionary<double, double>();
                Dictionary<double, double>.ValueCollection channelCoordinateValues = channelCoordinates.Value.Values;
                Dictionary<double, double>.KeyCollection channelCoordinatesKeys = channelCoordinates.Value.Keys;
                int vectorCoordinatesSize = channelCoordinateValues.Count;
                for (int i = 2; i < vectorCoordinatesSize - 2; ++i)
                {
                    double[] window = new double[5];
                    for (int j = 0; j < 5; ++j)
                    {
                        window[j] = channelCoordinateValues.ElementAt(i - 2 + j);
                    }

                    for (int j = 0; j < 3; ++j)
                    {
                        //   Find position of minimum element
                        int min = j;
                        for (int k = j + 1; k < 5; ++k)
                            if (window[k] < window[min])
                                min = k;
                        //   Put found minimum element in its place
                        double temp = window[j];
                        window[j] = window[min];
                        window[min] = temp;
                    }

                    //   Get result - the middle element
                    resultChannelCoordinates.Add(channelCoordinatesKeys.ElementAt(i), window[2]);
                }

                /* for (int i = 0; i < 2; ++i)
                 {
                     resultChannelCoordinates.Add(channelCoordinatesKeys.ElementAt(i), channelCoordinateValues.ElementAt(1 - i));
                     resultChannelCoordinates.Add(channelCoordinatesKeys.ElementAt(vectorCoordinatesSize - 2 + i), channelCoordinateValues.ElementAt(vectorCoordinatesSize - 1 - i));
                 }*/

                result.Add(channelCoordinates.Key, resultChannelCoordinates);
            }

            return result;
        }

        private Dictionary<int, Dictionary<double, double>> extendSignalCoordinates(Dictionary<int, Dictionary<double, double>> signalCoordinates)
        {
            Dictionary<int, Dictionary<double, double>> result = new Dictionary<int, Dictionary<double, double>>();
            foreach (KeyValuePair<int, Dictionary<double, double>> channelCoordinates in signalCoordinates)
            {
                int coordinatesCount = channelCoordinates.Value.Count;
                Dictionary<double, double> coordinatesValue = channelCoordinates.Value;
                Dictionary<double, double> extendedChannelCoordinates = new Dictionary<double, double>()
                {
                    {coordinatesValue.ElementAt(coordinatesCount - 1).Key - 20.0, coordinatesValue.ElementAt(coordinatesCount - 2).Value},
                    {coordinatesValue.ElementAt(coordinatesCount - 1).Key - 10.0, coordinatesValue.ElementAt(coordinatesCount - 1).Value}
                };

                foreach (KeyValuePair<double, double> currentCoordinate in channelCoordinates.Value)
                {
                    extendedChannelCoordinates.Add(currentCoordinate.Key, currentCoordinate.Value);
                }

                extendedChannelCoordinates.Add((coordinatesValue.ElementAt(0).Key + 10.0), coordinatesValue.ElementAt(0).Value);
                extendedChannelCoordinates.Add((coordinatesValue.ElementAt(0).Key + 20.0), coordinatesValue.ElementAt(1).Value);

                result.Add(channelCoordinates.Key, extendedChannelCoordinates);
            }

            return result;
        }
    }
}
