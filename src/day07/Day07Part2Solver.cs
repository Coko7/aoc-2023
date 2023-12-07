namespace aoc_2023.src.day07
{
    public class Day07Part2Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/7

        public override int Day => 7;
        public override int Part => 2;

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

            Console.WriteLine($"Result for part 2 is {sumWinnings}");
        }

        private static int CompareCombinations(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            string aVal = a.Key;
            string bVal = b.Key;
            return CompareHands(aVal, bVal);
        }

        private static int CompareHands(string a, string b)
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

        private static int GetHandType(string hand)
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

        private static int CompareCards(char a, char b)
        {
            int aVal = GetCardValue(a);
            int bVal = GetCardValue(b);

            if (aVal > bVal) return 1;
            if (aVal < bVal) return -1;
            return 0;
        }

        private static int GetCardValue(char card)
        {
            if (char.IsDigit(card)) return card - 48;

            if (card == 'T') return 10;
            if (card == 'Q') return 12;
            if (card == 'K') return 13;
            if (card == 'A') return 14;

            if (card == 'J') return 1;

            throw new Exception("Invalid card label");
        }
    }
}
