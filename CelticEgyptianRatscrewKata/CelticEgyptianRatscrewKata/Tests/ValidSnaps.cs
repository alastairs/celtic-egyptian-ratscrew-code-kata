﻿using System.Collections.Generic;
using System.Linq;
using CelticEgyptianRatscrewKata.Rules;
using NUnit.Framework;
using Stack=CelticEgyptianRatscrewKata.Cards;

namespace CelticEgyptianRatscrewKata.Tests
{
	class ValidSnapsIncludes
	{
		[TestFixture]
		class AllSnapsShould
		{
			[TestCaseSource("DarkQueenSnaps")]
			[TestCaseSource("StandardSnaps")]
			[TestCaseSource("SandwichSnaps")]
			public void Match_On_Valid_Snaps(Stack stack)
			{
				var sut = CreateSut();

				var snapMatched = sut.IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[TestCaseSource("InvalidSnaps")]
			public void Not_Match_On_InvalidSnaps(Stack stack)
			{
				var sut = CreateSut();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			private static ISnapRule CreateSut()
			{
				return new AllSnaps(
					new StandardSnap(),
					new SandwichSnap(),
					new DarkQueenSnap());
			}

			private static IEnumerable<Stack> DarkQueenSnaps
			{
				get
				{
					yield return new Stack(new List<Card>
					{
						new Card(Suit.Spades, Rank.Queen)
					});
				}
			}

			private static IEnumerable<Stack> StandardSnaps
			{
				get
				{
					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Diamonds, Rank.Ace)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Hearts, Rank.Two),
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Diamonds, Rank.Ace)
					});
				}
			}

			private static IEnumerable<Stack> SandwichSnaps
			{
				get
				{
					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Diamonds, Rank.Ace)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Three),
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Diamonds, Rank.Ace)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Diamonds, Rank.Ace),
						new Card(Suit.Clubs, Rank.Three)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Hearts, Rank.Three),
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Diamonds, Rank.Ace),
						new Card(Suit.Clubs, Rank.Three)
					});
				}
			}

			private static IEnumerable<Stack> InvalidSnaps
			{
				get
				{
					yield return new Stack(Enumerable.Empty<Card>());

					yield return new Stack(new List<Card> { new Card(Suit.Clubs, Rank.Ace) });

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Hearts, Rank.Two)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Diamonds, Rank.Three)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Clubs, Rank.Two),
						new Card(Suit.Clubs, Rank.Three),
						new Card(Suit.Diamonds, Rank.Ace)
					});

					yield return new Stack(new List<Card>
					{
						new Card(Suit.Clubs, Rank.Ace),
						new Card(Suit.Spades, Rank.Queen)
					});
				}
			}
		}

		[TestFixture]
		class StandardSnapShould
		{
			[Test]
			public void Match_Two_Consecutive_Cards_Of_The_Same_Rank()
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
			public void Not_Match_For_A_Single_Card_Stack()
			{
				var stack = new Stack(new List<Card> { new Card(Suit.Clubs, Rank.Ace) });

				var standardSnap = new StandardSnap();
				var theSnap = standardSnap.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Not_Match_Stack_Where_All_Cards_Have_Different_Ranks()
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
			public void Not_Match_Stack_Where_Consecutive_Cards_Have_Different_Ranks()
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

			[Test]
			public void Match_Two_Consecutive_Cards_Not_On_Top_Of_The_Stack()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Hearts, Rank.Two),
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Diamonds, Rank.Ace)
				});
				var standardSnap = new StandardSnap();

				var snapMatched = standardSnap.IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[Test]
			public void Not_Match_On_An_Empty_Stack()
			{
				var stack = new Stack(Enumerable.Empty<Card>());
				var sut = new StandardSnap();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}
		}

		[TestFixture]
		class SandwichSnapShould
		{
			[Test]
			public void Match_Two_Cards_With_The_Same_Rank_Separated_By_A_Card_With_A_Different_Rank()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Diamonds, Rank.Ace)
				});

				var snapMatched = new SandwichSnap().IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[TestCase(2)]
			[TestCase(1)]
			[TestCase(0)]
			public void Not_Match_Stack_With_Less_Than_Three_Cards(int size)
			{
				var cards = new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Diamonds, Rank.Ace)
				};
				var stack = new Stack(cards.Take(size));

				var theSnap = new SandwichSnap().IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Not_Match_Stack_Where_All_Cards_Have_Different_Ranks()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Diamonds, Rank.Three)
				});

				var theSnap = new SandwichSnap().IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Match_Stack_Where_Sandwich_Snap_Is_Not_On_Top()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Three),
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Diamonds, Rank.Ace)
				});

				var snapMatched = new SandwichSnap().IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[Test]
			public void Match_Stack_With_Sandwich_Snap_Not_On_The_Bottom()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Diamonds, Rank.Ace),
					new Card(Suit.Clubs, Rank.Three)
				});

				var snapMatched = new SandwichSnap().IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[Test]
			public void Match_Stack_With_Sandwich_Snap_In_The_Middle()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Hearts, Rank.Three),
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Diamonds, Rank.Ace),
					new Card(Suit.Clubs, Rank.Three)
				});

				var snapMatched = new SandwichSnap().IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[Test]
			public void Not_Match_When_There_Is_More_Than_One_Card_In_The_Middle_Of_The_Sandwich()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					new Card(Suit.Clubs, Rank.Two),
					new Card(Suit.Clubs, Rank.Three),
					new Card(Suit.Diamonds, Rank.Ace)
				});

				var theSnap = new SandwichSnap().IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Not_Match_On_An_Empty_Stack()
			{
				var stack = new Stack(Enumerable.Empty<Card>());
				var sut = new StandardSnap();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}
		}

		[TestFixture]
		class DarkQueenSnapShould
		{
			private readonly Card queenOfSpades = new Card(Suit.Spades, Rank.Queen);

			[Test]
			public void Match_The_Queen_Of_Spades_On_Top_Of_The_Stack()
			{
				var stack = new Stack(new List<Card>
				{
					queenOfSpades
				});
				var sut = CreateSut();

				var snapMatched = sut.IsValidFor(stack);

				Assert.That(snapMatched);
			}

			[Test]
			public void Not_Match_The_Queen_Of_Spades_Not_On_Top_Of_The_Stack()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace),
					queenOfSpades
				});
				var sut = CreateSut();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Only_Match_The_Queen_Of_Spades()
			{
				var stack = new Stack(new List<Card>
				{
					new Card(Suit.Clubs, Rank.Ace)
				});
				var sut = CreateSut();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			[Test]
			public void Not_Match_On_An_Empty_Stack()
			{
				var stack = new Stack(Enumerable.Empty<Card>());
				var sut = CreateSut();

				var theSnap = sut.IsValidFor(stack);

				Assert.That(theSnap, Is.False);
			}

			private static DarkQueenSnap CreateSut()
			{
				return new DarkQueenSnap();
			}
		}
	}
}
