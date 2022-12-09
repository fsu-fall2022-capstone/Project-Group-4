<div align="center"> 

## Progress Report <br> <br> - Increment 3 - <br> <br> Group #4

</div>

### 1) **Team Members**

* Daniel Pijeira – DAP19D – dpijeira1
* Lawrence Martinez – LPM19 – alexander189
* Nathan Granger – NMG18D – ngranger22
* Tyler McLanahan – TJM20DB – tylermclanahan
* Tyler Pease – TCP20T – sirtarragon

### 2) **Project Title and Description**

#### **Orcs & Towers**

Orcs & Towers is a 2D, fantasy, tower defense game where the player must defend their base from waves of orcs. The player can build and upgrade towers to defend their base and expand the map to make the path longer for the orcs. The player is given a set number of lives and a starting amount of money to purchase towers with, each round ends when all enemies have been destroyed.

### 3) **Accomplishments and overall project status during this increment**

During increment 3 we were able to get most of the last pieces of our Tower defense game implemented. This increment we added in a couple new tower types, tower upgrades, the ability to sell towers back to the store, new enemy types, new enemy sprites, new tower sprites, we revamped the UI shop to include all of our towers as well as give the player the ability to minimize the shop window, we also added extra purchasable abilities for the towers that only last 10-15 seconds as well as a UI shop for them, added in placement functionality for the purchasable tower abilities, we fixed bugs like the fast forward button sending enemies to the players home, fixed the player money bug that was adding extra money after the first enemy killed, changed the font scheme for the game, changed the UI background color of the main scene to make it easier to view the in progress game, and we changed some of the UI for the Game Over screen to mimic a real ‘in production’ tower defense game. Really the biggest thing that we were not able to get implemented this increment was the animation of enemy sprites, although it is worth mentioning that changing the sprites from red squares to actual orc sprites added a slight animation feel to the game.

### 4) **Challenges, changes in the plan and scope of the project and things that went wrong during this increment**

There are a number of different changes to the original scope of the project, mainly due to the lack of major progress with these aspects. The core functionality of the project has been limited to an expanding map mechanic, with the addition of a story mode and a preset path gameplay being fully dropped as goals. We were also unable to add really any sort of animation in this increment, but were able to change the enemy sprites from red squares to orcs which does give a slight animated feel to the game.  It is hard to say the reasoning for these changes/issues, it seems team members ended up being busier than originally expected and were unable to fully implement their proposed functionalities, for many of the team members this is not their only class and exams may have made them unable to work on the game as much as they may have wanted to.

### 5) **Team Member Contribution for this increment**

* Daniel Pijeira — Made some contributions to accomplishments in the Progress Report. Participated in the video presentation.
* Lawrence Martinez — This increment I implemented the UI and functionality to be able to sell and upgrade towers. Then if a tower was already upgraded I would make that UI element not popup. I also fixed a bug with the key binds for pausing the game not working. I added functional requirements to the documents. Participated in the video.
* Nathan Granger — This increment I made changes to the UI on the StartScene (got rid of extra ‘new survival game’ button, changed the color scheme of ‘new game’ button), I also changed the font scheme for the whole game (downloaded a free opensource font asset and implemented it to all game text), I updated the GameOver screen with UI to show the players score as well as added some code to the MoneyManager script to update the players score, I created all of the UI for the new towers cards in the shop except the lightning tower as well as created the UI cards for all of the tower boons in the shop, I then created the BoonCostDisplay script to display boon names and costs on the UI shop cards, I also worked with Tyler Pease to fix a player money bug that was adding in too much after the first enemy was killed and also sometimes not allowing more money to be added, I also worked with Tyler Pease to add in the Boon placement functionality so that Boons can be added to towers from the shop, I also updated the background color of our main game scene to fit our game scheme better, and lastly I updated the enemy sprite from a red square to a orc and added in code in the Enemy script to check the direction the enemy is moving and flip them if need be so they face the way they are moving (adds a slight animated feel to things). In the documentation for this increment I wrote the ‘Accomplishments and overall project status’ section in the Progress Report, I added to the ‘Challenges, changes in the plan and scope of the project’ section in the Progress Report, wrote my contribution to the project here in the ‘Team member contribution’ section, I also contributed a small amount to the ‘Assumptions and Dependencies’ and to the ‘Operating Environment’ sections on the RD document, and lastly I participated in, recorded, and posted the increment 3 video to Youtube.
* Tyler McLanahan — Added in functionality for upgrading towers. Each basic tower will receive a permanant range and damage boost, and elemental towers receive special upgrades to their effects. When upgraded, Ice towers now burn over time while slowing, Fire towers now do area of effect damage on impact, and Lightning towers now chain lightning when striking enemies. I cleaned up the way statuses are applied to enemies. I also helped create the graphics used for towers and projectiles. Finally, I contributed to the RD and IT documents by adding functional and non-functional requirements. Participated in Video presentation.
* Tyler Pease — Updated diagrams for the RD document based on changes throughout the increment. Updated to the different shop UI to allow for it to iterate through a ScrollView, worked on sprite rendering code and sprites themselves for towers (made other changes to accommodate), added AOE Bullet implementation and trebuchet tower, fixed placement manager while communicating with Nathan to accommodate for boons, changed map expansion rate and its split ability to improve on performance, and finally worked on a number of bug fixing through quick commits (null reference errors, helped with fixing fast forward button). Participated in Video presentation.

### 6) **Plans for the next increment**


### 7) **Link to video**

[https://youtu.be/1_TmROROKFw](https://youtu.be/1_TmROROKFw)
