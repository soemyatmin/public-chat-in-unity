# PublicChatInUnity
A simple chat function with firebase realtime database and anonymous authentication.

This project in using Unity version 2019.4.0f1
screen ratio => 9:16

Before you use this project: 
1. Create a firebase project 
2. Add the google-services.json via (project->setting->app)
3. Allow anonymous login 
![Anonymous](https://user-images.githubusercontent.com/17348039/87237394-bb138080-c41b-11ea-943a-10f6dbf2cb8b.JPG)

4. Create a firebase realtime database 
![RealtimeDatabase](https://user-images.githubusercontent.com/17348039/87237395-c49ce880-c41b-11ea-966e-d94fecc91092.JPG)

the rule can be 
```
{
  "rules": {
      ".write": "auth.uid != null"
      ".read": "auth.uid != null"
    }
}
```
or all public. 

Flow Chart for Anonymous login 
![FlowchartDiagram1](https://user-images.githubusercontent.com/17348039/87237216-928a8700-c419-11ea-92d4-d22bc806ec97.png)


I use SimpleJSON and JSON .NET For Unity to easier to manage data.
SimpleJSON => https://wiki.unity3d.com/index.php/SimpleJSON

JSON.NET For Unity => https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347
