using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Polaris.Kinect.Resources;

namespace Polaris.Kinect.Extensions
{
    public static class KinectExtensions
    {
        public static KinectSensor GetDefaultKinectSensor()
        {
            var sensorsCount = KinectSensor.KinectSensors.Count;
            if (sensorsCount == 0)
            {
                throw new KinectNotFoundException(ExceptionStrings.KinectSensorNotFound);
            }
            KinectSensor sensor = (from sensorToCheck in KinectSensor.KinectSensors
                                   where sensorToCheck.Status == KinectStatus.Connected
                                   select sensorToCheck).FirstOrDefault();
            if (sensor == null)
            {
                throw new KinectNotFoundException(ExceptionStrings.ConnectedKinectSensorNotFound);
            }
            return sensor;
        }

        public static void EnsureStart(this KinectSensor target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (!target.IsRunning)
            {
                target.Start();
            }
        }

        public static KinectAudioSource GetKinectAudioSource(this KinectSensor target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            // Obtain the KinectAudioSource to do audio capture
            KinectAudioSource source = target.AudioSource;
            source.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression; // No AEC for this sample
            source.AutomaticGainControlEnabled = false; // Important to turn this off for speech recognition
            return source;
        }
    }
}