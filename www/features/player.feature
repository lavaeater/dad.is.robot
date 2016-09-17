Feature: Moving around
    As a a player of the Game
    I want to be able to move about
    So that I can explore the game world

    Scenario: Accelerating the player
         Given a player that is standing still
            And acceleration is 1
         When I hit accelerate
         Then the players speed should be 2