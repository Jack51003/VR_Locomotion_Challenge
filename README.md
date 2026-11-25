# VR_Locomotion_Challenge

An implementation of a locomotion and object interaction technique with VR. Built on Unity with the Meta Quest 3s.

## Setup (this fork)

- Unity 6000.6.6f1, Meta XR All-in-One SDK 209.0.0, OpenXR 1.19.0 (Oculus XR Plugin removed)
- Android: min API 32, target API 34, IL2CPP, ARM64
- Tools > Quest Setup > Use OpenXR for Quest + Link: re-applies the OpenXR feature setup
- Tools > Quest Setup > Build APK: builds to Builds/VRParkour.apk

Based on [VR-locomotion-parkour](https://github.com/teamdarmstadt/VR-locomotion-parkour) (MIT).

## Demo Video

2022 with Object Interaction Task

<a href="http://www.youtube.com/watch?feature=player_embedded&v=ZVDoHTefdR0
" target="_blank"><img src="http://img.youtube.com/vi/ZVDoHTefdR0/0.jpg" 
alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10" /></a>


2021 Version

<a href="http://www.youtube.com/watch?feature=player_embedded&v=5s-vTwTFc7U
" target="_blank"><img src="http://img.youtube.com/vi/5s-vTwTFc7U/0.jpg" 
alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10" /></a>

## How to Start

```{bash}
git clone https://github.com/TU-Darmstadt-HCI-Lab/VR-locomotion-parkour.git
```

- download the GitHub repo and open __VRParkour__ folder as a Unity project
- implement your locomotion technique in `LocomotionTechnique.cs`
- Selection...
- play and see how fast and how many coins you can get!

## Misc

### Core

- Unity 6000.0.60f1 LTS
- Meta XR All-in-One SDK 78.0.0
- Oculus XR Plugin 4.5.2

### Rendering settings

- [ref](https://developer.oculus.com/documentation/unity/unity-conf-settings/#rendering-settings)
- Color space: Linear
- OpenGL ES 3.0
- Multithreaded Rendering

### Configuration settings

- [ref](https://developer.oculus.com/documentation/unity/unity-conf-settings/#configuration-settings)
- Scripting backend: IL2CPP
- Target architectures: ARM64

### Cybersickness reduction

- [Ginger VR](https://github.com/angsamuel/GingerVR) Note: This repo seems not being maintained anymore. Still, it can be a reference for implementing your cybersickness reduction technique.

### Scene

- [Low Poly Ultimate Pack](https://assetstore.unity.com/packages/3d/props/low-poly-ultimate-pack-54733)
- [Forest - Low Poly Toon Battle Arena / Tower Defense Pack](https://assetstore.unity.com/packages/3d/environments/forest-low-poly-toon-battle-arena-tower-defense-pack-100080)

### Sound effect and music

- Winner Winner Funky Chicken Dinner (YouTube Audio Library)
- [tone beep](https://freesound.org/people/pan14/sounds/263133/)
- [crowd yay](https://freesound.org/people/mlteenie/sounds/169233/)

## License

MIT
