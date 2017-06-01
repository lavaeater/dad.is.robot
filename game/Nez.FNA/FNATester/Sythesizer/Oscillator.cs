using System;


namespace SimpleSynth.Synthesizer
{
	public static class Oscillator
	{
		public static float sin( float frequency, float time )
		{
			return (float)Math.Sin( frequency * time * 2 * Math.PI );
		}


		public static float square( float frequency, float time )
		{
			return sin( frequency, time ) >= 0 ? 1.0f : -1.0f;
		}


		public static float sawtooth( float frequency, float time )
		{
			return (float)( 2 * ( time * frequency - Math.Floor( time * frequency + 0.5 ) ) );
		}


		public static float triangle( float frequency, float time )
		{
			return Math.Abs( sawtooth( frequency, time ) ) * 2.0f - 1.0f;
		}
	}
}
