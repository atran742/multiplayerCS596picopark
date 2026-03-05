# Piko Parc

A 2D cooperative multiplayer platformer game built in Unity and uses Netcode for multiplayer.
One level is a collaborative platformer while the second level ties to players together to figure our the platformer. 

## Game Overview
Players should be able to jump and move left and right <br> 
They should be able to collect gems<br> 
Movement is owner-authoritative using Unity Netcode<br> 
A shared camera follows both players<br> 
Players can pull and drag each other using a rope constraint in the second level<br> 
Falling into a kill zone respawns the player at the scene spawn point<br> 
The rope mechanic forces cooperation — if one player falls, the other can pull them back up.<br> 

## Installations
Unity 

## Features
Multiplayer using Unity Netcode for GameObjects<br> 
Owner-authoritative player movement<br> 
Server-authoritative scene spawn management<br> 
Shared midpoint camera with dead zone and smooth follow<br> 
Rope physics using joint constraints<br> 
Scene-based spawn system<br> 
Respawn system using ServerRpc + ClientRpc<br> 
Network-synced gem collection (NetworkVariable)<br> 



