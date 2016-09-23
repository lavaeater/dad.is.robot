function WorldViewModel() {
    var self = this;
    self.worldSize = 10;
    self.world = new World(self.worldSize, 100);
    self.playerX = ko.observable(0);
    self.playerY = ko.observable(0);

    self.movePlayer = function (stepX, stepY) {
        self.playerX(self.playerX() + stepX);
        self.playerY(self.playerY() + stepY);
    };

    self.buttonClicked = function (key) {
        if (key == "s") {
            self.movePlayer(0, 1);
        }
        if (key == "w") {
            self.movePlayer(0, -1);
        }
        if (key == "a") {
            self.movePlayer(-1, 0);
        }
        if (key == "d") {
            self.movePlayer(1, 0);
        }
    };

    self.player2Tile = function (coord) {
        return Math.floor(coord / 10);
    };

    self.currentTileX = ko.computed(function () {
        return self.player2Tile(self.playerX());
    });

    self.currentTileY = ko.computed(function () {
        return self.player2Tile(self.playerY());
    });

    self.player2ViewPortX = ko.computed(function () {
        return self.playerX() - (self.currentTileX() * 10);
    });

    self.player2ViewPortY = ko.computed(function () {
        return self.playerY() - (self.currentTileY() * 10);
    });

    self.isPlayerHere = function (x, y) {
        return self.player2ViewPortX() == x() && self.player2ViewPortY() == y();
    };

    self.playerPos = ko.computed(function () {
        return KeyToString(self.playerX(), self.playerY());
    });

    self.currentTile = ko.computed(function () {
        return self.world.getTileAt(self.currentTileX(), self.currentTileY());
    });

    self.mapRows = ko.computed(function () {
        return self.currentTile().getMap();
    });

    self.currentTileColor = function (tileType) {
        if (tileType === 'w') {
            return 'blue';
        }
        if (tileType === 'g') {
            return 'green';
        }
        if (tileType === 'f') {
            return 'darkgreen';
        }
        if (tileType === 'd') {
            return 'yellow';
        }
        if (tileType === 'm') {
            return 'grey';
        }
    };

    self.keyup = function (data, event) {
        if (event.key == 'w' || event.key == 'a' || event.key == 's' || event.key == 'd') {
            self.buttonClicked(event.key);
        }
    };

    self.subTiles = {};

    self.getTileUnderPlayer = ko.computed(function () {
        //Simply get the tile currently Under the player and save it to the "cache" for later use
        var key = KeyToString(self.playerX(), self.playerY());
        if (!_.has(self.tiles, key)) {
            self.subTiles[key] = self.currentTile().subTiles[key];
        }
        return self.subTiles[key];
    });

    self.encounterGenerator = new EncounterEngine();

    self.eventInfo = ko.computed(function () {
        //generate events etc, using eventengine / questengine. Yay! For now, return notimpl
        return ko.toJSON(self.encounterGenerator.getEncounter(self.getTileUnderPlayer()));
    });

    return {
        buttonClicked: self.buttonClicked,
        playerPos: self.playerPos,
        currentTile: self.currentTile,
        mapRows: self.mapRows,
        currentTileColor: self.currentTileColor,
        keyup: self.keyup,
        currentTileX: self.currentTileX,
        currentTileY: self.currentTileY,
        player2ViewPortX: self.player2ViewPortX,
        player2ViewPortY: self.player2ViewPortY,
        isPlayerHere: self.isPlayerHere,
        eventInfo: self.eventInfo,
        tileUnderPlayer: self.getTileUnderPlayer
    };
}