var charEditor;
var cursors;
var game = new Phaser.Game(500, 500, Phaser.AUTO, 'phaser-example', { preload: preload, create: create, update: update });

function preload() {
    //  You can fill the preloader with as many assets as your game requires

    //  Here we are loading an image. The first parameter is the unique
    //  string by which we'll identify the image later in our code.

    //  The second parameter is the URL of the image (relative)
    //game.load.image('charedit2', 'assets/img/char_edit_prototype002.png');
    game.load.image('charedit1', 'assets/img/char_edit_prototype003.png');

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
    var charEditSprite = game.add.sprite(0, 0, 'charedit1');
    var eyes = [game.add.sprite(0,0, 'eyes01'), game.add.sprite(0,0, 'eyes02')];


    //all sprites in one array for easy scaling etc.
    var allSprites = [charEditSprite, eyes[0], eyes[1]];

    _.forEach(allSprites, function(sprite)  {
      sprite.scale.setTo(0.25,0.25);
    });

    _.forEach(eyes, function(eye) {
      eye.visible = false;
    });

    cursors = game.input.keyboard.createCursorKeys();



    charEditor = new charEdit(charEditSprite, eyes);
}

function update() {

}

var charEdit = function(charEditSprite, eyes) {
  var self = this;
  self.offsetX = 100;
  self.offsetY = 0;
  self.charEditSprite = charEditSprite;
  self.eyes = eyes;

  //Event handlers for keys

/*
Create array of arrays for different parts, this array is
controlled by up-down keys to change if fix face etc...

Also, figure out how to save the character for later!
*/

  self.selectedEyeIndex = 0;
  self.selectedEyeSprite = eyes[self.selectedEyeIndex];

  self.indexUp = function(indexVar, arrayForIndex) {
    self[indexVar]++;
    if(self[indexVar] > arrayForIndex.length - 1 )
    {
      self[indexVar] = 0;
    }
  };

  self.indexDown = function(indexVar, arrayForIndex) {
    self[indexVar]--;
    if(self[indexVar] < 0)
    {
      self[indexVar] = arrayForIndex.length - 1;
    }
  };

  self.eyeLeftClick = function() {
    self.indexDown('selectedEyeIndex', self.eyes);
    self.updateSelectedEyeSprite();
  };

  self.eyeRightClick = function() {
    self.indexUp('selectedEyeIndex', self.eyes);
    self.updateSelectedEyeSprite();
  };

  self.updateSelectedEyeSprite = function() {
    //Todo: rewrite as a "generic" function to update any sprite in this editor
    self.selectedEyeSprite.visible = false;
    self.selectedEyeSprite = self.eyes[self.selectedEyeIndex];
    self.selectedEyeSprite.visible = true;
  };


  cursors.left.onDown.add(self.eyeLeftClick, self);
  cursors.right.onDown.add(self.eyeRightClick, self);

  self.updateSelectedEyeSprite();

  return {
    eyeLeftClick: self.eyeLeftClick,
    eyeRightClick: self.eyeRightClick
  };
};
