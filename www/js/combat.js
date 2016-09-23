function CombatSession(protagonist, antagonists) {
    var currentRound = 0;
    return {
        protagonist: protagonist,
        antagonists: antagonists,
        currentRound: currentRound
    }
}

function CombatViewModel() {
    var self = this;

    var player = new Player('Nausicae');

    var antagonists = [];
    antagonists.push(new Radiyote());

    var combatSession = new CombatSession(player, antagonists);

    var readyToPlay = ko.computed(function() {

    });

    var playRound = function() {

    };

    return {
        protagonist: combatSession.protagonist,
        antagonists: combatSession.antagonists
    };
}