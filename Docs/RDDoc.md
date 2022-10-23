<div align="center">

## Software Requirements and Design Document <br> <br> For Group #4 <br> <br> Version 0.2

<br>
<br>

### Authors:

**Daniel Pijeira**
<br>

**Lawrence Martinez**
<br>

**Nathan Granger**
<br>

**Tyler McLanahan**
<br>

**Tyler Pease**
<br>

</div>

<br>
<br>
<br>

### 1) Overview

In short, our *system* is a tower defense game. A tower defense game for the uninitiated is at its basics is a map with a path through it that enemies use to cross on, enemies that reach the end of the path take life from the player so the player has to spend their money to buy/build towers along the path to attempt to take out enemies before they reach the end, more enemies are generated each round. Our game though, unlike most, will allow the user to expand the map, lengthening the path enemies cross on.

### 2) Functional Requirements

1) High - The system lets the user be able to click play and pick a game mode.
2) High - The system lets the user be able to click quit and exit the game.
3) High - The system lets the user select any tower they want from the buy menu.
4) High - The system prevents the user from buying towers they can’t afford.
5) Medium - As the map expands the camera pans out.
6) Medium - The system lets towers target the nearest enemy in range.
7) Medium - When an enemy loses all their health it adds money to the users total money.
8) Medium - Bullets apply a set damage value to enemies.
9) Medium - Bullets update the status of enemies.
10) High - When an enemy makes it to the end tile the player loses a specified health.
11) High - When the player loses all health the game ends.
12) Medium - When an enemy gets hit by an elemental bullet the specified elemental effect is applied to the enemy.
13) Low - Make towers prioritize targeting enemies that aren’t already being targeted by other towers.
14) High - When playing in survival mode the map expands every 5 rounds.
15) Low - An animation is played when a tower shoots an enemy.
16) Medium - The map generation creates multiple left and right turns for path generation.
17) Medium - In story mode every five rounds a predetermined mini-boss/boss spawns.
18) Low - The buy menu lists all towers and their prices.
19) Low - The enemies have health bars informing the user how much damage they’ve taken.
20) Low - The system lets towers shoot in a 360 degree arc.
21) Low - The system lets towers shoot only in their specified range.
22) Medium - The system has enemies follow the random generated path.
23) High - The system detects a collision between bullets and enemies.
24) High - The system prevents towers from being placed on top of each other.
25) High - The system prevents towers from being placed in random generated paths.
26) High - The system ends the game when the user runs out of lives.
27) Medium - The system should let the user be able to save the current state of the game.
28) Low - The system should let the user be able to pause the current game.
29) Low - The system should let the user be able to fast forward the current game.
30) Medium - The system takes a specified amount of money out of the users total money when they buy a new tower.
31) Medium - The system lets the user upgrade a tower.
32) Medium - The system takes a specified amount of money out of the users total money when they upgrade a tower.
33) Low - The user can hover over a tower and see the range of their tower.
34) Medium - The system lets the user click on towers to see possible upgrades.
35) Low - The system lets users click on towers to see stats of the tower.
36) Medium - The system lets the user load a saved game from the main menu.
37) Medium - When the game starts it displays to the user in the main menu the options for Play game, Load game, Settings, and Quit.
38) Medium - The game over screen displays to the user Restart, Go to Main Menu, and Quit.
39) High - The generated map lets you place towers.
40) High - the generated map lets enemies spawn.
41) Medium - The system has a set range, cost, name, and damage for every tower.
42) Medium - The system has a set speed, health, kill reward, and damage for every enemy.
43) Low - The system allows towers to switch the enemy they are currently targeting.

### 3) Non-functional Requirements

1) All code should be well documented and easily readable to any developer.
2) Keep the number of calculations to a minimum so placing multiple towers doesn’t slow down the game.
3) The game should be able to run on Windows, Linux, and MacOs.
4) The game should be light on resources so anyone can run it.*
5) The game should have a fast response time/little time waiting for things to load.
6) The game should not crash when too many enemies/towers are on the map at the same time.
7) The game should support massive map sizes as some expansion games might go on for a while.
8) The game should have a simple to understand and clear UI that doesn’t distract as much from the main game.

### 4) Use Case Diagram

[![Use-Case-Diagram](/Docs/Diagrams/UseCaseDiagram.png)](/Docs/Diagrams/UseCaseDiagram.png)

### 5) Class Diagram and/or Sequence Diagrams

[![Class-Diagram](/Docs/Diagrams/ClassDiagram.png)](/Docs/Diagrams/ClassDiagram.png)

#### Map Generation Sequence Diagram

[![Sequence-Diagram_MapGen](/Docs/Diagrams/SequenceDiagram_MapGen.png)](/Docs/Diagrams/SequenceDiagram_MapGen.png)

#### Enemies Sequence Diagram

[![Sequence-Diagram_Enemies](/Docs/Diagrams/SequenceDiagram_Enemies.png)](/Docs/Diagrams/SequenceDiagram_Enemies.png)

#### Placement Sequence Diagram

[![Sequence-Diagram_PlaceMan](/Docs/Diagrams/SequenceDiagram_PlaceMan.png)](/Docs/Diagrams/SequenceDiagram_PlaceMan.png)

### 6) Operating Environment

Our game could, at its finished state, theoretically operate on any of the main operating systems: Windows, Linux, MacOS. Most machines hardware-wise will be able to handle running our game, though for best operation it would be suggested that the machine have at least 8GB Ram, and a processor equivalent or better than an Intel i5 or extended buffering/glitches might occur.

### 7) Assumptions and Dependencies
  
One of the dependencies for this project is the base code comes from a tutorial series off of YouTube to help ourselves start in learning Unity. Over the next two increments we should be able to move away from this code/change it almost completely.
