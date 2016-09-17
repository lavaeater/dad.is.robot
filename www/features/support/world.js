var Player = require('../../js/Player');

function World() {
    this.player = new Player(0,0,0,1,0,{});
};

module.exports = function() {
    this.World = World;
}

