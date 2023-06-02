# Pierre's Sweet and Savory Treats
#### A web application with user authentication and a many-to-many relationship. The app should have the following features:
* User Authentication - All users have access to read functionality, but only logged in users can create, update, or delete.
* A many-to-many relationship between ```Treat``` models and ```Flavor``` models. Many treats can be many different flavors.
* A splash page with a list of each kind of treat and flavor, with links to see all the treats/flavors belonging to a single category.

#### By Thomas McDowell  

## Technologies Used:
* C#
* .NET 6.0
* MySql

## Description:
Long Description / Mission Statement / What the app do.  

## Setup/Installation Req's:

### Set Up and Run Project
1. Clone this repo.
2. Open the terminal and navigate to this project's production directory called "BracketTracker". 
3. Within the production directory "BracketTracker", create a new file called `appsettings.json`.
4. Within `appsettings.json`, put in the following code, replacing the `uid` and `pwd` values with your own username and password for MySQL. For the LearnHowToProgram.com lessons, we always assume the `uid` is `root` and the `pwd` is `epicodus`.

```json
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Port=3306;database=bracket_tracker;uid=[YOUR SQL USERNAME];pwd=[YOUR SQL PASSWORD];"
  }
}
```

5. Set up the Database. In your terminal in the project directory (BracketTracker.Solution/BracketTracker), run ```dotnet ef database update```

6. Run ```dotnet watch run``` to view the project in your web browser. Enter your computer password when prompted.





(Additional Instructions about how the app do.)

## Known Bugs:
Any known bugs here

## License:
Git your license, yo!

## One More Thing:
Don't forget to get a picture of your database relations layout with [SQL Designer](https://ondras.zarovi.cz/sql/demo/). Review as needed with [this epicodus lesson](https://www.learnhowtoprogram.com/c-and-net/database-basics/using-sql-designer)

Testing