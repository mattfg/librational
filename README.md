wait is someone actually reading this? oh, it's probably me in the future. hi. how's it going? just as well personally and a lot better globally, I hope. here in october 2020 things are. well, you know.

you're probably here because you want to know about this code, so I'll stop wasting your time.

this library is my own shitty little approximation of Racket's excellent Exact type. I tested it manually but haven't gotten around to a proper test project yet... Probably never will.

if you download just this library on its own it's probably gonna complain about a missing libexcept dependency. I wrote libexcept too, but at this point the only thing you need from it is `ImpossibleException`, which is defined as nothing more than:

    public class ImpossibleException: Exception
    {
    }

that issue aside, here's what's in this code.

BigRational is the main point of entry. This is the one you're supposed to use. I recommend aliasing it to Rat or BigRat (https://stackoverflow.com/questions/9257989/defining-type-aliases), A because it's fun and B to save on typing. The only reason everyone uses `int` everywhere is it's so easy to type.
BigRat has a numerator and a denominator, which are both BigIntegers. It should be reasonably self-explanatory, at least the important parts.

UnsafeBigRational is there for when you're doing a lot of operations in sequence and you don't care about mutability or reduced fractions. I.e., you should not use these across threads and you probably shouldn't pass them to other methods.
When you're done with your computations, call .Normalize to convert it into a BigRational. Doing so will create a BigRational and also mutate the UnsafeBigRational you called .Normalize on.
"Why isn't there a _normalized flag to prevent it from doing unnecessary work?" because it still has to return a BigRational, and calling the constructor is where the work happens. Either way you're calling the constructor, so the work is still happening.
"Why not cache the BigRational result and return that instead of newing up a new one?" Hm, that's not a bad idea. I don't have a good answer. Cause I'm lazy?
Update: I did it

IComparable2 is a utility. It probably could have gone in its own libcomparable or something, but maybe I'll do that later. It just define sthe comparison operators for you as long as you implement IComparable.

IEnumerableExtensions adds 2 extension methods to IEnumerable, allowing you to sum BigRationals

IRational is an internal interface because the two things that extend it, BigRational and UnsafeBigRational, are meant to be the *only* two things that extend it. It provides a lot of basic functionality common to both BigRational and UnsafeBigRational. You know, looking at it again in retrospect, I'm not sure this needed to be an interface. I never take it in as a param anywhere, I don't need to be able to treat BigRational and UnsafeBigRational uniformly. Did I just do this to save on typing? It would improve performance to just copy paste this into the two classes that implement. Maybe I'll do that.
If IRational doesn't exist at the time you read this, it's because I did that and you no longer need to worry about it. But if I didn't:
Some constants are defined here. They're not used yet, but it makes sense to have them, I think. Well, maybe not. Infinity is not a rational number. Maybe I'll remove them. Whatever you do, don't just use them as "invalid value" special case markers. That's how Hoare made his billion dollar mistake. Don't be a lazy hack and misuse these. I mean, I'm a lazy hack, but that's no excuse for you to be too.
Oh, we've also got Ln here, the natural log. To get another log in base B, just do `rat.Ln() / ln(B)`.

RationalOperators implements the basic operators. I think I put these in their own file because they're pretty much the same for both BigRat and UnsafeBigRat, so if you update one you want to update them both, so it's convenient to have them in the same file.
Also, it reduces scrolling.
Nothing much to see here, these are defined algebraically: ms's BigInteger is doing the heavy lifting, which means a smarter programmer than me can probably find a brilliant way to optimize these (John Carmack HMU).
