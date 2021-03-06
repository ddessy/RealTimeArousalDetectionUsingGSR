﻿/*
 * Copyright 2016 Sofia University
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * This project has received funding from the European Union's Horizon
 * 2020 research and innovation programme under grant agreement No 644187.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


namespace Assets.Rage.RealTimeArousalDetectionUsingGSRAsset.Utils
{
    public class InflectionPoint
    {
        private double x;
        private double y;
        private int index;
        private ExtremaType extremumType;

        public InflectionPoint(double x1, double y1, int indexOrigin)
        {
            this.x = x1;
            this.y = y1;
            this.index = indexOrigin;
        }

        public InflectionPoint(SignalDataByTime signalValue, int indexOrigin, ExtremaType extremum)
        {
            this.x = signalValue.Time;
            this.y = signalValue.SignalValue;
            this.index = indexOrigin;
            this.extremumType = extremum;
        }


        public ExtremaType ExtremaType
        {
            get
            {
                return extremumType;
            }
            set
            {
                extremumType= value;
            }
        }

        public int IndexOrigin
        {
            get
            {
                return index;
            }
            set
            {
                x = value;
            }
        }

        public double CoordinateX
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double CoordinateY
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}
