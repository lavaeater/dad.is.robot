var game = new Phaser.Game(800, 600, Phaser.AUTO, 'phaser-example', { preload: preload, create: create });

function preload() {
    //  You can fill the preloader with as many assets as your game requires

    //  Here we are loading an image. The first parameter is the unique
    //  string by which we'll identify the image later in our code.

    //  The second parameter is the URL of the image (relative)
    game.load.image('charedit2', 'assets/img/char_edit_prototype002.png');
    game.load.image('charedit1', 'assets/img/char_edit_prototype001.png');

    //create an array of asset names for faces, for instance.
    game.load.image('eyes01', 'assets/img/eyes01.png');
    game.load.image('eyes02', 'assets/img/eyes02.png');

    /*
      How do we keep track of the necessary offsets? They could be stored as some data OR all sprites pertaing to head, for instance,
      could be the exact same size.
    */
}

function create() {
    //  This creates a simple sprite that is using our loaded image and
    //  displays it on-screen
    var charEditSprite = game.add.sprite(0, 0, 'charedit2');
    var eyes = [game.add.sprite(0,0, 'eyes01'), game.add.sprite(0,0, 'eyes02')];

    var charEditor = new charEdit(charEditSprite, eyes);

}

var charEdit = function(charEditSprite, eyes) {
  var self = this;
  self.charEditSprite = charEditSprite;
  self.eyes = eyes;

  self.selectedEyeIndex = 0;
  self.selectedEyeSprite = eyes[self.selectedEyeIndex];

  self.indexUp = function(indexVar, arrayForIndex) {
    indexVar++;
    if(indexVar > arrayForIndex.length - 1 )
    {
      indexVar = 0;
    }
  };

  self.indexDown = function(indexVar, arrayForIndex) {
    indexVar--;
    if(indexVar < 0)
    {
      indexVar = arrayForIndex.length - 1;
    }
  };

  self.eyeLeftClick = function() {
    self.indexDown(self.selectedEyeIndex, self.eyes);
  };

  self.eyeRightClick = function() {
    self.indexUp(self.selectedEyeIndex, self.eyes);
  };

  self.updateSelectedEyeSprite = function() {
    //Todo: rewrite as a "generic" function to update any sprite in this editor
    self.selectedEyeSprite.visible = false;
    self.selectedEyeSprite = self.eyes[self.selectedEyeIndex];
    self.selectedEyeSprite.visible = true;
  };




  /*
  Vi försöker med något slags generisk för ansiktsbilderna... vi har assets för faces
  */

  return {

  };
};
