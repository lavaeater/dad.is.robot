function Player (x, y, speed, acceleration, direction, sprite) {
    var self = this;

    self.x = x;
    self.y = y;
    self.speed = speed;
    self.direction = direction;
    self.sprite = sprite;
    self.acceleration = acceleration;

    self.accelerate = function () {
        self.speed += self.acceleration;
    };
};

module.exports = Player;