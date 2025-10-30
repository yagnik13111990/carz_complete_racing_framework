# Carz_Complete_Racing_FrameWork
A complete Unity-based racing game framework featuring multiple race modes, and persistent progression systems. Includes drift, sprint, lap, elimination, and time trial events, with player inventory, car purchasing, achievements, quests, and save/load functionality.

Carz Racing Framework is a feature-rich, extensible racing game system built in Unity — designed around clean architecture, reusable patterns, and real-world racing physics.

The framework supports multiple race modes, AI opponents with dynamic pathfinding, and fully modular systems for levels, settings, and UI.
Every subsystem — from AI car control to camera switching — is built with scalability and performance in mind, using Unity’s DOTS, Job System, and ECS hybrid components.

This isn’t just a game — it’s a complete racing game foundation that can power new race types, cars, and systems with minimal coupling and maximum flexibility.

<br><br><br>


🧩 Core Systems & Features

--- Communication across scene---

-> A Singleton–Service Locator hybrid used to centralize manager access across scenes — eliminating the need to make each manager its own Singleton.

<br><br><br>

--- Race Events (via Factory Pattern) ---

-Elimination
-Time Trial
-Sprint Race
-Lap Race
-One v One

-> All race modes are generated at runtime using a Factory Pattern, allowing easy extension and clean reusability.

<br><br><br>

--- AI System ---

-> AI cars implemented using object detection and pathfinding
   Look-ahead point system with Unity splines for realistic racing lines

-> Adaptive speed control based on track curvature (curvy or straight)
   Multithreaded job system for efficient AI updates

-> Intelligent overtaking and collision avoidance


<br><br><br>

--- Car Control System (Hybrid DOTS) ---

-> Realistic gear shifting, traction, and turn control

-> Drift control mechanics for advanced handling

-> Built for performance with hybrid ECS/DOTS integration and Job System

<br><br><br>


--- Settings System (MVVM Pattern) ---

-> Uses MVVM architecture for complete separation of UI and logic

-> Settings data stored and accessed via enum-based references (no string usage)

-> Persistent saving/loading with JSON serialization


<br><br><br>

--- Level Selection & Creation ---

-> Runtime level generation system
   Supports countless levels without predefined limits with integration of Json data assignment
   Level UI items built dynamically using the Builder Pattern and Json Data 

-> Automatic saving/loading of progress and unlocked levels

<br><br><br>


--- Camera System (Strategy Pattern)---

-> Dynamic runtime camera switching
   Strategy-based implementation allows flexible transitions
   (e.g., bonnet, bird eye cam, drone cam , orbital cam)

 <br><br><br>

--- Saving & Loading System ---

-> Persistent data across the entire game using NewtonSoft JSON serialization
   Handles progress, player data, unlocked levels, car setup, race stats and settings stats


<br><br><br>

--- Race Result Management ---

-> Dynamic scoring system based on race type
   Event-driven result generation for Elimination, Time Trial, etc.
   
<br><br><br>

--- Tech Stack & Patterns Used---
- Followed SOLID ,DRY , YAGNI Principle Throughout The Project
  
- System	Pattern / Tech
- Race Event Creation	Factory Pattern
- Camera System	Strategy Pattern
- Level UI & Generation	Builder Pattern
- Settings System	MVVM Pattern
- AI Pathfinding	Spline + Look-ahead + Job System
- Data Persistence	JSON Serialization
- Performance	Hybrid DOTS / Job System
- car shop using Addressables + MVC + json saving + Observer Pattern for car selection
- car inventory using Addressables + json loading

<br><br><br>

--- Gameplay Highlights ---

-> Multiple playable race types with real-time switching
   Fully dynamic AI racing experience
   
-> Responsive car physics and drift mechanics
   Deeply integrated settings and saving/loading flow
   
-> Designed for scalability, readability, and performance


<br><br><br>

--- Architecture Goals ---

-> Fully modular, pattern-driven architecture
-> Extensible for new race modes, AI behaviors, and camera types
-> Clean separation of logic (MVVM, ECS hybrid ,Strategy , Abstract Factory ,MVC , Builder , Observer , Repository , Service Locator + Singleton , Event Bus )
-> Built for maintainability, scalability, and performance

<br><br><br>

--- Future Extensions ---

Multiplayer (Photon or Netcode for GameObjects)
Advanced replay camera system
Dynamic weather & track surface simulation
