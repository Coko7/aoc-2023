namespace aoc_2023.src.day07
{
    public class Day07Solver : AocSolver
    {
        // Link to the puzzle: https://adventofcode.com/2023/day/7

        public override int Day => 7;
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

        public override void SolvePart2()
        {
            string input = ReadInput("example");
            string[] lines = ReadInputLines("example");

            Console.WriteLine($"Result for part 2 is ");
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
    }
}
