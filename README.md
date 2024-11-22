# WindowsPokedexApp
This is an app that I made using Windows Forms in C#. That is just a fun app to simulate a Pokedex
from the game Pokemon. It uses event-handling to deal with most interactions and uses a bunch of custom controls (Of varying quality, currently). It also pulls
information including sprite information from https://pokeapi.co/

## What is it???
This is supposed to similate the Pokedex from Pokemon, and is specifically based
off the Pokedex to gen 5. It uses [this](https://github.com/jtwotimes/PokeApiNet) .NET wrapper for [PokeAPI](https://pokeapi.co/).

## Prerequisites
This is a .NET app made with .NET 8.0, so that is the only actual prerequesite.

## TODO
* The app is a bit buggy, especially for the first few seconds as it loads all the pokemon.
* Currently, it only displays mostly basic information about the Pokemon, but I'd like to add more information.
* Improve organization and decouple my classes. It could use a lot of work.
* Build and deploy it into an actual executable that can be downloaded. Currently this is just the source code.
