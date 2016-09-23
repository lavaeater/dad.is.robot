function CombatViewModel() {
    var self = this;

    var player = new Player('Nausicae');

    var antagonists = [];
    antagonists.push(new Radiyote());

    var readyToPlay = ko.computed(function() 
    );

    return {
        protagonist: player,
        antagonists: antagonists
    };
}