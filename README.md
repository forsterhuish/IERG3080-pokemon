# IERG3080ProjectPart-ii_HUI Sze Ho, HO Tsz Ngong - Design report
## GUI design pattern
We intend to adapt **MVP** design pattern for the project. The mainView itself only contains the control, and subscribed to the presenter. Button events handlers are pointing towards subscribed presenter functions. Therefore the view can be change freely, only need to follow a certain pattern, etc, Inventory button, start button. The presenter and model do not need to be changed for further levels(view) in the future. \
Since this is a simple game with majority of Model evolve around `User`(at least using its `Inventory` and `PokemonInventory` attribute), we decided single presenter is appropriate for this project. \
And we dedicated the presenter to have minimal amount of logic handling, leaving most of the logic to Model classes, by method calls.
However since some logic are view based, etc start grid deletion, some basic lines remains in view.cs.

## Class design
### Pokemon
For the **Pokemon** class, since we believe that all Pokemon should share some common features, e.g. HP, MP, type, some attack methods (`Bite`, `Punch`), therefore we adopted **strategy** as the major structural pattern. The class **PokemonTemplate** act as a basic framework for other Pokemon class to follow. PokemonTemplate defined certain set of operations that all Pokemon should share, while also include certain delegates (e.g. `AttackAction`, `IdleAction`) for future Pokemon class to add new action that is specific to each subclass. For example, the class **Pikachu** will have its own attack method  `HundredThousandVolts()` while another class **IERG_Student** will have an attack method `Coding()` that is unique for this Pokemon. By defining such framework, we hope that this class can be future proof, such that future new Pokemon class can work seamlessly with the current structure. 

Besides, we also create the class **PokemonFactory** which serves as a “database” for other classes to access and obtain a copy for each Pokemon created. As a result, we have included the **ICloneable** interface in PokemonTemplate which include a `Clone()` method, so that other classes, such as Inventory, can get access to a static database of all Pokemons without manually create a new one. We believe that this allows better performance (in terms of time and space) such that if any new Pokemon class contains many fields and methods, then instead of a costly instance creation operation, a copy of existing instance will be a better way for obtaining a new Pokemon. Also, the PokemonFactory class can be modified to support new features, such as controlling the number of “wild” pokemons that exists on the map.

### User
For a single player game, it is logical to make the **User** class a **Singleton**. Since then we can have access to the same content by having the presenter subscribe to the User. Also since User class is the core of all the logics, Singleton can ensure everyone having the same data.

For inventory storage, Dictionary is being used, having string *(item name)* as key, int *(number of item)* as value. Therefore we are passing string to indicate which items refering to. For further prove, say, when different items are implemented with extra attributes(eg `Durability`) or methods(eg `onUse()`, `getDurability()`), a `Item` class should be used to replace string. Like `PokemonTemplate`, the `Item` is the template for other item classes to follow. \
`Item` class is designed but not used in this verison, because encountered difficulties on treating same class type (eg `Coin`) but different objects, modifications are needed to combine two objects together. But with limited time, we dicided to workaround by having simple string as parameter.

### Map
For the map, a island fashion style is being used, such that there will not be infinite extentions, but rather a fixed level. A 3\*5 grid is used this time. One can easily change the dimension for creating different levels. For easy implementation, **radiobutton** is being used as a **Node**, such that the previous location can be ignored.

**Singleton** is being used, for the same reason as User.

For buttons shown up ramdomly (wildPokemon) and fixed location (Gym), it mostly based around **Grid**. \
We used the section of currentNode, and comparing it to wildButton setion, and determine is the Button reachable for the player or not. Currently, wildButtons can be appear in sections having no nodes, making those unreachable, then can only wait for it to despawn. In the future, parameters can be pass from Map to PokemonSpawner, telling if the radiobutton in the section is visible or not, indicating is that section reachable and therefore should buttons be spawned there. A clock is being used to control the speed of Pokemon Spawning and Despawning.

