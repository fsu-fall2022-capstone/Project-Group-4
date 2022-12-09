<div align="center">

## Software Implementation and Testing Document <br> <br> For Group #4 <br> <br> Version 0.5

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

### 1) Programming Languages

C# is the main programming language of the project. The main reasoning behind this decision is due to the usage of the Unity Engine (which uses C# as the programming language for game development); in addition, the language was chosen for its similarity to C and C++, as well as some prior experience in the C# course for a number of the members for group #4.

### 2) Platforms, APIs, Databases, and other technologies used

* Unity 2021.3.1f1 - Game Engine - Used entirely everywhere for this project, as it is the main engine for the game and easy to access for its development.
* .NET Standard 2.1, C# 7.0 - Required by the Project via Unity - The project requires the use of .NET Standard 2.1 and C# 7.0 for the project to run properly.

### 3) Execution-based Functional Testing

We performed functional testing on our project through sanity and integration tests using Github. For sanity testing, we determined proper functionality using the Unity Editor game simulation feature to ensure scripts functioned as intended before committing them to Github. For example, one of our functional requirements in the RD document states that bullets from towers in our game must apply a set damage value to an enemy. We applied sanity tests after the initial commit to ensure there were no bugs in collision and damage registration on an enemy. Once sanity tests were complete we used git merge for integration testing to ensure there were no conflicts among new assets commited to the system. For both of these tests we used a black box testing method, focusing solely on functionality.

### 4) Execution-based Non-Functional Testing

We performed similar testing as we did for functional testing where we went through sanity tests. We would run longer tests to make sure things like having X amount of towers with Y amount of enemies on the screen didn't hurt performance. Due to the nature of sanity checking, we constantly have to use the UI making sure that it's very usable and doesn't take too many clicks to get things done.

### 5) Non-Execution-based Testings

We organized non-execution based testing through Github and our development Discord. Commits and pull requests on Github are thoroughly documented for the ease of the team. We frequently asked each other for feedback on committed code as well as conducted code walkthroughs and reviews.
