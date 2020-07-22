# Servers (Currently)
Current setup of the servers during early development phases. The planning further down is not set in stone but a possible direction to take.

## Game Server
### Game server takes care of all incoming connections, authorization and game updates. It does not rely on any other server to function
Port 8133
Accessibility Public

---------

# Servers (Future planning)
All servers are single threaded except for the Front Server that will handle multiple connections

## Front Server / Connection Server
### The server that will hold all game connections, redirect data where needed.
### The server also keeps track on active servers, this is the center of providing information about all running servers
### When a login is successefull, the front server will tell the client which game server to connect to.
Port 8113
Accessibility Public

## Auth Server
### The server that handles login requests sent from the Front Server. This has read access to the db.
Port 8123
Accessibility Internal

## Game Server - Instanced
### A game server instance keeps track of players, AIs, and other realtime events as well as all player persistance to the database. 
### This also includes non realtime updates. Each Game Server may have their own dedicated persistance for storing active game state data.
### Player data is stored separately as it is being globally used.
Port 8133
Accessibility Public


## Ideas
### Synchronizing player movements
Since we are using a pathfinding, click to move kind of system. We can interpolate between the start (previous destination), destination, (length, which is minimum dest-start) 
to get an estimate % of progression. We can then use that to determine where a player is when another player joins the game while the player is walking.

Example: Player A walks from 10,0,20 to 44,0,7. The distance is grabbed from the pathfinding algorithm, be it unitys built in navmeshagent when sending walkto destination, also provide a length
		 this can potentially be hacked by players. but only to make the character move as if they were walking through walls. basically. But it would appear as if their movement speed is crazy.

Another solution? Do the a-star algorithm on the server side. but this requires more CPU usage and also a copy of the world map.


## Data Persistance
There are 3 main databases

### User Account db
It stores all account related data such as username, password, linked services as Twitch, YouTube, etc. 

### Player Character db
It stores all player/characters available for the different users.

### Game server db
It stores the active game session states and information regarding NPCs, Objects, Items, and any other world changes.


## Server Communications
### Login
When a client connects to the front server, it will need to send an auth request before it can get assigned a game server.
The FS (front server) will then ask the AS (auth server) to validate its request. If its valid a token is given back for the current session.

The FS will then wait for another request from the client (this time bearing the token) to join a game