using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GameTheory
{

    /*
    ----
    Notes Game Theory
    ----

    ----
    Game Theory
    By: Mark L. Burkey
    Publisher: Business Expert Press
    Pub. Date: February 7, 2013
    Print ISBN-13: 978-1-60649-362-5
    Web ISBN-13: 978-1-60649-363-2
    Pages in Print Edition: 136
    ----
    */

    /*
    ----
    Basic concepts
     - differs in that more than one agent is making choices
     - best choice of A depends on the choice of B and vice versa
     - kinds of games
      - games-of-skill, concern training and talent
      - games-of-chance, concern random outcome
      - games-of-strategy, concern player choice
     - game theory is mostly concerned with games-of-strategy
     - simultaneous game, means the players make choices at the same
        time or in-so-far as one player is not aware of the other 
        players choice.
     - dominant strategy, when a particular choice is always better 
        than all others
     - Nash equilibrium, when neither player would want to change their
        choice based on the choice of the other player "mutual best response"
       - a payoff matrix is the simplist form of input 
       - a payoff matrix may have zero-to-many Nash equilibrium
     - the theory is a mathematical so everything still concerns numbers
    ----
    */

    /*
    ----
    Basic Nash Equilibrium examples
     - The Prisoner's Dilemma
      +--------+----------------------+
      |        |        Clyde         |
      |        +----+--------+--------+
      |        |    | Rat    | Deny   |
      | Bonnie +----+--------+--------+
      |        |Rat |-10,-10 | 0,-20  |
      |        +----+--------+--------+
      |        |Deny| -20,0  | -1,-1  |
      +--------+----+--------+--------+
      - the strategy is explored from one players prespective at-a-time
       - where Clyde answers what they would do knowing the choice made by Bonnie
       - Clyde: "if Bonnie choose Rat then I would choose Rat"
       - Clyde: "if Bonnie choose to Deny then I would choose Rat"
       - the same is true from Bonnie perspective and Rat\Rat is the 
         dominant strategy
       - the crux is that both players don't actually know the other players
         choice until they have made their own.

     - The Game of Chicken
      +--------+----------------------------------+
      |        |                Ren               |
      |        +--------+------------+------------+
      |        |        |Straight    | Swerve     |
      | Chuck  +--------+------------+------------+
      |        |Straight|-100,-100   |  25,-2     |
      |        +--------+------------+------------+
      |        | Swerve |  -2,25     |   0,0      |
      +--------+--------+------------+------------+
      - Chuck: "if Ren choose Straight then my choices would be -100 or -2, I would choose -2, Swerve"
      - Chuck: "if Ren choose Swerve then my choices would be 25 or 0, I would choose 25, Straight"
      - Ren is the same as Chuck, there are two equilibria (-2,25) and (25,-2)

     - Cooperative Coordination Game
      - payoff is greatest when both choose the same 
      +--------+----------------------------------+
      |        |            Hunter 2              |
      |        +--------+------------+------------+
      |        |        |   Stag     |   Hare     |
      |Hunter 1+--------+------------+------------+
      |        |  Stag  |   2,2      |   0,1      |
      |        +--------+------------+------------+
      |        |  Hare  |   1,0      |   1,1      |
      +--------+--------+------------+------------+
      - There is one Stag and two Hares (one Hare near each Hunter)
      - Hunter 1: "if the other Hunter goes for the Stag I would go for it too"
      - Hunter 1: "if the other Hunter goes for their Hare I would go for my Hare"
      - Hunter 2 is the same as Hunter 1, again two equilibria (2,2) and (1,1)

     - Pure Coordination Game
      - payoff only happens when both choose the same
      - drivers must choose which side of the road to drive on
      +--------+----------------------------------+
      |        |            Driver 2              |
      |        +--------+------------+------------+
      |        |        |   Left     |  Right     |
      |Driver 1+--------+------------+------------+
      |        |  Left  |   0,0      | -10,-10    |
      |        +--------+------------+------------+
      |        | Right  | -10,-10    |   0,0      |
      +--------+--------+------------+------------+
      - demo's the extreme importance of coordination

     - Zero-Sum or Fixed-Sum Game
      - a game with a payoff matrix where the players interest are directly opposed
      +--------+----------------------------------+
      |        |         Odds Player              |
      |        +--------+------------+------------+
      |        |        |  Heads     |  Tails     |
      |  Evens +--------+------------+------------+
      | Player | Heads  |   1,-1     |  -1,1      |
      |        +--------+------------+------------+
      |        | Tails  |   -1,1     |   1,-1     |
      +--------+--------+------------+------------+ 
      - each players has a pool of pennies 
      - each player puts down a penny at-the-same-time
      - when laid-down pennies match Evens Player gets both 
        - (their penny back and the Odd's penny)
      - when the laid-down pennies don't match Odds Player gets both
      - there is no dominant strategy
       - each of the four possible outcomes has one loser and one winner
       - the best strategy is to play Heads 50% of the time
      - is called "Zero-Sum" because the sum of the four boxes is 0

     - Zero-Sum with Dominant Strategy
      +--------+----------------------------------+
      |        |          Player B                |
      |        +--------+------------+------------+
      |        |        |   Left     |  Right     |
      |Player A+--------+------------+------------+
      |        |   Up   |   8,-8     |   5,-5     |
      |        +--------+------------+------------+
      |        |  Down  |  -10,10    |  -7,7      |
      +--------+--------+------------+------------+
      - Player A always chooses Up and therefore Player B always chooses Right
       - (5,-5) is the dominant strategy
    ----
    */

    /*
    ----
    Basic Games Revisited
     - Prisoners Dilemma, the incentive is always to choose Rat even if 
       Bonnie knows, for sure, that Clyde will choose Deny
     - the fix to the Prisoners Dilemma is to have a threat-of-force 
       which makes the incentive to Rat less - typically through some kind
       of organization (e.g. drug-cartel shoots those who choose Rat)

     - The Game of Chicken 
      - the fix is called "Tying your hands" where the ability of one 
        of the players to choose is eliminated (e.g. Chuck removes the 
         steering wheel and therefore cannot even choose to Swerve)
      - this makes Ren's payoff as either -100, or -2 only
      - another fix is have the Ren believe in a different payoff matrix
        (e.g. losing the game is worse-than-death for Chuck or Chuck has 
          no fear of death)

     - Zero-Sum Game
      - is overused as in scarcity of resources where the fact that fighting over
        them actually waste resource.
      - has no fix, only attempting to change the rules (e.g. get other player to 
        choose first)
    ----
    */

    /*
    ----
    Iterated Dominance and Dominated Strategies
     - idea of finding dominant strategy, removing any choices which 
       are never choosen, and then repeating 
     - from a matrix point-of-view it means finding whole rows and columns
       which are never choosen
     - in this example the iterations continue until there is only one choice

         ========== 0 ========== 
          | V      | W      | X      | Y      | Z
        A | (5, 0) | (4, 0) | (-2, 0)| (0, 5) | (-1, 1)
        B | (0, 2) | (3, 3) | (3, 4) | (0, 1) | (3, 6)
        C | (3, 2) | (0, 0) | (1, 5) | (5, 0) | (1, 3)
        D | (2, 7) | (-2, 1)| (0, 5) | (2, 2) | (0, 5)
        E | (1, 1) | (2, 5) | (-2, 2)| (-1, 4)| (0, 0)

         ========== 1 ========== 
          | V      | W      | X      | Y      | Z
        A | (5, 0) | (4, 0) | (-2, 0)| (0, 5) | (-1, 1)
        B | (0, 2) | (3, 3) | (3, 4) | (0, 1) | (3, 6)
        C | (3, 2) | (0, 0) | (1, 5) | (5, 0) | (1, 3)

         ========== 2 ========== 
          | X       | Y      | Z
        A | (-2, 0) | (0, 5) | (-1, 1)
        B | (3, 4)  | (0, 1) | (3, 6)
        C | (1, 5)  | (5, 0) | (1, 3)

         ========== 3 ========== 
          | X      | Y      | Z
        B | (3, 4) | (0, 1) | (3, 6)
        C | (1, 5) | (5, 0) | (1, 3)

         ========== 4 ========== 
          | X      | Z
        B | (3, 4) | (3, 6)
        C | (1, 5) | (1, 3)

         ========== 5 ========== 
          | X      | Z
        B | (3, 4) | (3, 6)

         ========== 6 ========== 
          | Z
        B | (3, 6)
     */
    [TestFixture]
    public class TestIteratedDominantStrategy
    {
        double[,] playerA = {
            {5,4,-2,0,-1},
            {0,3,3,0,3},
            {3,0,1,5,1},
            {2,-2,0,2,0},
            {1,2,-2,-1,0}};

        double[,] playerB = {
            {0,0,0,5,1},
            {2,3,4,1,6},
            {2,0,5,0,3},
            {7,1,5,2,5},
            {1,5,2,4,0}};

        [Test]
        public void TestPayoffMatrixToString()
        {
            var testSubject = new PayoffMatrix(playerA, playerB);
            var testResult = testSubject.ToString();
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetPlayerChoices()
        {
            var testSubject = new PayoffMatrix(playerA, playerB);
            var testResult = testSubject.GetPlayerChoices(true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var i in testResult)
                Console.WriteLine(i.ToString(true));
        }

        [Test]
        public void TestGetDominantChoices()
        {
            var testSubject = new PayoffMatrix(playerA, playerB);
            var testInput = testSubject.GetPlayerChoices(true);
            var testResult = testSubject.GetDominantChoices(testInput, true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testSubject.SetItems(testResult);

            testInput = testSubject.GetPlayerChoices(false);
            testResult = testSubject.GetDominantChoices(testInput, false);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testSubject.SetItems(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestGetIteratedDominantStrategy()
        {
            var testSubject = new PayoffMatrix(playerA, playerB);
            var testControl = testSubject.Items;
            Assert.IsNotNull(testControl);
            Assert.AreNotEqual(0, testControl.Count);
            testSubject.GetIteratedDominantStrategy();

            var testResult = testSubject.Items;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Assert.AreNotEqual(testControl.Count, testResult.Count);
            Console.WriteLine(testSubject.ToString());

        }

    }

    public class PayoffMatrixItem
    {
        public PayoffMatrixItem()
        {
            Labels = new Tuple<string, string>(string.Empty, string.Empty);
            Scores = new Tuple<double, double>(double.MinValue, double.MinValue);
        }

        public PayoffMatrixItem(Tuple<string, string> labels, Tuple<double, double> scores)
        {
            Labels = labels;
            Scores = scores;
        }

        public Tuple<string, string> Labels { get; }
        public Tuple<double,double> Scores { get; }

        public override int GetHashCode()
        {
            return Labels?.GetHashCode() ?? 1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PayoffMatrixItem item))
                return false;
            return Equals(item.Labels);
        }

        public bool Equals(Tuple<string, string> labels)
        {
            return Labels.Item1 == labels.Item1 && Labels.Item2 == labels.Item2;
        }

        public override string ToString()
        {
            var str = "{'player-A': {'choice': '" + Labels.Item1 + "', 'score': " + Scores.Item1 + "}, " +
                      "'player-B': {'choice': '" + Labels.Item2 + "', 'score': " + Scores.Item2 + "}}";
            return str;
        }

        public Tuple<string, double> GetValue(bool isPlayerA)
        {
            var score = isPlayerA ? Scores.Item1 : Scores.Item2;
            var label = isPlayerA ? Labels.Item1 : Labels.Item2;

            return new Tuple<string, double>(label,score);
        }

        public string ToString(bool isPlayerA)
        {
            var score = isPlayerA ? Scores.Item1 : Scores.Item2;
            var wouldChoose = isPlayerA ? Labels.Item1 : Labels.Item2;
            var whenTheyChoose = isPlayerA ? Labels.Item2 : Labels.Item1;
            
            var str = "{'when-they-choose': '" + whenTheyChoose + "', 'i-will-choose': '" + wouldChoose + "', 'score': " + score + "}";
            return str;
        }
    }


    public class PayoffMatrix
    {
        private List<PayoffMatrixItem> _items = new List<PayoffMatrixItem>();

        #region ctor
        public PayoffMatrix(Tuple<double[,], string[]> playerA, Tuple<double[,], string[]> playerB)
        {
            Init(playerA, playerB);
        }

        public PayoffMatrix(double[,] playerA, double[,] playerB)
        {
            var playerAChoicesLabels = new List<string>();
            var playerBChoicesLabels = new List<string>();
            for (var i = (byte)'A'; i < (byte)'A' + playerA.GetLongLength(0); i++)
            {
                playerAChoicesLabels.Add(Convert.ToChar(i).ToString());
            }

            for (var i = (byte)'Z' - playerB.GetLongLength(1) + 1; i <= (byte)'Z'; i++)
            {
                playerBChoicesLabels.Add(Convert.ToChar(i).ToString());
            }

            var playerATuple = new Tuple<double[,], string[]>(playerA, playerAChoicesLabels.ToArray());
            var playerBTuple = new Tuple<double[,], string[]>(playerB, playerBChoicesLabels.ToArray());
            Init(playerATuple, playerBTuple);
        }

        private void Init(Tuple<double[,], string[]> playerA, Tuple<double[,], string[]> playerB)
        {
            var pAlst = ConvertToList(playerA.Item1);
            var pBlst = ConvertToList(playerB.Item1);

            var payoffScores = pAlst.Zip(pBlst, (i1, i2) => i1.Zip(i2, (n, v) => new Tuple<double, double>(n, v)))
                .ToList();

            var playerALabels = playerA.Item2;
            var playerBLabels = playerB.Item2;

            PlayerAChoices = playerALabels;
            PlayerBChoices = playerBLabels;

            for (var i = 0; i < payoffScores.Count; i++)
            {
                var row = payoffScores[i].ToList();
                for (var j = 0; j < row.Count; j++)
                {
                    var label = new Tuple<string, string>(playerALabels[i], playerBLabels[j]);
                    var score = row[j];

                    var item = new PayoffMatrixItem(label, score);
                    _items.Add(item);
                }
            }
        }

        #endregion

        #region properties
        internal string[] PlayerAChoices { get; private set; }

        internal string[] PlayerBChoices { get; private set; }

        public List<PayoffMatrixItem> Items => _items;
        #endregion

        internal List<double[]> ConvertToList(double[,] matrix)
        {
            var lst = new List<double[]>();
            if (matrix == null || matrix.GetLongLength(0) == 0)
                return lst;

            for (var i = 0; i < matrix.GetLongLength(0); i++)
            {
                var myInts = new List<double>();
                for (var j = 0; j < matrix.GetLongLength(1); j++)
                {
                    myInts.Add(matrix[i, j]);
                }
                lst.Add(myInts.ToArray());
            }

            return lst;
        }

        public override string ToString()
        {
            var lines = new List<string>();
            var columnLabels = new List<string> {" "};
            columnLabels.AddRange(PlayerBChoices);
            lines.Add(string.Join(" | ", columnLabels));
            
            foreach (var playerAChoice in PlayerAChoices)
            {
                var rowLine = new List<string> {playerAChoice};
                var rowItems = new List<Tuple<double, double>>();
                foreach (var playerBChoice in PlayerBChoices)
                {

                    var match = _items.FirstOrDefault(i =>
                        i.Equals(new Tuple<string, string>(playerAChoice, playerBChoice)));
                    if (match == null)
                        continue;

                    rowItems.Add(match.Scores);
                }

                rowLine.AddRange(rowItems.Select(i => i.ToString()));
                lines.Add(string.Join(" | ", rowLine));
            }

            return string.Join(Environment.NewLine, lines);
        }

        public List<PayoffMatrixItem> GetPlayerChoices(bool isPlayerA)
        {
            var myChoices = isPlayerA ? PlayerAChoices : PlayerBChoices;
            var yourChoices = isPlayerA ? PlayerBChoices : PlayerAChoices;

            var items = new List<PayoffMatrixItem>();

            foreach (var yourChoice in yourChoices)
            {
                PayoffMatrixItem iChoose = new PayoffMatrixItem();
                foreach (var myChoice in myChoices)
                {
                    var playerAChoice = isPlayerA ? myChoice : yourChoice;
                    var playerBChoice = isPlayerA ? yourChoice : myChoice;
                    var match = _items.FirstOrDefault(i =>
                        i.Equals(new Tuple<string, string>(playerAChoice, playerBChoice)));
                    if(match == null)
                        continue;
                    var thisScore = isPlayerA ? match.Scores.Item1 : match.Scores.Item2;
                    var myScore = isPlayerA ? iChoose.Scores.Item1 : iChoose.Scores.Item2;
                    if (thisScore > myScore)
                        iChoose = match;
                }

                items.Add(iChoose);
            }

            return items;
        }

        internal List<PayoffMatrixItem> GetDominantChoices(List<PayoffMatrixItem> playerChoices, bool isPlayerA)
        {
            var uqChoiceLabels = playerChoices.Select(c => c.GetValue(isPlayerA).Item1).Distinct();
            var remainder = new List<PayoffMatrixItem>();
            foreach (var item in _items)
            {
                if(uqChoiceLabels.Contains(item.GetValue(isPlayerA).Item1))
                    remainder.Add(item);
            }

            return remainder;
        }

        internal void SetItems(List<PayoffMatrixItem> items)
        {
            if (items == null || !items.Any())
                return;
            _items = items;
            PlayerAChoices = _items.Select(i => i.Labels.Item1).Distinct().ToArray();
            PlayerBChoices = _items.Select(i => i.Labels.Item2).Distinct().ToArray();
        }

        public void GetIteratedDominantStrategy()
        {
            var i = 0;
            if (_items == null || !_items.Any())
                return;
            while (true)
            {
                var isPlayerA = i % 2 == 0;
                var choices = GetPlayerChoices(isPlayerA);
                var dominant = GetDominantChoices(choices, isPlayerA);

                //where any choices dominated
                if (dominant.Count == _items.Count)
                    break;
                SetItems(dominant);
                i += 1;
            }
        }
    }
}
