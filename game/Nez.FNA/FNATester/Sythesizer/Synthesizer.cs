using Microsoft.Xna.Framework.Input;
using Nez;
using SimpleSynth.Synthesizer;


namespace FNATester
{
	public class Synthesizer : Component, IUpdatable
	{
		int _currentOscillator = 0;
		Synth _synth;


		public Synthesizer()
		{
			_synth = new Synth();
		}


		void IUpdatable.update()
		{
			checkNoteTrigger( Keys.A, 0 );
			checkNoteTrigger( Keys.W, 1 );
			checkNoteTrigger( Keys.S, 2 );
			checkNoteTrigger( Keys.E, 3 );
			checkNoteTrigger( Keys.D, 4 );
			checkNoteTrigger( Keys.F, 5 );
			checkNoteTrigger( Keys.T, 6 );
			checkNoteTrigger( Keys.R, 6 );
			checkNoteTrigger( Keys.G, 7 );
			checkNoteTrigger( Keys.Y, 8 );
			checkNoteTrigger( Keys.H, 9 );
			checkNoteTrigger( Keys.U, 10 );
			checkNoteTrigger( Keys.J, 11 );
			checkNoteTrigger( Keys.K, 12 );
			checkNoteTrigger( Keys.O, 13 );
			checkNoteTrigger( Keys.L, 14 );

			if( Input.isKeyPressed( Keys.Up ) )
			{
				_currentOscillator = Mathf.incrementWithWrap( _currentOscillator, 4 );
				updateOscillator();
			}

			if( Input.isKeyPressed( Keys.Down ) )
			{
				_currentOscillator = Mathf.decrementWithWrap( _currentOscillator, 4 );
				updateOscillator();
			}
		}


		public override void onRemovedFromEntity()
		{
			_synth.Dispose();
			_synth = null;
		}


		void updateOscillator()
		{
			switch( _currentOscillator )
			{
				case 0:
					_synth.oscillator = Oscillator.sin;
					break;
				case 1:
					_synth.oscillator = Oscillator.square;
					break;
				case 2:
					_synth.oscillator = Oscillator.triangle;
					break;
				case 3:
					_synth.oscillator = Oscillator.sawtooth;
					break;
			}
		}


		void checkNoteTrigger( Keys key, int note )
		{
			if( Input.isKeyPressed( key ) )
				_synth.noteOn( note );

			if( Input.isKeyReleased( key ) )
				_synth.noteOff( note );
		}
	
	}
}