### Catching (Capture)
A new Grid is being used, placed on top of the Map. A sequence minigame with class `CatchGame` is being used. \
The game itself is a **Singleton**, meaning that it will only initiate one time only. Every other time, `newCatchGame()` is called to change the background and the sequence pattern. Currently the winning logics are inside the eventhandler itself. But in the future, we will refactor the code and move the winning logic into its own function, allowing more flexibility.

### Battle (Gym)
For the **battle** component, we decided to implement a battle between user and CPU pokemon (more like a “training” battle instead of battle between two real-world players). Instead of the simultaneous battle mode in original Pokemon GO, we adopt **sequential battle game** mode, like some old-school online battle games. To stimulate the taking turns action, the **paper-scissors-stone game** is adopted. The user needs to first choose a pokemon that user owns and an opponent pokemon that user wish to fight with. Then, for each turn, user need to play a paper-scissors-stone game with the opponent, and the winner can initiate an attack. Since this project is a prototype for single player game, an **AI** class is included for demonstration purpose. The opponent pokemon will be turned into an AI and performs attack/idle actions on user’s pokemon/itself respectively. 

On the design issue, we have adopted **singleton** for battle since it is more natural to allow only a single battle to happen in the whole game, and different classes can refer to the same battle instance easily. We hope that using our current battle design, we can allow more flexible extension of current functionality, such as the support of online battles. 

### Inventory
In the Grid dedicated to Inventory, two StackPanel InvList and DetailList are created for the left and right contents. InvList will have Buttons for user to choose which page they want to access. DetailList are the shown, depends on which page the user clicked.

In the current setup, method `showDetail()` combined with different `Detailxxx()` is being used to redirect how the appearence of DetailList should be changed. For future, we want to futher adapt a **Strategy** pattern to organize it better. Because of this, a seperate `Detail` class will be created, with all the `Detailxxx()` methods moved to here. 

Based on the "Dictionary string workaround" mentioned, we use Inventory UI to centralize the use of items. Buttons will be dedicated to use of items like potions, with local methods designed. With more new items in the future with different effects, or same effect but different values, it is certainly better to have the `Item` class inplement the `onUse()` methods. Such then there will not be methods for every item available in this less appropriate `Inventory` class, it can then just be a `void useItem(Item item) {item.onUse();}`. 

For the Trade function, it is the "Extra Feature", a small game "Reforge" with its own class is dedicated to this feature, turning 2 pokemon into 1 if succeed. The `ReforgeGame` class can be easily modified if needed.

### Item factory
`ItemFactory` is a basic class with the signle purpose, return random items after successfully catching pokemon. Currently it is a semi-**Factory** pattern, since we are not returning a new class object, but just a simple string, because of the previous mentioned workaround. But after `Item` class is implented with its children classes like `Potion`, this can be modified easily to adapt the changes.

### MISC
#### Singleton
Most of the classes use **Singleton** pattern, for multiple reasons.

One reason is for data consistance. For classes like `User`, it is easier for everyone to keep a reference to the User instance, and work with the data `User` provides. \
Second reson is the initiation. For classes closely related to UI, like `Inventory` and `CatchGame`, it is better to have one instance, then open and close the same instance everytime. This can prevent multiple windows from oepning and bugged the game, and prevent multiple object initiated and use up unnessary memory.

#### UI creation
As one can tell from the Source Code, especially from `Inventory` class, a bug chunk of codes are for creating different controls like StackPanel, TextBlock. These code blocks are often repetitive. So when refactoring the code in the future, we want to dedicate a new class for the UI setup, potentially make use of Strategy pattern.

## Division of Labour
HUI, Sze Ho Forster : Pokemon, Battle(+ AI, PaperScissorsStone) \
HO, Tsz Ngong : Map (+ PokemonSpawner), CatchGame, Inventory (+ ReforgeGame), User
