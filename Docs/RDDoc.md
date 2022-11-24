<div align="center">

## Software Requirements and Design Document <br> <br> For Group #4 <br> <br> Version 0.4

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
44) Medium - The system lets elemental towers apply status effects to enemies.
45) Medium - The system allows enemies to use abilities for a specified duration.
46) Medium - The system allows enemies to use abilities after a specified duration.
47) High - The system should display to the user the current amount of money.
48) High - The system should display to the user the current amount of health.
49) Low - The system should display kill rewards after enemies die.
50) Medium - The system should allow boons to be purchased from the shop.
51) Medium - Boons should last for the specified duration and be destroyed after.
52) Medium - Boons should remove all the effects and return the state of play to normal when destroyed.
53) Medium - Boons should remove effects when targets are out of range or add them when they are in range.
54) Medium - Towers should be able to support multiple boon effects at the same time.
55) Low - The system should let the user change difficulty.
56) Medium - The system should allow the user to speed up game time.
57) Medium - The system should contain various enemy types with different abilities and niches that make them unique.

### 3) Non-functional Requirements

1) All code should be well documented and easily readable to any developer.
2) Keep the number of calculations to a minimum so placing multiple towers doesn’t slow down the game.
3) The game should be able to run on Windows, Linux, and MacOs.
4) The game should be light on resources so anyone can run it.*
5) The game should have a fast response time/little time waiting for things to load.
6) The game should not crash when too many enemies/towers are on the map at the same time.
7) The game should support massive map sizes as some expansion games might go on for a while.
8) The game should have a simple to understand and clear UI that doesn’t distract as much from the main game.
9) The game should be difficult enough to not allow the player to live forever.

### 4) Use Case Diagram

<details name="Use-Case Diagram">
<summary>Use-Case Diagram</summary>

[![Use-Case-Diagram](/Docs/Diagrams/UseCaseDiagram.png)](/Docs/Diagrams/UseCaseDiagram.png)

</details>

Use Case: Playing an expansion game  
Actors: Player, System  
Pre condition: Launch the game  
Normal Flow: The player clicks start new expansion game  
Post condition: A new round is started along with a generating a map  
Alternative Flows: When a game ends the player can choose to start again  
Nonfunctional Requirements: The system is reliable and light on resources to quickly start a game


Use Case: Place a tower  
Actors: Player, System, Shop manager, Money manager  
Pre condition: The player has enough money to buy a tower  
Normal Flow: The player uses the system UI to select a tower and place it on a tile on the map. The system then takes away money from the user after the tower is placed  
Post condition: A new object for a tower is created on the selected tile and begins to track and attack enemies.  
Alternative Flows: The player doesn’t have enough money to place a tower and the system informs them of lack of money. Another case is the player doesn’t want to place a tower.  
Nonfunctional Requirements: The system is reliable and light on resources

