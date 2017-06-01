namespace SimpleSynth.Synthesizer
{
	public class Voice
	{
		public bool IsAlive { get; private set; }

		enum VoiceState { Attack, Sustain, Release }

		VoiceState _state;
		float _frequency;
		float _time;
		float _fadeMultiplier;
		int _fadeCounter;
		readonly Synth _synth;


		public Voice( Synth synth )
		{
			_synth = synth;
		}


		public void start( float frequency )
		{
			_frequency = frequency;
			_time = 0.0f;
			_fadeMultiplier = 0.0f;

			_fadeCounter = 0;

			if( _synth.fadeInDuration == 0 )
				_state = VoiceState.Sustain;
			else
				_state = VoiceState.Attack;

			IsAlive = true;
		}


		public void stop()
		{
			if( _synth.fadeOutDuration == 0 )
			{
				IsAlive = false;
			}
			else
			{
				_fadeCounter = (int)( ( 1.0f - _fadeMultiplier ) * _synth.fadeOutDuration );
				_state = VoiceState.Release;
			}
		}


		public void process( float[,] workingBuffer )
		{
			if( IsAlive )
			{
				var samplesPerBuffer = workingBuffer.GetLength( 1 );
				for( var i = 0; i < samplesPerBuffer; i++ )
				{
					if( _state == VoiceState.Attack )
					{
						_fadeMultiplier = (float)_fadeCounter / _synth.fadeInDuration;

						++_fadeCounter;
						if( _fadeCounter >= _synth.fadeInDuration )
							_state = VoiceState.Sustain;
					}
					else if( _state == VoiceState.Release )
					{
						_fadeMultiplier = 1.0f - (float)_fadeCounter / _synth.fadeOutDuration;

						++_fadeCounter;
						if( _fadeCounter >= _synth.fadeOutDuration )
						{
							IsAlive = false;
							return;
						}
					}
					else
					{
						_fadeMultiplier = 1.0f;
					}

					var sample = _synth.oscillator( _frequency, _time );
					workingBuffer[0, i] += sample * 0.2f * _fadeMultiplier;
					_time += 1.0f / Synth.sampleRate;
				}
			}
		}

	}
}