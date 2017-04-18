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

using AssetManagerPackage;
using AssetPackage;
using Assets.Rage.GSRAsset.SignalDevice;
using Assets.Rage.GSRAsset.SignalProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Rage.GSRAsset.Integrator
{
    public class RealTimeArousalDetectionUsingGSRAsset : BaseAsset, IArousalDetection, ISignalDeviceController
    {
        #region Fields
        private RealTimeArousalDetectionAssetSettings settings = null;
        GSRHRDevice gsrDevice;
        GSRSignalProcessor gsrProcessor;
        #endregion Fields

        #region Constructors
        public RealTimeArousalDetectionUsingGSRAsset() : base()
        {
            gsrDevice = new GSRHRDevice();
            gsrProcessor = new GSRSignalProcessor();
            settings = new RealTimeArousalDetectionAssetSettings();

            //preventing multiple asset creation
            if (AssetManager.Instance.findAssetsByClass(this.Class).Count > 1)
            {
                Logger.Log("Attempt for more than one instance of the class RealTimeArousalDetectionUsingGSRAsset (it is not allowed).");
            }
        }
        #endregion Constructors

        #region Properties
        public override ISettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = (value as RealTimeArousalDetectionAssetSettings);
            }
        }

        #endregion Properties


        #region PublicMethods
        public GSRHRDevice GetGSRDevice()
        {
            return gsrDevice;
        }

        public GSRSignalProcessor GetGSRSignalProcessor()
        {
            return gsrProcessor;
        }

        public double GetGSRFeature(string featureName)
        {
            if ("SCRArousalArea".Equals(featureName))
            {
                return gsrProcessor.GetArousalStatistics().SCRArousalArea;
            }

            if ("SCRAchievedArousalLevel".Equals(featureName))
            {
                return gsrProcessor.GetArousalStatistics().SCRAchievedArousalLevel;
            }

            if ("SCLAchievedArousalLevel".Equals(featureName))
            {
                return gsrProcessor.GetArousalStatistics().SCLAchievedArousalLevel;
            }

            if ("MovingAverage".Equals(featureName))
            {
                return gsrProcessor.GetArousalStatistics().MovingAverage;
            }

            return -1;
        }

        public void GetSignalData(byte[] data)
        {
            gsrDevice.GetSignalData(data);
        }

        public int GetSignalSampleRate()
        {
            return gsrDevice.GetSignalSampleRate();
        }

        public int GetSignalSampleRateByConfig()
        {
            return gsrDevice.GetSignalSampleRate();
        }

        public void OpenPort()
        {
            gsrDevice.OpenPort();
        }

        public void SelectCOMPort(string portName)
        {
            gsrDevice.SelectCOMPort(portName);
        }

        public int SetMaxArousalLevel(int numberOfLevels)
        {
            if (numberOfLevels < 100 && numberOfLevels > 0)
            {
                gsrProcessor.ArousalLevel = numberOfLevels;
                return 0;
            }

            return -1;
        }

        public void SetSignalSamplerate()
        {
            gsrDevice.SetSignalSamplerate();
        }

        public int SetSignalSamplerate(string speed)
        {
            return gsrDevice.SetSignalSamplerate(speed);
        }

        public void SetSignalSettings()
        {
            gsrDevice.SetSignalSettings();
        }

        public int SetTimeWindow(double timeWindow)
        {
            if (timeWindow > 0)
            {
                gsrProcessor.DefaultTimeWindow = Convert.ToDouble(timeWindow);
                return 0;
            }

            return -1;
        }

        public int StartSignalsRecord()
        {
            return gsrDevice.StartSignalsRecord();
        }

        public int StopSignalsRecord()
        {
            return gsrDevice.StartSignalsRecord();
        }

        public enum SettingsLocation
        {
            /// <summary>
            /// An enum constant representing the local option.
            /// </summary>
            Local,
            /// <summary>
            /// An enum constant representing the server option.
            /// </summary>
            Server
        }

        #endregion PublicMethods

        #region InternalMethods
        /// <summary>
        /// Method returning the RealTimeArousalDetectionAssetSettings for internal use.
        /// </summary>
        /// <returns> Settings of this Asset. </returns>
        internal RealTimeArousalDetectionAssetSettings getSettings()
        {
            return (this.settings);
        }

        internal RealTimeArousalDetectionAssetSettings LoadDefaultRealTimeArousalDetectionAssetSettings(String location)
        {
            RealTimeArousalDetectionAssetSettings gsrSettings = (RealTimeArousalDetectionAssetSettings)AssetManager.Instance.findAssetByClass("RealTimeArousalDetectionUsingGSRAsset").Settings;
            if (SettingsLocation.Local.Equals(location))
            {
                IDataStorage storage = (IDataStorage)AssetManager.Instance.Bridge;
                if (storage != null && storage.Exists(gsrSettings.LocalSource))
                {
                    return (this.getRealTimeArousalDetectionAssetSettingsByString(storage.Load(gsrSettings.LocalSource)));
                }
            }

            if (SettingsLocation.Server.Equals(location))
            {
                IWebServiceRequest serverRequest = (IWebServiceRequest)AssetManager.Instance.Bridge;
                if (serverRequest != null)
                {
                    RequestSetttings requestSettings = new RequestSetttings();
                    requestSettings.method = "GET";
                    requestSettings.uri = new Uri(gsrSettings.ServerSource);
                    requestSettings.requestHeaders = new Dictionary<string, string>(); 

                    RequestResponse requestResponse = new RequestResponse();

                    serverRequest.WebServiceRequest(requestSettings, out requestResponse);
                    return (this.getRealTimeArousalDetectionAssetSettingsByString(requestResponse.body));
                }
            }

            return null;
        }

        /// <summary>
        /// Method for storing the settings to a xml file.
        /// </summary>
        /// 
        /// <param name="settings"> RealTimeArousalDetectionAssetSettings value. </param>
        /// <param name="configFile"> path to the target file. </param>
        internal void writeSettingsToFile(RealTimeArousalDetectionAssetSettings settings, String configFile)
        {
            IDataStorage storage = (IDataStorage)AssetManager.Instance.Bridge;
            if (storage != null)
            {
                storage.Save(configFile, settings.SettingsToXmlString());
            }
        }

        internal RealTimeArousalDetectionAssetSettings getRealTimeArousalDetectionAssetSettingsByString(String xmlContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RealTimeArousalDetectionAssetSettings));
            using (TextReader reader = new StringReader(xmlContent))
            {
                RealTimeArousalDetectionAssetSettings result = (RealTimeArousalDetectionAssetSettings)serializer.Deserialize(reader);
                return (result);
            }
        }


        #endregion InternalMethods
    }

}