Use Case: Spawn enemies  
Actors: System, Round controller, Enemy system  
Pre condition: The system has started a new round  
Normal Flow: After a round starts the round controller spawns enemies at a spawn portal.  
Post condition: Enemies move along a path to attack player home  
Alternative Flows: An enemy has a special ability to spawn new enemies and creates at their current tile location. They then follow along the path as regular enemies.  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Attack enemies  
Actors: System, Tower System  
Pre condition: A tower is currently placed on a valid tile in a round  
Normal Flow: A tower detects enemies and begins to use BarrelRotation to turn towards an enemy and checks to make sure that enemy is in range. Then when it’s in range it attacks the enemy spawning in a bullet object.  
Post condition: A bullet is spawned  
Alternative Flows: The enemy isn’t in range so the tower doesn’t spawn a bullet object to attack the enemy with.  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Bullet collision  
Actors: System, Tower System, Enemy System  
Pre condition: A bullet object was created by a tower that was attacking an enemy in range  
Normal Flow: A bullet object goes towards an enemy and upon detect collision it damages the enemy.  
Post condition: The enemy takes damage and checks if it got killed or not.  
Alternative Flows: A bullet misses and doesn’t damage an enemy in the process.  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Give Money  
Actors: Player, System, Money Manager, Shop manager, Round controller  
Pre condition: The player sells a tower  
Normal Flow: Once a player chooses to sell a tower they then activate Money manager to add money to the player pool through giving player money in Round controller.  
Post condition: The total money the player has increases.  
Alternative Flows: N/A  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Upgrade Tower  
Actors: Player, System, Money manager, Shop manager, Round controller  
Pre condition: There exists a tower that can be upgraded on a valid tile.  
Normal Flow: A player clicks a tower and upgrades the tower. Shop manager then upgrades the tower and removes money through the money manager.  
Post condition: The players total money increases.  
Alternative Flows: The playe doesn't have enough money to upgrade and the system informs them they need more money and the tower doesn't get upgraded.  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Generate Map  
Actors: System, Map System  
Pre condition: A new round and game has been started by the player  
Normal Flow: Map generator generates a new map through a draw map function. Then the tileset generator creates all the tiles for enemies to follow on the map.  
Post condition: The game has a new randomly generated map  
Alternative Flows: N/A  
Nonfunctional Requirements: The system is reliable and light on resources 

Use Case: Expand map  
Actors: System, Map System 
Pre condition: A new round has started  
Normal Flow:Map generator uses expand Map to check if it is valid to expand and then creates a new tileset for enemies to follow and calls draw map again to recreate the map.  
Alternative Flows: N/A  
Nonfunctional Requirements: The system is reliable and light on resources 

### 5) Class Diagram and/or Sequence Diagrams

#### Class Diagram

<details name="Class Diagram">
<summary>Diagram</summary>

[![Class-Diagram](/Docs/Diagrams/ClassDiagram.png)](/Docs/Diagrams/ClassDiagram.png)

</details>

#### Map Generation Sequence Diagram

<details name="Map Gen Sequence Diagram">
<summary>Diagram</summary>

[![Sequence-Diagram_MapGen](/Docs/Diagrams/SequenceDiagram_MapGen.png)](/Docs/Diagrams/SequenceDiagram_MapGen.png)

</details>

#### Enemies Sequence Diagram

<details name="Enemies Sequence Diagram">
<summary>Diagram</summary>

[![Sequence-Diagram_Enemies](/Docs/Diagrams/SequenceDiagram_Enemies.png)](/Docs/Diagrams/SequenceDiagram_Enemies.png)

</details>

#### Placement Sequence Diagram

<details name="Place Man Sequence Diagram">
<summary>Diagram</summary>

[![Sequence-Diagram_PlaceMan](/Docs/Diagrams/SequenceDiagram_PlaceMan.png)](/Docs/Diagrams/SequenceDiagram_PlaceMan.png)

</details>

### 6) Operating Environment

Our game could at its finish theoretically operate on any of the main operating systems; Windows, Linux, macOS. Most machines hardware wise will be able to handle running our game, though for best operation it would be suggested that the machine have at least 8GB Ram, and a processor equivalent or better than an intel i5 or extended buffering/glitches might occur. It may be worth mentioning that as our game grows in size through the increments that these operating environment specs will most likely still hold the ability to run our game, but machines with better processors, more RAM, and possible additional GPU’s will be able to run our game much more smoothly.

### 7) Assumptions and Dependencies
  
Dependenices for this project are based on the following assumptions:

#### a) Unity Tutorials

One of the dependencies for this project is that much of the original base code comes from a tutorial series off of youtube. Most of that original base code has been changed completely at this point, but some is still being used for functions that we have not yet been able to modify to fit our game scheme. Over the next increment we will further move away from what little of this code is left/change it almost completely. 

#### b) Unity Editor Package Manager

All, non-Editor related packages are assumed to be dependencies for the project. These packages are as follows:

* 2D Sprite
* 2D Tilemap Editor
* TextMeshPro

#### c) C-Sharp Libraries

Some libraries are assumed to be dependencies for the project. These libraries are as follows:

* System.Collections
* LINQ
