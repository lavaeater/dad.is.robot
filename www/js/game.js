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

    game.load.image('goggles01', 'assets/img/goggles01.png');
    game.load.image('goggles02', 'assets/img/goggles02.png');
    game.load.image('goggles03', 'assets/img/goggles03.png');

    game.load.image('upper01', 'assets/img/upper01.png');
    game.load.image('upper02', 'assets/img/upper02.png');

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
    var goggles = [game.add.sprite(0,0, 'goggles01'), game.add.sprite(0,0, 'goggles02'), game.add.sprite(0,0, 'goggles03')];
    var tops = [game.add.sprite(0,0, 'upper01'), game.add.sprite(0,0, 'upper02')];
    //all sprites in one array for easy scaling etc.
    var allSprites = [charEditSprite, eyes[0], eyes[1], goggles[0], goggles[1], goggles[2], tops[0], tops[1]];

    _.forEach(allSprites, function(sprite)  {
      sprite.scale.setTo(0.25,0.25);
    });

    _.forEach(eyes, function(eye) {
      eye.visible = false;
    });

    _.forEach(goggles, function(goggle) {
      goggle.visible = false;
    });

    _.forEach(tops, function(top) {
      top.visible = false;
    });

    cursors = game.input.keyboard.createCursorKeys();

    charEditor = new charEdit(charEditSprite, eyes, goggles, tops);
}

function update() {

}

var charEdit = function(charEditSprite, eyes, goggles, tops) {
  var self = this;
  self.offsetX = 100;
  self.offsetY = 0;
  self.charEditSprite = charEditSprite;
  self.parts = [eyes, goggles, tops];
  self.selectedIndexes = [0,2,0];
  self.partIndex = 0;

/*
Create array of arrays for different parts, this array is
controlled by up-down keys to change if fix face etc...

Also, figure out how to save the character for later!
*/

self.hidePartAt = function(partIndex, selectedIndex) {
  self.parts[partIndex][selectedIndex].visible = false;
};

self.showPartAt = function(partIndex, selectedIndex) {
  self.parts[partIndex][selectedIndex].visible = true;
};

  self.upClick = function() {
    self.partIndex++;
    if(self.partIndex > self.parts.length - 1) {
      self.partIndex = 0;
    }
  };

  self.downClick = function() {
    self.partIndex--;
    if(self.partIndex < 0) {
      self.partIndex = self.parts.length - 1;
    }
  };

  //Event handlers for keys
  self.leftClick = function() {
    //Hide current item
    self.hidePartAt(self.partIndex, self.selectedIndexes[self.partIndex]);
    //change selectedIndex at partIndex
    self.selectedIndexes[self.partIndex]--;
    if(self.selectedIndexes[self.partIndex] < 0) {
      self.selectedIndexes[self.partIndex] = self.parts[self.partIndex].length - 1;
    }
    //show current item
    self.showPartAt(self.partIndex, self.selectedIndexes[self.partIndex]);
  };

  self.rightClick = function() {
    //Hide current item
    self.hidePartAt(self.partIndex, self.selectedIndexes[self.partIndex]);
    //change selectedIndex at partIndex
    self.selectedIndexes[self.partIndex]++;
    if(self.selectedIndexes[self.partIndex] > self.parts[self.partIndex].length - 1) {
      self.selectedIndexes[self.partIndex] = 0;
    }
    //show current item
    self.showPartAt(self.partIndex, self.selectedIndexes[self.partIndex]);

  };

  cursors.left.onDown.add(self.leftClick, self);
  cursors.right.onDown.add(self.rightClick, self);

  cursors.up.onDown.add(self.upClick, self);
  cursors.down.onDown.add(self.downClick, self);

  self.showPartAt(0,0);

  return {
  };
};
