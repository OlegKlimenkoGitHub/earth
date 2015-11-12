# earth
Game battle for earth

Skip to content
This repository  
Search
Pull requests
Issues
Gist
 @OlegKlimenkoGitHub
 Unwatch 1
  Star 0
 Fork 0 OlegKlimenkoGitHub/earth
Branch: master  earth/_describe.me
@OlegKlimenkoGitHubOlegKlimenkoGitHub 2 minutes ago describe
1 contributor
RawBlameHistory     264 lines (220 sloc)  9.91 KB

Project history:
================================

11.11.2015
Animation robot is visible when the page is loaded - fixed.

01.10.2015
Placed applications online.

26.09.2015
I make a redirect th death-page to the Game\Index
Added ProgressBar while creating the game world at registration

25.09.2015
Make a section About.
Make a section Rules.
Fixed a bug. Allows you to build your arrangements in a foreign country for foreign resources.

24.09.2015
Created a controller "Battles".
Created a controller "Reports".
Created a class "Reports".
Stores data for only the last move. When NextTurn be updated.
Add the table "Evs" to store events.
Created a class "Journal".
Inserted the robot animation.
Added ProgressBar in NextTurn

21.09.2015
Created a controller "Bunker".
Very large delay in clearing tables. (Rewrite the query)
I made through a stored procedure. A huge increase in the speed.

20.09.2015
Do not work through ProgressBar SignalR when creating a new game - fixed.
Added a handler for the button "New game".

19.09.2015
Did the button "Next Turn".

18.09.2015
Created a controller "Factory".
  
17.09.2015
Created a controller "Country".
Created a controller "Design".

10.09.2015
Added logging in controllers Account and Map
Checked work of NLog.
Accelerate downloads, found bottlenecks.

09.09.2015
Did ProgressBar for long running queries. (For example initialization games or processing speed)  using SignalR

08.09.2015
Made the primary initialization of the game are opponents, set dependencies.
Brought map with colored countries.
Set in the project .NET Framework 4.5. Eliminate compilation errors. We need to work SignalR.

06.09.2015
When registering a new user, it is automatically assigned the role of "Gamer".
In the Controller Map Add authorization. Allow only users with the role of "Gamer".

01.09.2015
Moved stored procedures.
Moved to Battle.Domain work with database tables.

31.08.2015
Changed the architecture of the project.
Created a library Battle.Domain (EF).
Created MVC project Battle.WebUI (EF, Ninject, Mock, BootStrap, NLog).

30.08.2015
Created a new version of the project (version 1.0).
Changed the name of the project on the "Battle".
To lay the foundations of multilingual. Including the database structure.
Moved the table authorization from the local user database into my database.
To figure out how to make the base multitenantnoy.

28.08.2015
Finished the study of the video
https://www.youtube.com/watch?v=MlPYmNVGQEs&list=PLJUoF2h8Z-brW94dTZ-ZIOhjFq90_lt5K
According to the results of the modified development plans.
It was decided to create a new version of the program.

23.08.2015
Install the program Log2Console. I set up a cooperation with NLog
Connect by logging NLog

20.08.2015
I have tested the tide of battle.
Army winner is not determined correctly - fixed.
Added logging
In gloval.asax added Database.SetInitializer <CountryContext> (null); to relieve the problem when you change the model
Make backup ms sql database

02.08.2015
If the army a few players were in the same country to produce a battle.
Create an indexed list of players (shooting turns)
Create lists of mechs ready to fire (not dead and has not fired)
Choose a random mech that will shoot
Choose a random army on which he will shoot
Choose a random mechs which will fire (number of bellows equal to the number of guns)
Cannon hits all the targets with armor <= her caliber.
Deaths mechs strike from the army (qnt = 0)
If the whole army was killed, to strike out the player from the list. (isEmpty)
If the list has only one player - a victory
If you win - to tie the country to the winner
Shoot mech removed from the list ready to fire.
Pass the course to the next player on the list.
If a list of mechs ready to fire emptied the battle ends. Others will fight in the next turn.

01.08.2015
Finished method SendMechs

31.07.2015
Made refactoring class Game. All sql-queries make in separate methods.
Wrote engine for the tab "Bunker".

30.07.2015
NextTurn 
Increase ore reserves worldwide. 
Randomly generated bellows design for computers (one per turn). 
Rounding always in the smaller side, ie discard after the decimal point. 
How many mechs build? qnt = rnd 1 to minerals. 
What resources will be spent on a mech? res = minerals / qnt 
what will be the caliber? caliber = rnd 1 to res 
How many guns deliver? guns = rnd 1 to (res / caliber) 
How much armor sheds? shield = res - guns * caliber 
Check whether there is an army for this player and in this country, if not - create 
Increases the number of mechs in the table Battlemech 
Building a bellows according to plan how much is enough resources. 
Reduce the table of country resources

29.07.2015
Wrote engine for the tab "Factory".
Make a button "Next Turn".

28.07.2015
Factory. Make it a drop-down list of possible designs.

26.07.2015
Wrote engine for the tab "Design Bureau".

25.07.2015
Wrote engine for the tab "Country".

24.07.2015
When loading page, i drew a map of the world in which all countries are shaded color of their players
When downloading generated get ajax-request to the server.
Written the controller, which processes the request and returns the data in json format.
http://techbrij.com/map-chart-pop-up-asp-net-mvc-jvectormap
Learn to paint its color code of the country.
http://jvectormap.com/examples/random-colors/
Implemented the submenu.
Make the right of the map display panel content hopper. 
    
21.07.2015
In the database set up field null and set the relationship between the tables.
For each table I created a class-bound Entity Framework.
Created a context for the classes.
Added Class Game, which will control the game and keep the current state.
Added initialization players.

20.07.2015
Connected the library jvectormap to the project.

18.07.2015
Import data from excel spreadsheet in country
Generated pop-data.js file with the data on the real number of the population.
Shaded country blue varying intensity. The smaller the population the paler.

17.07.2015
Created the database of the game.
Prepared to excel spreadsheet with the number of the world population.

15.07.2015
Created the new project MVC.
Wrote the legend of the game based on clippings from Galaxy Times.
Developed a functional description (game mechanics) of the game.
Prepared description of the required tables.


Status API Training Shop Blog About Pricing
© 2015 GitHub, Inc. Terms Privacy Security Contact Help
