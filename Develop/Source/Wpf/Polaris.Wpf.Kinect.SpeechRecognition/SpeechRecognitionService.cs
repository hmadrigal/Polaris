using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Polaris.Kinect.Extensions;
using Polaris.Kinect.Resources;

namespace Polaris.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="Rambutan.Services.ISpeechRecognitionService"/>
    /// </summary>
    internal class SpeechRecognitionService : ISpeechRecognitionService
    {
        #region Constants

        public const String KinectSpeechRecognizerFlag = "Kinect";
        public const String EnglishUnitedStatesCulture = "en-US";

        public const String IsListeningToCommandsMonitorThreadName = "IsListeningToCommandsMonitorThread";

        #endregion Constants

        #region Fields

        private object isInitializedSync = new object();
        private bool isInitialized;

        private object isRunningSync = new object();
        private bool isRunning;

        private object grammarCollectionSync = new object();
        private List<RegisteredGrammar> grammarCollection;
        private SpeechRecognitionEngine speechRecognitionEngine;
        private KinectAudioSource audioSource;

        private Subject<ISpeechRecognitionPayload> speechRecognitionSubject = new Subject<ISpeechRecognitionPayload>();
        Dictionary<Int32, IDisposable> speechRecognizedListeners;

        private Subject<Boolean> listeningStatusSubject = new Subject<bool>();
        Dictionary<Int32, IDisposable> statusListeners;

        private RecognizerInfo speechRecognizer;

        private object isListeningToCommandsSync = new object();
        private DateTime isListeningToCommandsSetTime;

        private bool isListeningToCommands;
        private string applicationName;
        private System.IO.Stream audioSourceStream;

        protected DateTime IsListeningToCommandsSetTime
        {
            get
            {
                lock (isListeningToCommandsSync)
                {
                    return isListeningToCommandsSetTime;
                }
            }
            set
            {
                lock (isListeningToCommandsSync)
                {
                    if (isListeningToCommandsSetTime != value)
                    {
                        isListeningToCommandsSetTime = value;
                    }
                }
            }
        }

        #endregion Fields

        #region Properties

        public bool IsInitialized
        {
            get
            {
                lock (isInitializedSync)
                {
                    return isInitialized;
                }
            }
            set
            {
                lock (isInitializedSync)
                {
                    isInitialized = value;
                }
            }
        }

        #region ISpeechRecognitionService Members

        public bool IsListeningToCommands
        {
            get
            {
                lock (isListeningToCommandsSync)
                {
                    return isListeningToCommands;
                }
            }
            set
            {
                lock (isListeningToCommandsSync)
                {
                    if (isListeningToCommands != value)
                    {
                        isListeningToCommands = value;
                        if (value)
                        {
                            IsListeningToCommandsSetTime = DateTime.Now;
                        }
                        this.listeningStatusSubject.OnNext(value);
                    }
                }
            }
        }

        #endregion ISpeechRecognitionService Members

        #endregion Properties

        #region Constructor

        public SpeechRecognitionService(IUnityContainer container)
        {
            this.grammarCollection = new List<RegisteredGrammar>();
            this.speechRecognizedListeners = new Dictionary<int, IDisposable>();
            this.statusListeners = new Dictionary<int, IDisposable>();
            this.Container = container;
        }

        #endregion Constructor

        #region Methods

        #region ISpeechRecognitionService Members

        public void Initialize()
        {
            lock (isInitializedSync)
            {
                if (IsInitialized) { return; }
                applicationName = Container.IsRegistered<String>(GlobalInstanceNames.ApplicationName) ? Container.Resolve<String>(GlobalInstanceNames.ApplicationName) : String.Empty;
                KinectSensor sensor = KinectExtensions.GetDefaultKinectSensor();
                sensor.EnsureStart();
                this.audioSource = sensor.GetKinectAudioSource();
                this.speechRecognizer = this.GetKinectSpeechRecognizer();
                IsInitialized = true;
            }
        }

        public void Start()
        {
            if (!IsInitialized)
            {
                throw new ServiceNotInitializedException(ExceptionStrings.SpeechRecognitionServiceNotInitialized);
            }
            lock (isRunningSync)
            {
                isRunning = true;
            }
        }

        public void Stop()
        {
            if (!IsInitialized)
            {
                throw new ServiceNotInitializedException(ExceptionStrings.SpeechRecognitionServiceNotInitialized);
            }
            lock (isRunningSync)
            {
                isRunning = false;
            }
        }

        public Guid AddSpeechCommandSet(string command, string[] commandArguments, Guid creatorId)
        {
            if (!IsInitialized)
            {
                throw new ServiceNotInitializedException(ExceptionStrings.SpeechRecognitionServiceNotInitialized);
            }

            var choices = new Choices();

            foreach (var item in commandArguments)
            {
                choices.Add(item);
            }
            var gb = new GrammarBuilder { Culture = CultureInfo.GetCultureInfo(EnglishUnitedStatesCulture) };

            // Specify the culture to match the recognizer in case we are running in a different culture.
            gb.Append(command);
            if (commandArguments.Length > 0)
            {
                gb.Append(choices);
            }
            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);
            var registrationId = Guid.NewGuid();
            this.grammarCollection.Add(new RegisteredGrammar() { CreatorId = creatorId, Grammar = g, RegistrationId = registrationId, Command = command });
            if (this.speechRecognitionEngine != null)
            {
                this.speechRecognitionEngine.LoadGrammar(g);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(StartListening, TimeSpan.FromSeconds(1));
                //StartListening(null);
            }
            return registrationId;
        }

        public void RemoveSpeechCommands(Guid registrationId)
        {
            if (!IsInitialized)
            {
                throw new ServiceNotInitializedException(ExceptionStrings.SpeechRecognitionServiceNotInitialized);
            }

            var toRemoveItems = (from item in this.grammarCollection
                                 where item.RegistrationId == registrationId
                                 select item).ToList();

            foreach (var registeredGrammar in toRemoveItems)
            {
                var grammar = registeredGrammar.Grammar;
                this.grammarCollection.Remove(registeredGrammar);
                if (this.speechRecognitionEngine != null)
                {
                    this.speechRecognitionEngine.UnloadGrammar(grammar);
                }
            }
        }

        public void RemoveAllSpeechCommands(Guid creatorId)
        {
            if (!IsInitialized)
            {
                throw new ServiceNotInitializedException(ExceptionStrings.SpeechRecognitionServiceNotInitialized);
            }

            var toRemoveItems = (from item in this.grammarCollection
                                 where item.CreatorId == creatorId
                                 select item).ToList();

            foreach (var registeredGrammar in toRemoveItems)
            {
                var grammar = registeredGrammar.Grammar;
                this.grammarCollection.Remove(registeredGrammar);
                if (this.speechRecognitionEngine != null)
                {
                    this.speechRecognitionEngine.UnloadGrammar(grammar);
                }
            }
        }

        public void Dispose()
        {
            if (this.speechRecognitionEngine != null)
            {
                this.speechRecognitionEngine.RecognizeAsyncStop();
                this.speechRecognitionEngine.SetInputToNull();
                this.speechRecognitionEngine.Dispose();
            }
            if (this.audioSource != null)
            {
                this.audioSource.Stop();
            }
        }

        public void AddSpeechRecognizedListener(Action<ISpeechRecognitionPayload> onSpeechRecognizedAction)
        {
            var hashCode = onSpeechRecognizedAction.GetHashCode();
            var listener = speechRecognitionSubject.Subscribe(onSpeechRecognizedAction);
            speechRecognizedListeners[hashCode] = listener;
        }

        public void RemoveSpeechRecognizedListener(Action<ISpeechRecognitionPayload> onSpeechRecognizedAction)
        {
            var hashCode = onSpeechRecognizedAction.GetHashCode();
            if (speechRecognizedListeners.ContainsKey(hashCode))
            {
                var listener = speechRecognizedListeners[hashCode];
                speechRecognizedListeners.Remove(hashCode);
                listener.Dispose();
            }
        }

        public void AddCommandListeningStatusListener(Action<bool> onListeningStatusChangedAction)
        {
            var hashCode = onListeningStatusChangedAction.GetHashCode();
            var listener = this.listeningStatusSubject.Subscribe(onListeningStatusChangedAction);
            statusListeners[hashCode] = listener;
        }

        public void RemoveCommandListeningStatusListener(Action<bool> onListeningStatusChangedAction)
        {
            var hashCode = onListeningStatusChangedAction.GetHashCode();
            if (statusListeners.ContainsKey(hashCode))
            {
                var listener = statusListeners[hashCode];
                statusListeners.Remove(hashCode);
                listener.Dispose();
            }
        }

        #endregion ISpeechRecognitionService Members

        #region Private Methods

        private void StartListening(Object state)
        {
            if (state is TimeSpan)
            {
                var delay = (TimeSpan)state;
                Thread.Sleep(delay);
            }
            var monitorThread = new Thread(MonitorIsListeningToCommands);
            monitorThread.IsBackground = true;
            monitorThread.Name = IsListeningToCommandsMonitorThreadName;
            monitorThread.Start();

            this.speechRecognitionEngine = new SpeechRecognitionEngine(speechRecognizer.Id);

            this.speechRecognitionEngine.LoadGrammar(GetApplicationNameGramar());
            foreach (var item in this.grammarCollection)
            {
                this.speechRecognitionEngine.LoadGrammar(item.Grammar);
            }
            this.speechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;
            this.speechRecognitionEngine.SpeechHypothesized += OnSpeechHypothesized;
            this.speechRecognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;

            audioSourceStream = this.audioSource.Start();
            this.speechRecognitionEngine.SetInputToAudioStream(
                audioSourceStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            this.speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private RecognizerInfo GetKinectSpeechRecognizer(String culture = EnglishUnitedStatesCulture)
        {
            var recognizer = (from item in SpeechRecognitionEngine.InstalledRecognizers()
                              where
                                 item.AdditionalInfo.ContainsKey(KinectSpeechRecognizerFlag) &&
                                 String.Compare(Boolean.TrueString, item.AdditionalInfo[KinectSpeechRecognizerFlag], true) == 0 &&
                                 culture.Equals(item.Culture.Name, StringComparison.InvariantCultureIgnoreCase)
                              select item).FirstOrDefault();

            if (recognizer == null)
            {
                throw new SpeechRecognitionEngineNotFoundException(String.Format(ExceptionStrings.SpeechRecognitionEngineNotFound, culture));
            }
            return recognizer;
        }

        private Grammar GetApplicationNameGramar()
        {
            var applicationNameGrammarBuilder = new GrammarBuilder() { Culture = CultureInfo.GetCultureInfo("en-US") };
            var choices = new Choices();

            //foreach (var item in ApplicationNames.AvailableApplicationNames)
            //{
            if (!String.IsNullOrEmpty(applicationName))
            {
                choices.Add(applicationName);
            }
            //c
            applicationNameGrammarBuilder.Append(choices);
            var applicationNameGrammar = new Grammar(applicationNameGrammarBuilder);
            return applicationNameGrammar;
        }

        private void MonitorIsListeningToCommands()
        {
            var IsListeningToCommandsWaitTime = TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if (IsListeningToCommandsSetTime + IsListeningToCommandsWaitTime <= DateTime.Now && IsListeningToCommands)
                {
                    IsListeningToCommands = false;
                }
            }
        }

        #endregion Private Methods

        #endregion Methods

        #region Speech Recognition Event Handlers

        private void OnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (!isRunning) { return; }
            //Debug.WriteLine("\nSpeech Rejected");
            //if (e.Result != null)
            //{
            //    DumpRecordedAudio(e.Result.Audio);
            //}
        }

        private void OnSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            if (!isRunning) { return; }
            IsListeningToCommandsSetTime = DateTime.Now;
            Debug.Write("\rSpeech Hypothesized: \t" + e.Result.Text);
        }

        private void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!isRunning) { return; }
            if (e.Result.Confidence >= 0.55)
            {
                //DumpRecordedAudio(e.Result.Audio, e.Result.Text);
                Debug.WriteLine("\nSpeech Recognized: \t{0}\tConfidence:\t{1}", e.Result.Text, e.Result.Confidence);
                if (String.IsNullOrEmpty(applicationName))
                {
                    this.IsListeningToCommands = true;
                }
                else if (applicationName.Equals(e.Result.Text, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.IsListeningToCommands = true;
                    return;
                }
                if (!this.IsListeningToCommands) { return; }
                var argument = e.Result.Words.Skip(1).Aggregate<RecognizedWordUnit, String, String>("", (current, next) => current + " " + next.Text, (result) => result);
                var speechCommand = e.Result.Words[0].Text;
                var command = (from item in this.grammarCollection
                               where String.Compare(speechCommand, item.Command, true) == 0
                               select item);
                speechRecognitionSubject.OnNext(new SpeechRecognitionPayload()
                {
                    Text = e.Result.Text,
                    Confidence = e.Result.Confidence,
                    Command = speechCommand,
                    Argument = argument,
                });
                this.IsListeningToCommands = false;
            }
            else
            {
                Debug.WriteLine("\nSpeech Recognized but confidence was too low: \t{0}", e.Result.Confidence);
                //DumpRecordedAudio(e.Result.Audio);
            }
        }

        #endregion Speech Recognition Event Handlers

        //private static void DumpRecordedAudio(RecognizedAudio audio, String word = "")
        //{
        //    if (audio == null)
        //    {
        //        return;
        //    }

        //    int fileId = 0;
        //    string filename;
        //    while (File.Exists((filename = "RetainedAudio_" + word + "_" + fileId + ".wav")))
        //    {
        //        fileId++;
        //    }

        //    Debug.WriteLine("\nWriting file: {0}", filename);
        //    using (var file = new FileStream(filename, System.IO.FileMode.CreateNew))
        //    {
        //        audio.WriteToWaveStream(file);
        //    }
        //}

        public IUnityContainer Container { get; set; }
    }
}