# Carrom Project
![image](https://github.com/rahulsingh2031/Carrom-Project/assets/106699245/cb42b625-a49a-4e4b-99ca-cc6e905fd909)

This is a carrom game project developed for an internship. It features a single mode of AI vs. player gameplay.
The game is played on a virtual carrom board, where the player competes against an AI opponent.

# Note:
In the Carrom Game project, I adopted an Event-Driven Architecture to separate the user interface (UI) and logic components as much as possible. This architectural approach promotes loose coupling and modularity, allowing for easier maintenance and future expansions.

The project consists of multiple scripts, each responsible for a specific aspect of the game. The main script(Game Manager) controls the overall game state and manages the turns of the player and AI opponent. By centralizing the game state management, it becomes easier to handle events and transitions between different game states.

The UI and logic components are decoupled from each other. This means that the UI elements, such as buttons, graphics, and user input handling, are implemented separately from the core game logic. The UI elements communicate with the logic components through events, enabling a clear separation of concerns.

Currently, the player's Striker has a UI dependency, meaning that the UI component is directly linked to the Striker's behavior. However, with some modifications, this dependency can be removed, further decoupling the UI and logic components. This separation allows for easier maintenance and testing of each component independently.

One notable advantage of the project's design is its flexibility for future expansions. For example, if you want to add a player-vs-player (PvP) mode, the existing structure can accommodate this change without requiring significant modifications. The modularity and separation of concerns in the codebase make it easier to add new functionalities and game modes.

However, there is one issue in the current project: both the AI and player scripts reuse some code. To minimize code duplication and improve maintainability, it is recommended to introduce a base class that encapsulates common functionalities. By extracting shared code into a base class, you can eliminate redundancy and enhance code reusability

# Warning:
Since I have worked on  this project for about 1.5 days only  ,it might have some unexpected bugs which i didn't found during my development.although i expect no game breaking bug is present currently in the
project
