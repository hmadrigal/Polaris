using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    /// <summary>
    /// When implemented, provides a mechanism to do speech recognition on a simplified approach oriented to commands.
    /// </summary>
    public interface ISpeechRecognitionService : IDisposable
    {
        /// <summary>
        /// Initializes the speech recognition service.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts the speech recognition service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the speech recognition service.
        /// </summary>
        void Stop();

        /// <summary>
        /// Adds a set of words or phrases to the set of grammars for the recognition service.
        /// </summary>
        /// <param name="commandSet">Set of phrases to be added</param>
        Guid AddSpeechCommandSet(string command, string[] commandArguments, Guid creatorId);

        /// <summary>
        /// Removes a set of words or phrases that were previously added to the Recognition service.
        /// </summary>
        /// <param name="speechCommandSetId">Global unique identifier of the command set</param>
        void RemoveSpeechCommands(Guid registrationId);

        void RemoveAllSpeechCommands(Guid creatorId);

        void AddSpeechRecognizedListener(Action<ISpeechRecognitionPayload> onSpeechRecognizedAction);

        void RemoveSpeechRecognizedListener(Action<ISpeechRecognitionPayload> onSpeechRecognizedAction);

        void AddCommandListeningStatusListener(Action<Boolean> onListeningStatusChangedAction);

        void RemoveCommandListeningStatusListener(Action<Boolean> onListeningStatusChangedAction);

        bool IsListeningToCommands { get; }
    }
}