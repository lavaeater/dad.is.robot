function ViewModel() {
    var self = this;
    self.worldSize = 10;
    self.world = new World(self.worldSize, 100);
    self.playerX = ko.observable(0);
    self.playerY = ko.observable(0);

    self.movePlayer = function(stepX, stepY) {
        self.playerX(self.playerX() + stepX);
        self.playerY(self.playerY() + stepY);
    };    

    self.buttonClicked = function(key) {
        if(key == "w") {
            self.movePlayer(0, 1);
        }
        if(key == "s") {
            self.movePlayer(0, -1);
        }
        if(key == "a") {
            self.movePlayer(-1, 0);
        }
        if(key == "d") {
            self.movePlayer(1, 0);
        }
    };

    self.player2World = function(coord) {
        if(coord < 0) {
            return Math.ceil(coord / 10);
        }
        if(coord >= 0) {
            return Math.floor(coord / 10);
        }
    };

    self.playerPos = ko.computed(function() {
        return KeyToString(self.playerX(), self.playerY());
    });

    self.currentTile = ko.computed(function() {
        return self.world.getTileAt(self.player2World(self.playerX()), self.player2World(self.playerY()));
    });

    self.mapRows = ko.computed(function() {
        return self.currentTile().getMap();
    });

    self.currentTileColor = function(tileType) {
        if(tileType === 'w') {
            return 'blue';
        }
        if(tileType === 'g') {
            return 'green';
        }
        if(tileType === 'f') {
            return 'darkgreen';
        }
        if(tileType === 'd') {
            return 'yellow';
        }
        if(tileType === 'm') {
            return 'grey';
        }
    };

    self.keyup = function(data, event) {
        if(event.key == 'w' || event.key == 'a' || event.key == 's' || event.key == 'd') {
            self.buttonClicked(event.key);
        }
    };

    return {
        buttonClicked: self.buttonClicked,
        playerPos: self.playerPos,
        currentTile: self.currentTile,
        mapRows: self.mapRows,
        currentTileColor: self.currentTileColor,
        keyup: self.keyup
    };
};