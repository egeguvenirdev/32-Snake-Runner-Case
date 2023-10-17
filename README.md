# assignment01-egeguvenirdev
assignment01-egeguvenirdev created by GitHub Classroom

Project Version: 2019.4.18

Game Mechanics:

1- Merging (Merge Town): Begining of the game, you can increase the level of the main ball, which enhances the player's size and power.

![Merge](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/4e97adfc-3dd9-4fbe-933e-3625f4a46b88)



2- Pushing: In mini game, both enemies and the player attempt to push each other down.


![pushing](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/e1ef3c34-9feb-4e89-9ba6-5fa7b57b56f1)
![pushing 2](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/ef47b1ad-53c7-426e-ba5d-1b023067f7b2)




3- Swerve (Aquapark.io): The primary control of the character.


![Swerve](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/9ccedbaf-785e-4d0b-b943-fa595a1b9dfe)



4- Stack and Growing (Stack & Paper.io 2): Balls collected on the path are lined up behind us like a snake and follow us. At the end of the game, they provide additional (size and power) enhancements.


![Stack](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/3ee28634-f0e6-40ac-b66b-bec4b0418299)



Additional Mechanics:

1- Upgrade System: Users can make permanent upgrades before the game starts using the money they earn.
Income: Increases the money earned by the player.
Size: Reduces the opponent's pushing power at the end of the game.
Power: Increases the player's pushing power at the end of the game.


![Upgrade](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/96e22ec3-8c46-4316-81ab-0556d468c520)



2-Gates: When passed through by the player during the game, it provides temporary enhancements.


![Gate](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/df020aa9-17df-4403-ae52-dfcbc4d67893)

----------

Coding:

1- The game runs on a single scene, using multiple scenes resulted in performance issues and was not supported on Android 5.0 and below. The game has a complete game loop (win-lose loop). The Level Manager continues the loop by using the old levels when all levels are completed.

2- Functions like Start, Awake, OnEnable, etc. were avoided whenever it possible. Necessary assignments were made by managers using the Init and DeInit functions of scripts.

3- Buttons, upgrade cards, collectible objects, and similar items with similar functions inherit from a base class.

4- Tag usage was minimized. Physics operations were performed using layers. This way unity doesn't have to handle unwanted collisions.

5- Instead of using strings, a ConstantVariables class was created, and the ConstantVariables class is used in places where string usage is necessary.

6- "Boss stats implemented by a dictionary that works with a readonly struct referencing (bosslevel is the key) instead of using scriptable objects. (EnemyConfig (structure) - EnemyConfigUtility (dictionaruy))

7- Scripts that would use the Update function are subscribed as a sub-action to the Update Manager, allowing multiple updates to be handled through a single update.

8- Action Manager was used to reduce the dependencies between scripts.

9- Object pooling was used to avoid unnecessary instantiation and destruction of objects, preventing the inflation of the garbage collector.

10- Unnecessary show, cast, and receive shadows were disabled.

11- GPU Instance option enabled in materials that objects sharing same meshes.

12- Getter and setter properties were used to access script variables for preserving encapsulation.

-----

Profiler for a level:



![Profiler](https://github.com/RubyGameStudioAssignments/assignment01-egeguvenirdev/assets/96069970/ff08c870-4a4d-4531-8df9-9be98212f7b7)
