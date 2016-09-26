# webConsole

Python console that runs in the server but is controlled from a web client.

# build instructions

Loading in [webConsole.csproj](./webConsole/webConsole.csproj) VSTO 2012 or greater w/[NTVS](https://www.visualstudio.com/vs/node-js/) will automatically install javascript dependencies. If you are not using VSTO or NTVS you'll have to install them manually:
````
npm install
````

Install [browserify](http://browserify.org/) if you haven't already:
````
npm install -g browserify
````

Compile web client from `/webConsole/APP`:
````
browserify app.js --debug | exorcist ./output/bundle.js.map > ./output/bundle.js
````
Run from visual studio. Access console at [http://localhost:65220/Console]().
