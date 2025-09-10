# Castle Crash

The Vectorfield and simulations can be inspected in the the Sample Scene.   
Cellular Automata and AI can be testet in the Small Board Scene.

To test the Cellular Automata, you have to start the game.  
After that wait till you have drawn some cards and play either of the following cards: Drow Ranger, Dragon Knight  
The AI has 3 States/Goals: Survival, Gain Board Controll, Kill the player.  
To achieve all 3 states please follow the instructions after starting the game:

- Kill the player: Dont have any units on the board.
- Gain Board Controll: Have more active units than the AI.
- Survival: Change the AI Health to 20 or less and have more units on the board.

The default setting for letting the AI act is by pressing spacebar or uncheck the box: "Plan on Click", that can be found in the Enemy Gameobject.  
For the purpose of convinience i added the needed data to the inspector.

## Asset Integration

- Models: Drow Ranger, Dragon Knight (DK)
- Owner: Valve
- Origin: Dota2
- Link to License: [Link](https://store.steampowered.com/subscriber_agreement#2)
- Important Part: Article 2 Paragraph D
- Usage Restriction: Non-Comercial

TLDR: "Free use for Fanart and none commercial."

### Known Issues

- Dragon knight has no Animations, as I exportet it without all it's actions baked in.
- Units might bug out after fighting. Could not figure it out why nor could I recreate the bug perfectly.
- A lot of the VFX code are in files that should not be there. Time pressure kept enticed me to do it.
- Performance issues when the board gets to large.
