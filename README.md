# Pure Hololens 2 Hands
If you're new to developing for the Hololens 2, you should start with the fabulous [MRTK](https://github.com/Microsoft/MixedRealityToolkit-Unity). However, MRTK (which includes over 60,000 lines of code in over 1000 files) feels like extreme overkill if all you really want is your hands.

This project contains 2 files of code, and and three prefabs. "Hands.cs" gets the raw joint data, and maps it to "HandProxyJoint" objects, which are a bunch of transforms available in the scene. My goal was to make this as simple as possible. There's intentinally not even a "show/hide proxies" toggle. 

The hands only work in Unity2018.4 with the platform set to Universal Windows Platform. You can see your hand joints working on builds on a Hololens 2 device, or in the editor through the XR -> Holographic Emulation -> Remote To Device system (which is the system I adore and highly recommend.)
