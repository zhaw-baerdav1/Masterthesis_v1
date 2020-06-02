# Masterthesis_v1

----------------------------------------------
Required setup before checking out the project
----------------------------------------------
- Unity 2019.2.12f
- Git Installed
- Git LFS Installed (https://git-lfs.github.com/)
- Path variables for Git defined in environment variables (--> %USERPROFILE%\AppData\Local\GitHubUnity\git\cmd\) and computer restarted

----------------------------------------------
Steps
----------------------------------------------
1. Open CMD
2. Execute: "git clone https://github.com/zhaw-baerdav1/Masterthesis_v1.git %YOUR_LOCAL_DIRECTORY_OF_PROJECT%"
3. Wailt until cloning is done and close CMD
4. Open project in Unity (takes around 10 minutes for Unity to read all new files)

%YOUR_LOCAL_DIRECTORY_OF_PROJECT% = Either create an empty folder or create a new project in Unity and replace the (default) project data

----------------------------------------------
Known Issues
----------------------------------------------
1. 
Issue: The editor script for the CC assets doesn't work properly when setting up a new project.
Solution: Revert changes in Git-Window.

2.
Issue: The editor script for trees seem to corrupt the materials and the prefabs referring to it.
Solution: Work in progress.
