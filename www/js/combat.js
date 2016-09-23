function CombatViewModel() {
    var self = this;

    var player = new Player('Nausicae');

    var antagonists = [];
    antagonists.push(new Radiyote());

    return {
        protagonist: player,
        antagonists: antagonists
    };
}