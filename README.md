# Ravenfall-Server
![CI Build Ravenfall-Server](https://github.com/zerratar/Ravenfall-Server/workflows/CI%20Build%20Ravenfall-Server/badge.svg)

The Game Server for the Twitch Integrated MMORPG Ravenfall

This repo also includes the Centralized Chat Bot now, moved from previous repo to avoid duplicate code bases.

This repo also use .NET 5 Preview 7.

The network library used in this project is a modified version of https://github.com/DarkRiftNetworking/Hazel-Networking to fix memory allocation issues and more. This is being used in any projects communicating with the game server.