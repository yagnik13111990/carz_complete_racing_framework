# Carz_Complete_Racing_FrameWork <br>
A complete Unity-based racing game framework featuring multiple race modes, and persistent progression systems. Includes drift, sprint, lap, elimination, and time trial events, with player inventory, car purchasing, achievements, quests, and save/load functionality. <br>

Carz Racing Framework is a feature-rich, extensible racing game system built in Unity — designed around clean architecture, reusable patterns, and real-world racing physics. <br>

The framework supports multiple race modes, AI opponents with dynamic pathfinding, and fully modular systems for levels, settings, and UI. <br>
Every subsystem — from AI car control to camera switching — is built with scalability and performance in mind, using Unity’s DOTS, Job System, and ECS hybrid components. <br>

This isn’t just a game — it’s a complete racing game foundation that can power new race types, cars, and systems with minimal coupling and maximum flexibility. <br>

<br><br><br>


🧩 Core Systems & Features <br>

--- Communication across scene--- <br>

-> A Singleton–Service Locator hybrid used to centralize manager access across scenes — eliminating the need to make each manager its own Singleton. <br>

<br><br><br>

--- Race Events (via Factory Pattern) --- <br>

- Elimination <br>
- Time Trial <br>
- Sprint Race <br>
- Lap Race <br>
- One v One <br>

-> All race modes are generated at runtime using a Factory Pattern, allowing easy extension and clean reusability. <br>

<br><br><br>

--- AI System --- <br>

-> AI cars implemented using object detection and pathfinding <br>
   - Look-ahead point system with Unity splines for realistic racing lines <br>

-> Adaptive speed control based on track curvature (curvy or straight) <br>
   - Multithreaded job system for efficient AI updates <br>

-> Intelligent overtaking and collision avoidance <br>


<br><br><br>

--- Car Control System (Hybrid DOTS) --- <br>

-> Realistic gear shifting, traction, and turn control <br>

-> Drift control mechanics for advanced handling <br>

-> Built for performance with hybrid ECS/DOTS integration and Job System <br>

<br><br><br>


--- Settings System (MVVM Pattern) --- <br>

-> Uses MVVM architecture for complete separation of UI and logic <br>

-> Settings data stored and accessed via enum-based references (no string usage) <br>

-> Persistent saving/loading with JSON serialization <br>


<br><br><br>

--- Level Selection & Creation --- <br>

-> Runtime level generation system
   - Supports countless levels without predefined limits with integration of Json data assignment <br>
   - Level UI items built dynamically using the Builder Pattern and Json Data <br> 

-> Automatic saving/loading of progress and unlocked levels <br>

<br><br><br>


--- Camera System (Strategy Pattern)--- <br>

-> Dynamic runtime camera switching <br>
   - Strategy-based implementation allows flexible transitions <br>
     (e.g., bonnet, bird eye cam, drone cam , orbital cam) <br>

 <br><br><br>

--- Saving & Loading System --- <br>

-> Persistent data across the entire game using NewtonSoft JSON serialization <br>
   - Handles progress, player data, unlocked levels, car setup, race stats and settings stats <br>


<br><br><br>

--- Race Result Management --- <br>

-> Dynamic scoring system based on race type <br>
   - Event-driven result generation for Elimination, Time Trial, etc. <br>
   - Race Progression tracking during the race <br>
   
<br><br><br>

--- Tech Stack & Patterns Used--- <br>
- Followed SOLID ,DRY , YAGNI Principle Throughout The Project <br>
  
- System	Pattern / Tech <br>
- Race Event Creation	Factory Pattern <br>
- Camera System	Strategy Pattern <br>
- Level UI & Generation	Builder Pattern <br>
- Settings System	MVVM Pattern <br>
- AI Pathfinding	Spline + Look-ahead + Job System <br>
- Data Persistence	JSON Serialization <br>
- Performance	Hybrid DOTS / Job System <br>
- car shop using Addressables + MVC + json saving + Observer Pattern for car selection <br>
- car inventory using Addressables + json loading <br>

<br><br><br>

--- Gameplay Highlights ---

-> Multiple playable race types with real-time switching <br>
   - Fully dynamic AI racing experience <br>
   
-> Responsive car physics and drift mechanics <br>
   - Deeply integrated settings and saving/loading flow <br>
   
-> Designed for scalability, readability, and performance <br>


<br><br><br>

--- Architecture Goals ---

-> Fully modular, pattern-driven architecture <br>
-> Extensible for new race modes, AI behaviors, and camera types <br>
-> Clean separation of logic (MVVM, ECS hybrid ,Strategy , Abstract Factory ,MVC , Builder , Observer , Repository , Service Locator + Singleton , Event Bus ) <br>
-> Built for maintainability, scalability, and performance <br>

<br><br><br>

--- Future Extensions ---

-> Multiplayer (Photon or Netcode for GameObjects) <br>
-> Advanced replay camera system <br>
-> Dynamic weather & track surface simulation <br>



