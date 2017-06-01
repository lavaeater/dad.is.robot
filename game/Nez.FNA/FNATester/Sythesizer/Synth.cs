using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Nez;

namespace SimpleSynth.Synthesizer
{
	/// <summary>
	/// More or less a direct port of David Gouveia's SimpleSynth with some modifications: https://github.com/davidluzgouveia/SimpleSynth
	/// </summary>
	public class Synth : IDisposable
	{
		// For Debugging
		public int activeVoicesCount { get { return _activeVoices.Count; } }
		public int freeVoicesCount { get { return _freeVoices.Count; } }
		public int keyRegistryCount { get { return _keyRegistry.Count; } }

		// Synth Parameters
		public const int channels = 1;
		public const int sampleRate = 44100;
		public const int samplesPerBuffer = 2000; // Tweak this for latency
		public const int polyphony = 32;
		public int fadeInDuration = 200;
		public int fadeOutDuration = 200;
		public Func<float,float,float> oscillator = Synthesizer.Oscillator.sin;

		DynamicSoundEffectInstance _instance;
		readonly byte[] _xnaBuffer;
		readonly float[,] _workingBuffer;
		readonly Voice[] _voicePool;
		readonly List<Voice> _activeVoices;
		readonly Stack<Voice> _freeVoices;
		readonly Dictionary<int,Voice> _keyRegistry;


		public Synth()
		{
			// Create buffers
			const int bytesPerSample = 2;
			_xnaBuffer = new byte[channels * samplesPerBuffer * bytesPerSample];
			_workingBuffer = new float[channels, samplesPerBuffer];

			// Create voice structures
			_voicePool = new Voice[polyphony];
			for( var i = 0; i < polyphony; ++i )
				_voicePool[i] = new Voice( this );
			
			_freeVoices = new Stack<Voice>( _voicePool );
			_activeVoices = new List<Voice>();
			_keyRegistry = new Dictionary<int, Voice>();

			// Create DynamicSoundEffectInstance object and start it
			_instance = new DynamicSoundEffectInstance( sampleRate, channels == 2 ? AudioChannels.Stereo : AudioChannels.Mono );
			_instance.BufferNeeded += onBufferNeeded;
			_instance.Play();
		}


		void onBufferNeeded( object sender, EventArgs e )
		{
			submitBuffer();
		}


		public void noteOn( int note )
		{
			if( _keyRegistry.ContainsKey( note ) )
				return;

			var freeVoice = getFreeVoice();
			if( freeVoice == null )
				return;

			_keyRegistry[note] = freeVoice;
			freeVoice.start( SoundHelper.noteToFrequency( note ) );
			_activeVoices.Add( freeVoice );
		}


		public void noteOff( int note )
		{
			Voice voice;
			if( _keyRegistry.TryGetValue( note, out voice ) )
			{
				voice.stop();
				_keyRegistry.Remove( note );
			}
		}


		void submitBuffer()
		{
			// clear the buffer before filling it
			Array.Clear( _workingBuffer, 0, channels * samplesPerBuffer );
			fillWorkingBuffer();
			SoundHelper.convertBuffer( _workingBuffer, _xnaBuffer );
			_instance.SubmitBuffer( _xnaBuffer );
		}


		void fillWorkingBuffer()
		{
			// Call Process on all active voices
			for( var i = _activeVoices.Count - 1; i >= 0; --i )
			{
				Voice voice = _activeVoices[i];
				voice.process( _workingBuffer );

				// Remove any voices that stopped being active
				if( !voice.IsAlive )
				{
					_activeVoices.RemoveAt( i );
					_freeVoices.Push( voice );
				}
			}
		}


		Voice getFreeVoice()
		{
			if( _freeVoices.Count == 0 )
				return null;

			return _freeVoices.Pop();
		}


		#region IDisposable Support

		~Synth()
		{
			(this as IDisposable).Dispose();
		}


		public void Dispose()
		{
			if( _instance != null )
			{
				_instance.Stop( true );
				_instance.Dispose();
				_instance = null;
			}
		}

		#endregion

	}
}
