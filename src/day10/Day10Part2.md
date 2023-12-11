# My *convoluted* solution for Day 10 Part 2

For this tenth day of the Advent of Code 2023, things start getting harder.
Part 1 was not that hard, though harder than some of the previous challenges.
The real challenge of the day was part 2.
I took some time to think about the problem and tried to see how I could solve it but I haven't found an algorithm that works so far.
But after drawing on some paper and trying to think hard about the problem, I have realized that maybe I could find the solution without writing any code.

This does feel weird though since I had managed to solve all previous problems using a generic algorithm.
I would like to spend more time on this problem and come up with an algorithm that gives the solution for any input but since I did not want to spend the entire day on this, I resorted to a different approach instead.

## The Explanation

1. Find all the pipes that are part of the loop (done in part 1)
2. Print the entire map to the console so that:
	- Start pipe is displayed as 'S'
	- Ground is displayed as '.' (a dot)
	- Pipes that are not part of the loop are not printed
	- Pipes that are part of the loop are printed but using a specific unicode symbol:

		| Input | Render |
		| ----- | ------ |
		|   -   |   ═    |
		|   \|  |   ║    |
		|   7   |   ╗    |
		|   L   |   ╚    |
		|   F   |   ╔    |
		|   J   |   ╝    |
		
3. We now have a nice-looking representation of our map where all visible pipes are closely connected
4. Take a screenshot of the entire output and open it in Gimp
5. Use `Fuzzy Select` tool to chroma out what is outside of the pipes
6. Use `Bucket Fill` tool to erase all pipes. Now all that's left are the dots (ground) contained within the closed area
7. Then use `Select` > `By Color` and click on a dot. This will select all the dots in the image
8. Open the histogram `Windows` > `Dockable Dialogs` > `Histogram`
9. Write down the number of pixels
10. Divide by 9 because a single dot was made up of nine pixels in my screenshot
11. Submit the result for part 2

This is not an elegant solution to the problem since I had to resort to external tools that did all the work (Gimp).
But I have to say that finding the answer using this convoluted approach felt pretty good on the moment :)
