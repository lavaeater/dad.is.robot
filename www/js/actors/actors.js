function Actor(name, attributes, attacks, defenses) {
    return _.assign({name:name}, attributes, attacks, defenses);
}