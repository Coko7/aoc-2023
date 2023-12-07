namespace aoc_2023.src.day07
{
    public class Day07Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/7

        public override int Day => 7;

        #region Part 1

        // ===============================================
        //                      PART 1
        // ===============================================

        public override void Solve()
        {
            var list = new List<KeyValuePair<string, int>>();
            string[] lines = ReadInputLines("input");
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(' ');
                string hand = parts[0];
                int bid = int.Parse(parts[1]);

                var kvp = new KeyValuePair<string, int>(hand, bid);
                list.Add(kvp);
                list.Sort(CompareCombinations);
            }

            int rank = 1;
            int sumWinnings = 0;
            foreach (var kvp in list)
            {
                int winnings = rank * kvp.Value;
                sumWinnings += winnings;
                Console.WriteLine($"{rank++} {kvp.Key} {kvp.Value} => {winnings}");
            }

            Console.WriteLine($"Result is {sumWinnings}");
        }

        public static int CompareCombinations(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            string aVal = a.Key;
            string bVal = b.Key;
            return CompareHands(aVal, bVal);
        }

        public static int CompareHands(string a, string b)
        {
            int aVal = GetHandType(a);
            int bVal = GetHandType(b);

            if (aVal > bVal) return 1;
            if (aVal < bVal) return -1;

            for (int i = 0; i < a.Length; i++)
            {
                int cardCmp = CompareCards(a[i], b[i]);
                if (cardCmp != 0) return cardCmp;
            }

            return 0;
        }

        public static int GetHandType(string hand)
        {
            if (hand.Distinct().Count() == 1) return 100; // Five of a kind
            if (hand.Distinct().Count() == 5) return 1; // High card

            var distinctCards = hand.Distinct().ToArray();
            int[] freqs = new int[distinctCards.Length];

            foreach (char c in hand)
            {
                for (int i = 0; i < distinctCards.Length; i++)
                {
                    if (distinctCards[i] == c) freqs[i]++;
                }
            }

            if (freqs.Length == 2 && freqs.Max() == 4) return 90; // Four of a kind
            if (freqs.Length == 2 && freqs.Max() == 3) return 80; // Full house
            if (freqs.Length == 3 && freqs.Max() == 3) return 70; // Three of a kind
            if (freqs.Length == 4 && freqs.Max() == 2) return 10; // One pair

            return 20; // Two pair
        }

        public static int CompareCards(char a, char b)
        {
            int aVal = GetCardValue(a);
            int bVal = GetCardValue(b);

            if (aVal > bVal) return 1;
            if (aVal < bVal) return -1;
            return 0;
        }

        public static int GetCardValue(char card)
        {
            if (char.IsDigit(card)) return card - 48;

            if (card == 'T') return 10;
            if (card == 'J') return 11;
            if (card == 'Q') return 12;
            if (card == 'K') return 13;
            if (card == 'A') return 14;

            throw new Exception("Invalid card label");
        }

        #endregion

        #region Part 2

        // ===============================================
        //                      PART 2
        // ===============================================

        public override void SolvePart2()
        {
            var list = new List<KeyValuePair<string, int>>();
            string[] lines = ReadInputLines("input");
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(' ');
                string hand = parts[0];
                int bid = int.Parse(parts[1]);

                var kvp = new KeyValuePair<string, int>(hand, bid);
                list.Add(kvp);
                list.Sort(CompareCombinationsPart2);
            }

            int rank = 1;
            int sumWinnings = 0;
            foreach (var kvp in list)
            {
                int winnings = rank * kvp.Value;
                sumWinnings += winnings;
                Console.WriteLine($"{rank++} {kvp.Key} {kvp.Value} => {winnings}");
            }

            Console.WriteLine($"Result for part 2 is {sumWinnings}");
        }

        public static int CompareCombinationsPart2(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            string aVal = a.Key;
            string bVal = b.Key;
            return CompareHandsPart2(aVal, bVal);
        }

        public static int CompareHandsPart2(string a, string b)
        {
            int aVal = GetHandTypePart2(a);
            int bVal = GetHandTypePart2(b);

            if (aVal > bVal) return 1;
            if (aVal < bVal) return -1;

            for (int i = 0; i < a.Length; i++)
            {
                int cardCmp = CompareCardsPart2(a[i], b[i]);
                if (cardCmp != 0) return cardCmp;
            }

            return 0;
        }


        public static int GetHandTypePart2(string hand)
        {
            var freqDict = new Dictionary<char, int>();
            for (int i = 0; i < hand.Length; i++)
            {
                char card = hand[i];
                if (card == 'J') continue;

                if (freqDict.ContainsKey(card))
                {
                    freqDict[card]++;
                }
                else
                {
                    freqDict.Add(card, 1);
                }
            }

            char leadingCard;
            if (freqDict.Any())
            {
                leadingCard = freqDict.MaxBy(kvp => kvp.Value).Key;
                int jokerCount = hand.Count(card => card == 'J');
                freqDict[leadingCard] += jokerCount;
            }
            else
            {
                leadingCard = 'J';
                freqDict.Add('J', 5);
            }

            if (freqDict.Count == 1) return 100; // Five of a kind
            if (freqDict.Count == 2 && freqDict[leadingCard] == 4) return 90; // Four of a kind
            if (freqDict.Count == 2 && freqDict[leadingCard] == 3) return 80; // Full house
            if (freqDict.Count == 3 && freqDict[leadingCard] == 3) return 70; // Three of a kind
            if (freqDict.Count == 4 && freqDict[leadingCard] == 2) return 10; // One pair
            if (freqDict.Count == 5) return 1; // High card

            return 20; // Two pair
        }

        public static int CompareCardsPart2(char a, char b)
        {
            int aVal = GetCardValuePart2(a);
            int bVal = GetCardValuePart2(b);

            if (aVal > bVal) return 1;
            if (aVal < bVal) return -1;
            return 0;
        }

        public static int GetCardValuePart2(char card)
        {
            if (char.IsDigit(card)) return card - 48;

            if (card == 'T') return 10;
            if (card == 'Q') return 12;
            if (card == 'K') return 13;
            if (card == 'A') return 14;

            if (card == 'J') return 1;

            throw new Exception("Invalid card label");
        }

        #endregion
    }
}
