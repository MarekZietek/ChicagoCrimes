using System;
using System.Data;
using System.Data.SqlClient;


namespace Project_6
{
    class Program
    {

        //
        // Connection info for ChicagoCrimes database in Azure SQL:
        //
        static string connectionInfo = String.Format(@"
            Server=tcp:jhummel2.database.windows.net,1433;Initial Catalog=Netflix;
            Persist Security Info=False;User ID=student;Password=cs341!uic;
            MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;
            Connection Timeout=30;  ");


        //-------------------------------------------------------------------------------------
        static void OutputNumMovies()
        {
            SqlConnection db = null;

            try
            {
                //Establish new connection?
                db = new SqlConnection(connectionInfo);

                //Open and start the connection ? 
                db.Open();

                //Paste your sql commands onto here
                string sql = string.Format(@"
                SELECT Count(*) As NumMovies
                FROM Movies; ");

                //System.Console.WriteLine(sql);  // debugging:

                //Gets it ready to execute your sql code 
                SqlCommand cmd = new SqlCommand();

                //I guess connection is established here
                cmd.Connection = db;

                //sql code is ready for execution 
                cmd.CommandText = sql;

                //Executes and the results are stored 
                object result = cmd.ExecuteScalar();

                //Closes everything
                db.Close();

                //Converts to an int
                int numMovies = System.Convert.ToInt32(result);

                //Writes the line with the result 
                System.Console.WriteLine("Number of movies: {0}", numMovies);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("**Error: {0}", ex.Message);
                System.Console.WriteLine();
            }
            finally
            {
                // make sure we close connection no matter what happens:
                if (db != null && db.State != ConnectionState.Closed)
                    db.Close();
            }
        } //End of function



        //--------------------------------------------------------------------------------------------------------
        //Prints the top 10 best movies 
        static void TopTen()
        {

            //New line
            System.Console.WriteLine(" ");

            //Idk ? Set the data coming in to null ? 
            SqlConnection db = null;

            //Open up connection
            db = new SqlConnection(connectionInfo);
            db.Open();

            
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = db;

            string SQL = string.Format(@"
            SELECT TOP 10 Movies.MovieID, COUNT(*) AS NumReviews , ROUND(AVG(CONVERT(float, Rating)),5) AS AvgRating, Movies.MovieName 
            FROM Reviews
            INNER JOIN Movies ON Movies.MovieID = Reviews.MovieID
            GROUP BY Movies.MovieID, Movies.MovieName
            ORDER BY AvgRating DESC, Movies.MovieName ASC");

            //  6th  --  Execute the query 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = SQL;
            adapter.Fill(ds);

            var rows = ds.Tables["TABLE"].Rows;

            Console.WriteLine("Rank\tMovieID\tNumReviews\tAvgRating\tMovieName");
            int rank = 1;

            foreach (DataRow row in rows)
            {

                int id = System.Convert.ToInt32(row["MovieID"]);
                int numReviews = System.Convert.ToInt32(row["NumReviews"]);
                double AvgRating = System.Convert.ToDouble(row["AvgRating"]);
                string MovieName = System.Convert.ToString(row["MovieName"]);

                System.Console.WriteLine("{0}\t{1}\t{2}\t\t{3:0.00000}\t\t'{4}'", rank, id, numReviews, AvgRating, MovieName );

                rank++;
                //int col1 = System.Convert.ToInt32(row["COL1"]);
                //int col2 = System.Convert.ToInt32(row["COL2"]);
                //int col3 = System.Convert.ToInt32(row["COL3"]);
                //int col4 = System.Convert.ToInt32(row["COL4"]);
                //int col5 = System.Convert.ToInt32(row["COL5"]);
                //string code = System.Convert.ToString(row["MovieName"]);
                //System.Console.WriteLine("1 star: {0}", col1);
                //System.Console.WriteLine("2 starts: {0}", col2);

                //\t\t{3:0.00000}\t\t'{4}'

            } // End of foreach

            db.Close();

        } // End of function


        static void string_userInfo( string strID )
        {
            //New line
            System.Console.WriteLine(" ");

            //Idk ? Set the data coming in to null ? 
            SqlConnection db = null;

            strID = strID.Replace("'", "''");

            try
            {
                //Open up connection
                db = new SqlConnection(connectionInfo);
                db.Open();

                //-------------------------------
                string SQL1 = string.Format(@"
                    SELECT UserName FROM dbo.Users
                    WHERE UserName = '{0}'", strID);



                //  1st -- Executes the query 
                SqlCommand First_Cmd = new SqlCommand();
                First_Cmd.Connection = db;
                First_Cmd.CommandText = SQL1;
                object userReviews = First_Cmd.ExecuteScalar();


                string letter = System.Convert.ToString(userReviews);

                if (letter == "")
                {
                    System.Console.WriteLine("**User not found..");
                }
                //Big Else Statement
                else
                {
                    //Print statement 
                    System.Console.WriteLine("{0}", userReviews);

                    //----------------------------------
                    SqlCommand Second_Cmd = new SqlCommand();
                    Second_Cmd.Connection = db;

                    string SQL2 = string.Format(@"
                        SELECT UserID FROM dbo.Users
                        WHERE UserName = '{0}'", strID);

                    // 2nd --  Executes the query 
                    Second_Cmd.CommandText = SQL2;
                    object userID = Second_Cmd.ExecuteScalar();

                    //This  is for the fourth part
                    //int userIdFourthPart = System.Convert.ToInt32( userID);

                    //  2nd -- Print statement 
                    System.Console.WriteLine("User id: {0}", userID);

                    //------------------------------------
                    string SQL3 = string.Format(@"
                        SELECT Occupation FROM dbo.Users
                        WHERE UserName = '{0}'", strID);

                    //  3rd  --  Execute the query 
                    Second_Cmd.CommandText = SQL3;
                    object userOccupation = Second_Cmd.ExecuteScalar();

                    //  3rd -- Print statement 
                    System.Console.WriteLine("Occupaion: {0}", userOccupation);

                    //--------------------------------------
                    //This  number conversion
                    int userIdFourthPart = System.Convert.ToInt32(userID);

                    string SQL4 = string.Format(@"
                        SELECT ROUND( AVG(CONVERT(float, Rating)),5 ) FROM dbo.Reviews
                        WHERE UserID = {0}", userIdFourthPart);

                    //  4rd  --  Execute the query 
                    Second_Cmd.CommandText = SQL4;
                    object userAvg = Second_Cmd.ExecuteScalar();

                    //  4th -- Print statement 
                    System.Console.WriteLine("Avg: {0}", userAvg);

                    //-------------------------------------------------------
                    //Num Reviews : 7382
                    string SQL5 = string.Format(@"
                    SELECT COUNT(*) AS NumReview FROM dbo.Reviews
                    WHERE UserID = {0}", userIdFourthPart);

                    //  5th  --  Execute the query 
                    Second_Cmd.CommandText = SQL5;
                    object userNumReviews = Second_Cmd.ExecuteScalar();

                    //  5th -- Print statement 
                    System.Console.WriteLine("Num reviews: {0}", userNumReviews);

                    //---------------------------------------------------------
                    string SQL6 = string.Format(@"
                        SELECT
                        (SELECT COUNT(Rating) FROM dbo.Reviews 
                        WHERE UserID = {0} AND Rating = 1) AS COL1,
                        (SELECT COUNT(Rating) FROM dbo.Reviews 
                        WHERE UserID = {0} AND Rating = 2)AS COL2,
                        (SELECT COUNT(Rating) FROM dbo.Reviews 
                        WHERE UserID = {0} AND Rating = 3) AS COL3,
                        (SELECT COUNT(Rating) FROM dbo.Reviews 
                        WHERE UserID = {0} AND Rating = 4) AS COL4, 
                        (SELECT COUNT(Rating) FROM dbo.Reviews 
                        WHERE UserID = {0} AND Rating = 5) AS COL5;", userIdFourthPart);

                    //  6th  --  Execute the query 
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = db;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    cmd.CommandText = SQL6;
                    adapter.Fill(ds);

                    var rows = ds.Tables["TABLE"].Rows;

                    foreach (DataRow row in rows)
                    {
                        int col1 = System.Convert.ToInt32(row["COL1"]);
                        int col2 = System.Convert.ToInt32(row["COL2"]);
                        int col3 = System.Convert.ToInt32(row["COL3"]);
                        int col4 = System.Convert.ToInt32(row["COL4"]);
                        int col5 = System.Convert.ToInt32(row["COL5"]);
                        System.Console.WriteLine("1 star: {0}", col1);
                        System.Console.WriteLine("2 starts: {0}", col2);
                        System.Console.WriteLine("3 starts: {0}", col3);
                        System.Console.WriteLine("4 starts: {0}", col4);
                        System.Console.WriteLine("5 starts: {0}", col5);
                        //System.Console.WriteLine("3 starts: {0}", col3);
                    }
                } //End oof else

                db.Close();

            } //End of try

            catch(Exception ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("**Error: {0}", ex.Message);
                System.Console.WriteLine();

            }

            finally
            {
                // make sure we close connection no matter what happens:
                if (db != null && db.State != ConnectionState.Closed)
                    db.Close();
            }

        }// End of Function


        //--------------------------------------------------------------------------------------------------------
        static void userInfo( int intID )
        {

            //New line
            System.Console.WriteLine(" ");

            //Idk ? Set the data coming in to null ? 
            SqlConnection db = null;

            try
            {
                //Open up connection
                db = new SqlConnection(connectionInfo);
                db.Open();

                //-------------------------------------------
                SqlCommand Second_Cmd = new SqlCommand();
                Second_Cmd.Connection = db;

                //This is for the 0 == 0
                string SQL5 = string.Format(@"
                SELECT COUNT(*) AS NumReview FROM dbo.Reviews
                WHERE UserID = {0}", intID);

                //  5th  --  Execute the query 
                Second_Cmd.CommandText = SQL5;
                object userNumReviews = Second_Cmd.ExecuteScalar();

                //Transfer it to an int
                int comparison = System.Convert.ToInt32(userNumReviews);
                //--------------------------------------------

                //See if there was a reviewer
                if (comparison == 0)
                {
                    System.Console.WriteLine("**User not found...");
                }

                //Big Else Statement -- else just print out everything the way it should be 
                else {

                    //-------------------------------
                    string SQL1 = string.Format(@"
                    SELECT UserName FROM dbo.Users
                    WHERE UserID = {0}", intID);

                    //  1st -- Executes the query 
                    SqlCommand First_Cmd = new SqlCommand();
                    First_Cmd.Connection = db;
                    First_Cmd.CommandText = SQL1;
                    object userReviews = First_Cmd.ExecuteScalar();

                    //Print statement 
                    System.Console.WriteLine("{0}", userReviews);
                    //-------------------------------

                    string SQL2 = string.Format(@"
                    SELECT UserID FROM dbo.Users
                    WHERE UserID = {0}", intID);

                    // 2nd --  Executes the query 
                    //SqlCommand Second_Cmd = new SqlCommand();
                    //Second_Cmd.Connection = db;
                    Second_Cmd.CommandText = SQL2;
                    object userID = Second_Cmd.ExecuteScalar();

                    //  2nd -- Print statement 
                    System.Console.WriteLine("User id: {0}", userID);

                    //------------------------------
                    string SQL3 = string.Format(@"
                    SELECT Occupation FROM dbo.Users
                    WHERE UserID = {0}", intID);

                    //  3rd  --  Execute the query 
                    Second_Cmd.CommandText = SQL3;
                    object userOccupation = Second_Cmd.ExecuteScalar();

                    //  3rd -- Print statement 
                    System.Console.WriteLine("Occupaion: {0}", userOccupation);

                    //--------------------------------
                    string SQL4 = string.Format(@"
                    SELECT ROUND( AVG(CONVERT(float, Rating)),5 ) FROM dbo.Reviews
                    WHERE UserID = {0}", intID);

                    //  4rd  --  Execute the query 
                    Second_Cmd.CommandText = SQL4;
                    object userAvg = Second_Cmd.ExecuteScalar();

                    //  4th -- Print statement 
                    System.Console.WriteLine("Avg: {0}", userAvg);

                    //----------------------------------
                    //string SQL5 = string.Format(@"
                    //SELECT COUNT(*) AS NumReview FROM dbo.Reviews
                    //WHERE UserID = {0}", intID);

                    ////  5th  --  Execute the query 
                    //Second_Cmd.CommandText = SQL5;
                    //object userNumReviews = Second_Cmd.ExecuteScalar();

                    //  5th -- Print statement 
                    System.Console.WriteLine("Num reviews: {0}", userNumReviews);

                    //------------------------------------
                    string SQL6 = string.Format(@"
                    SELECT
                    (SELECT COUNT(Rating) FROM dbo.Reviews 
                    WHERE UserID = {0} AND Rating = 1) AS COL1,
                    (SELECT COUNT(Rating) FROM dbo.Reviews 
                    WHERE UserID = {0} AND Rating = 2)AS COL2,
                    (SELECT COUNT(Rating) FROM dbo.Reviews 
                    WHERE UserID = {0} AND Rating = 3) AS COL3,
                    (SELECT COUNT(Rating) FROM dbo.Reviews 
                    WHERE UserID = {0} AND Rating = 4) AS COL4, 
                    (SELECT COUNT(Rating) FROM dbo.Reviews 
                    WHERE UserID = {0} AND Rating = 5) AS COL5;", intID);

                    //  6th  --  Execute the query 
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = db;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    cmd.CommandText = SQL6;
                    adapter.Fill(ds);

                    var rows = ds.Tables["TABLE"].Rows;

                    foreach (DataRow row in rows)
                    {
                        int col1 = System.Convert.ToInt32(row["COL1"]);
                        int col2 = System.Convert.ToInt32(row["COL2"]);
                        int col3 = System.Convert.ToInt32(row["COL3"]);
                        int col4 = System.Convert.ToInt32(row["COL4"]);
                        int col5 = System.Convert.ToInt32(row["COL5"]);
                        System.Console.WriteLine("1 star: {0}", col1);
                        System.Console.WriteLine("2 starts: {0}", col2);
                        System.Console.WriteLine("3 starts: {0}", col3);
                        System.Console.WriteLine("4 starts: {0}", col4);
                        System.Console.WriteLine("5 starts: {0}", col5);
                        //System.Console.WriteLine("3 starts: {0}", col3);
                    }

                    db.Close();
                } //end of else

            }//End of TRY
            catch (Exception ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("**Error: {0}", ex.Message);
                System.Console.WriteLine();
            } //End of CATCH

            finally
            {
                // make sure we close connection no matter what happens:
                if (db != null && db.State != ConnectionState.Closed)
                    db.Close();
            }

        } //End of userInfo 



        //--------------------------------------------------------------------------------------------------------------------------------------
        //Function that takes in the string 
        static void String_MoviesInformation( string strMovie )
        {

            //New line
            System.Console.WriteLine(" ");

            //Idk ? Set the data coming in to null ? 
            SqlConnection db = null;

            strMovie = strMovie.Replace("'", "''");

            try
            {
                //Open up connection
                db = new SqlConnection(connectionInfo);
                db.Open();

                //Set up query
                string SQL = string.Format(@"
                SELECT * FROM dbo.Movies
                WHERE  MovieName  LIKE  '%{0}%'", strMovie );

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                cmd.CommandText = SQL;
                adapter.Fill(ds);

                var rows = ds.Tables["TABLE"].Rows;

                //If there were NO movies found
                if (rows.Count == 0)
                {
                    System.Console.WriteLine("** Movie not found...");
                    return;
                }

                //Print out -- 1  'Shawshank Redemption'    Year:1     
                int id=0;
                foreach (DataRow row in rows)
                {
                    id = System.Convert.ToInt32(row["MovieID"]);
                    string code = System.Convert.ToString(row["MovieName"]);
                    int count = System.Convert.ToInt32(row["MovieYear"]);
                    System.Console.WriteLine("{0}", id);
                    System.Console.WriteLine("'{0}'", code);
                    System.Console.WriteLine("Year: {0}", count);
                }

                //-------------------------------
                string SQL2 = string.Format(@"
                SELECT COUNT(*) AS NumReview FROM dbo.Reviews
                WHERE MovieID = {0}", id );

                //Executes the query 
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = db;
                cmd2.CommandText = SQL2;
                object NumReviews = cmd2.ExecuteScalar();

                //Used for the 0 == 0
                int comparison = System.Convert.ToInt32(NumReviews);

                // if  0  ==  0
                if (comparison == 0)
                {
                    System.Console.WriteLine("Avg review: 0");
                    System.Console.WriteLine("Avg rating: N/A");
                }

                //   Num reviews: 4066     Avg rating: 3.69      
                else
                {
                    //Print statement 
                    System.Console.WriteLine("Num reviews: {0} ", NumReviews);

                    //-------------------------------
                    string SQL3 = string.Format(@"
                SELECT ROUND( AVG(CONVERT(float, Rating)),5 ) FROM dbo.Reviews
                WHERE MovieID={0}", id);

                    //Executes the query 
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = db;
                    cmd3.CommandText = SQL3;
                    object Avg = cmd3.ExecuteScalar();
                    System.Console.WriteLine("Avg rating: {0} ", Avg);

                } // End Of Else 

                db.Close();

            }  // End of try

            catch (Exception ex)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("**Error: {0}", ex.Message);
                System.Console.WriteLine();
            }

            finally
            {
                // make sure we close connection no matter what happens:
                if (db != null && db.State != ConnectionState.Closed)
                    db.Close();
            }

        } //End of function 

        



        //--------------------------------------------------------------------------------------------------------------------------------------
        //Function that displays the movie information 
        static void MoviesInformation( int number )
        {

            //New line
            System.Console.WriteLine(" ");

            //Idk ? Set the data coming in to null ? 
            SqlConnection db = null;

            try
            {
                db = new SqlConnection(connectionInfo);

                db.Open();

                //------------------------------
                string SQL = string.Format(@"
                SELECT * FROM dbo.Movies
                WHERE MovieID = {0}", number );

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                cmd.CommandText = SQL;
                adapter.Fill(ds);

                var rows = ds.Tables["TABLE"].Rows;

                //System.Console.WriteLine(rows.Count);

                if(rows.Count == 0)
                {
                    System.Console.WriteLine("** Movie not found...");
                    return;
                }

                foreach (DataRow row in rows) {
                    int id = System.Convert.ToInt32(row["MovieID"]);
                    string code = System.Convert.ToString(row["MovieName"]);
                    int count = System.Convert.ToInt32(row["MovieYear"]);
                    System.Console.WriteLine("{0}", id);
                    System.Console.WriteLine("'{0}'", code);
                    System.Console.WriteLine("Year: {0}", count);
                }


                //--------------------------------
                string SQL2 = string.Format(@"
                SELECT COUNT(*) AS NumReview FROM dbo.Reviews
                WHERE MovieID = {0}", number );

                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = db;
                cmd2.CommandText = SQL2;
                object NumReviews = cmd2.ExecuteScalar();

                int comparison = System.Convert.ToInt32(NumReviews);

                if ( comparison == 0 ) 
                {
                    System.Console.WriteLine("Avg review: 0");
                    System.Console.WriteLine("Avg rating: N/A");

                }

                else
                {
                    System.Console.WriteLine("Num reviews: {0} ", NumReviews);

                    //-------------------------------
                    string SQL3 = string.Format(@"
                SELECT ROUND( AVG(CONVERT(float, Rating)),5 ) FROM dbo.Reviews
                WHERE MovieID={0}", number);

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = db;
                    cmd3.CommandText = SQL3;
                    object Avg = cmd3.ExecuteScalar();
                    System.Console.WriteLine("Avg rating: {0} ", Avg);
                }

                db.Close();

            }

            catch ( Exception ex )
            {
                System.Console.WriteLine();
                System.Console.WriteLine("**Error: {0}", ex.Message);
                System.Console.WriteLine();
            }

            finally
            {
                // make sure we close connection no matter what happens:
                if (db != null && db.State != ConnectionState.Closed)
                    db.Close();
            }
        } //End of function 




        //---------------------------------------------------------------------------------------------------------------------------
        //Function that prints out directions 
        static string GetUserCommand()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("What would you like?");
            System.Console.WriteLine("m. movie info");
            System.Console.WriteLine("t. top-10 info");
            System.Console.WriteLine("u. user info");
            System.Console.WriteLine("x. exit");
            System.Console.Write(">> ");

            string cmd = System.Console.ReadLine();

            return cmd.ToLower();
        } //End of function 





        //----------------------------------------------------------------------------------------------------------------------
        // Main:
        static void Main(string[] args)
        {

            System.Console.WriteLine("** Netflix Database App **");

            string cmd = GetUserCommand();

            //Main while loop with all the action 
            while (cmd != "x")
            {

                //If statements displaying movies
                if ( cmd == "m")
                {

                    //Start of the movie part
                    System.Console.Write("Enter movieId or part of movie name>> ");

                    //user input
                    string strNumber = System.Console.ReadLine();

                    //string input = System.Console.ReadLine();
                    int number;

                    if ( System.Int32.TryParse(strNumber, out number) )
                    {  
                        //  input  was  an  integer,  id  contains  that  value:
                        MoviesInformation( number );
                    }

                    else {
                        //  input  was  not  an  integer,  input  contains  the  text:
                        String_MoviesInformation( strNumber );
                    }

                } //End of Big If

                else if( cmd == "t")
                {
                    TopTen( );
                }

                //Else If Statement displaying the USER Information
                else if( cmd == "u")
                {
                    //Start of the movie part
                    System.Console.Write("Enter user id or name>> ");

                    //user input
                    string strID = System.Console.ReadLine();

                    //
                    int intID;

                    if (System.Int32.TryParse(strID, out intID))
                    {
                        //  input  was  an  integer,  id  contains  that  value:
                        userInfo( intID );
                    }

                    else
                    {
                        string_userInfo( strID );
                    }

                }

                cmd = GetUserCommand();

            } //End of while loop

            System.Console.WriteLine();
            System.Console.WriteLine("** Done **");
            System.Console.WriteLine();

        } //End of main

    } //End of Class

} //Name space
