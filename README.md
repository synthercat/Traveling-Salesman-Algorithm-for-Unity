# Traveling-Salesman-Algorithm-for-Unity


## Status : Testing fun project

### Description
This is my own implementation of solving the [Traveling Saleslman Problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem). I thought it would make a good excirsise to see if I could figure out an algorithm that solves this without refering to any known solutions.

### Screenshot
![tsp01](https://user-images.githubusercontent.com/15571710/33559594-fa772aea-d915-11e7-9887-37a4f95417ec.png)
![tsp02](https://user-images.githubusercontent.com/15571710/33559633-17cf62d8-d916-11e7-93a8-a60fa1b235b1.png)

### Usage
Here is what my script does:
* It creates a random board of destinations (Vector2).
* It generates a board on the screen and sets the orthographic camera accordingly as well as the size of the destinations according to how close the closest ones are.
* It goes through combinations of different ways to travel through all destinations.
* It draws the optimal route using a `Line Renderer` found on the home marker.

I also included a complete example in the `TSP.unitypackage` file

### Tip
I do not suggest solving it with more than 10 stops as it will get the app stuck for a bit.

### Note
This is not an optimized solution. It's just something I did for fun (and I did had fun!). It helped me realise how to solve problems with loops that take a long time to finish. Those solutions are not part of that version though, they were implemented on another project of mine.