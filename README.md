# VehicleDestruction

This Mod adds destruction to vehicles in Kitten Space Agency.
Currently, if you crash into an Astronomical (Planet, moon, asteriod) while going greater than 14m/s your vehicle will be destroyed.
Please note this is still very janky due to colliders being a bit weird in KSA.

Using ModMenu you can also view your last crash report if you want to see it again.

**Known Issue:** 
- Opening the last crash report without crashing a vehicle first will cause your game to crash. This is due to dummy data that is loaded.
- The current logic only checks for collisions with your currently controlled vehicle. If you switch vehicles mid-flight the previous vehicle will not be affected by the collision checks.

**License:** MIT License

## Dependencies

Each of the dependencies must be installed and put into the `KSA/Content/` folder and have their `mod.toml` info put into the `manifest.toml` in `Documents/MyGames/Kitten Space Agency/`.

1. ModMenu: [GitHub](https://github.com/MrJeranimo/ModMenu/tree/v0.1.0), [SpaceDock](https://spacedock.info/mod/4054/ModMenu)

## Using as a Dependency

Anyone is free to use this mod as a dependency for their own mods. I currently do not know how to do that so this section will be updated as I learn more about it.

I would recommend trying to point to the compiled `VehicleDestruction.dll` file to reference its code, but I am not sure if this will work. 

Good Luck!

## Installation

If you install the Release, just make sure to add

```
[[mods]]
id = "VehicleDestruction"
enabled = true
```

to `\Kitten Space Agency\Content\mainfest.toml` or `\Documents\My Games\Kitten Space Agency\manifest.toml`.

And make sure you put the `VehicleDestruction` folder in `\Kitten Space Agency\Content\`.
