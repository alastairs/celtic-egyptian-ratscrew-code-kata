﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CelticEgyptianRatscrewKata.Tests
{
	class ValidSnapsIncludes
	{
		[TestFixture]
		class StandardSnap
		{
			[Test]
			public void Should_Match_Two_Consecutive_Cards_Of_The_Same_Rank()
			{
				var cards = new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Diamonds, Rank.Ace)
				};
				var stack = new Stack(cards);

				var standardSnap = new StandardSnap();
				var matches = standardSnap.IsValidFor(stack);

				Assert.That(matches);
			}

			[Test]
			public void Should_Not_Match_For_A_Single_Card_Stack()
			{
				var stack = new Stack(new List<Card> { new Card(Suit.Clubs, Rank.Ace) });

				var standardSnap = new StandardSnap();
				var theSnap = standardSnap.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Should_Not_Match_Stack_Where_All_Cards_Have_Different_Ranks()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Hearts, Rank.Two)
				});

				var standardSnap = new StandardSnap();
				var theSnap = standardSnap.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Should_Not_Match_Stack_Where_Consecutive_Cards_Have_Different_Ranks()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Hearts, Rank.Two),
					new Card(Suit.Diamonds, Rank.Ace)
				});
				var standardSnap = new StandardSnap();

				var theSnap = standardSnap.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			private bool IsValidFor(Stack stack)
			{
				var stackFrame = stack.Take(2);

				return stack.Count() != 1 && stackFrame.First().HasSameRankAs(stackFrame.Last());
			}
		}
	}
}
