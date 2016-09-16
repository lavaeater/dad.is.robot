var assert = require('cucumber-assert');
var Player = require('../../js/player');

module.exports = function () {
    var player = new Player(0, 0, 0, 0.5, 0, {});;

    this.World = function() {
        this.player = player;
    };

    this.Given(/^a player that is standing still$/, function (callback) {
        // Write code here that turns the phrase above into concrete actions
        console.log("this.player: ");
        console.log(this.player);
        console.log("this: ");
        console.log(this);
        console.log("this.player.speed: ");
        console.log(this.player.speed);
        assert.equal(this.player.speed, 0, callback);
    });

    this.Given(/^acceleration is (\d+)\.(\d+)$/, function (arg1, arg2, callback) {
        // Write code here that turns the phrase above into concrete actions
        callback(null, 'pending');
    });

    this.When(/^I hit accelerate$/, function (callback) {
        // Write code here that turns the phrase above into concrete actions
        callback(null, 'pending');
    });

    this.Then(/^the players speed should increase by (\d+)\.(\d+)$/, function (arg1, arg2, callback) {
        // Write code here that turns the phrase above into concrete actions
        callback(null, 'pending');
    });
}