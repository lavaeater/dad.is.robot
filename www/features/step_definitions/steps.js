var assert = require('cucumber-assert');

module.exports = function () {

    this.Given(/^a player that is standing still$/, function (callback) {
        assert.equal(this.player.speed, 0, callback);
    });

    this.Given(/^acceleration is (\d+)$/, function (arg1, callback) {
        assert.equal(this.player.acceleration, arg1, callback)
    });

    this.When(/^I hit accelerate$/, function (callback) {
        this.player.accelerate();
        callback();
    });

    this.Then(/^the players speed should be (\d+)$/, function (arg1, callback) {
        assert.equal(this.player.speed, arg1, callback())
    });
}