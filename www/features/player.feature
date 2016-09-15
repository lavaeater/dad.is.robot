Feature: Moving around
    As a a player of the Game
    I want to be able to move about
    So that I can explore the game world

    Scenario: Accelerating the player
         Given that the player is standing still
            And acceleration is 0.5
         When I hit accelerate
         Then the players speed should increase by 0.5