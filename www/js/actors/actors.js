function Actor(name, attributes, attacks, defenses) {
    return _.assign({ name: name }, attributes, attacks, defenses);
}

function Action(name, type) {
    return {
        name: name,
        type: type
    };
}