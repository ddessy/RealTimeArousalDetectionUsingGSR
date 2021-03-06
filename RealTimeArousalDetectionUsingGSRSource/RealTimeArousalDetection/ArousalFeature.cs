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

using System;
using System.Text;

namespace Assets.Rage.RealTimeArousalDetectionUsingGSRAsset.Utils
{
    public class ArousalFeature
    {
        private double minimum;
        private double maximum;
        private Decimal mean;
        private Decimal stdDeviation;
        private String name;
        private double count;

        public ArousalFeature()
        {
            //super();
        }

        public ArousalFeature(String name)
        {
            this.name = name;
        }

        public double Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }

        public double Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }

        public Decimal Mean
        {
            get
            {
                return mean;
            }
            set
            {
                mean = value;
            }
        }

        public Decimal StdDeviation
        {
            get
            {
                return stdDeviation;
            }
            set
            {
                stdDeviation = value;
            }
        }

        public double Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Statistics for " + name + ": \n");
            str.Append("Minimum value: " + minimum + "\n");
            str.Append("Maximum value: " + maximum + "\n");
            str.Append("Mean value: " + mean + "\n");
            str.Append("Standard deviation: " + stdDeviation + "\n");
            str.Append("Count: " + count + "\n");

            return str.ToString();
        }
    }
}
