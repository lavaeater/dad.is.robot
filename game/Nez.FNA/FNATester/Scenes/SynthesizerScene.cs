using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.TextureAtlases;


namespace FNATester
{
	public class SynthesizerScene : Scene
	{
		public override void initialize()
		{
			addRenderer( new DefaultRenderer() );
			clearColor = Color.Black;

			var textureAtlas = content.Load<TextureAtlas>( Content.TextureAtlasTest.atlasImages );
			var tex = textureAtlas.getSubtexture( "background" );

			// create Entities with Sprites
			var tree = createEntity( "sprite" );
			tree.addComponent( new Sprite( tex ) )
			    .addComponent<Synthesizer>()
				.transform.setPosition( Screen.center );
		}
	}
}

