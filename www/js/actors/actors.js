function Attributes(strength, agility, intelligence, life) {
    return {
        strength: strength,
        agility: agility,
        intelligence: intelligence,
        life: life
    };
}

function Actor(name, attributes, actions, perks) {
    return _.assign({ name: name, actions: actions, perks: perks }, attributes);
}

function Action(name, type, modifiers, props) {
    return {
        name: name,
        type: type,
        modifiers: modifiers,
        props: props
    };
}

function Perk(name, modifiers) {
    return {
        name: name,
        modifiers: modifiers
    };
}

function CommonActions() {
    var actions = {};
    return actions;
}

function Player(name) {
    //Just returns a new player object with some attributes and stuffs;

    var attrs = new Attributes(10, 10, 10, 10);
    var actions = []; // a list of possible actions for this character
    actions.push(new Action('Stab', 'attack', { attack: 1 }, {}));
    actions.push(new Action('Slice', 'attack', { attack: 2, defense: -1, damage: 1 }, {}));
    actions.push(new Action('Defend', 'defense', { defense: 3 }, {}));
    actions.push(new Action('Run away', 'exit', { defense: -1 }, { exclusive: true }));

    var perks = [];
    perks.push(new Perk('Defensive stance', {attack: -3, defense: 3}));

    return new Actor(name, attrs, actions, perks);
}

function Radiyote() {
    var attrs = new Attributes(15, 12, 3, 8);
    var actions = [];
    actions.push(new Action('Bite', 'attack', { attack: 2, defense: -1, damage: 1 }));
    actions.push(new Action('Claw', 'attack', { attack: 1, damage: -2 }));
    actions.push(new Action('Run away', 'exit', { defense: -1 }, { exclusive: true }));
    var perks = [];
    perks.push(new Perk('Cornered animal', {attack: 3, defense: -3}));

    return new Actor('Radiyote', attrs, actions, perks);
}