using CoinFlipper.SwissArmy.Infrastructure.Repositories.Models;

namespace CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;

public static class Seed
{
    public static SentenceDb[] Sentences()
    {
        return new[]
        {
            new SentenceDb { Id = 1, Sentence = "If you don’t believe it or don’t get it, I don’t have the time to try to convince you, sorry.", Author = "Satoshi Nakamoto" },
            new SentenceDb { Id = 2, Sentence = "Scams always pump the hardest in a risk-on environment.", Author = "" },
            new SentenceDb { Id = 3, Sentence = "A clear mind is a foundation of a good trade.", Author = "" },
            new SentenceDb { Id = 4, Sentence = "Knowledge opens a lot of doors.", Author = "" },
            new SentenceDb { Id = 5, Sentence = "Little of something is greater than all of nothing.", Author = "" },
            new SentenceDb { Id = 6, Sentence = "Learn at your first bull run, retire at second.", Author = "" },
            new SentenceDb { Id = 7, Sentence = "How you do anything is how you do everything.", Author = "T. Harv Eker" },
            new SentenceDb { Id = 8, Sentence = "What you don't use you lose.", Author = "" },
            new SentenceDb { Id = 9, Sentence = "All good things come to those who wait.", Author = "" },
            new SentenceDb { Id = 10, Sentence = "The bull walks up the stairs and the bear jumps out the window.", Author = "" },
            
            new SentenceDb { Id = 11, Sentence = "Done is better than perfect.", Author = "Sheryl Sandberg" },
            new SentenceDb { Id = 12, Sentence = "The secret to change is to focus all of your energy not on fighting the old, but building the new.", Author = "Socrates" },
            new SentenceDb { Id = 13, Sentence = "Man becomes slave of his own repeated actions.", Author = "" },
            new SentenceDb { Id = 14, Sentence = "An investment in knowledge pays the best interest.", Author = "Benjamin Franklin" },
            new SentenceDb { Id = 15, Sentence = "With a good perspective on history, we can have a better understanding of the past and present, and thus a clear vision of the future.", Author = "Carlos Slim Helu" },
            new SentenceDb { Id = 16, Sentence = "The biggest risk of all is not taking one.", Author = "Mellody Hobson" },
            new SentenceDb { Id = 17, Sentence = "Know what you own, and know why you own it.", Author = "Peter Lynch" },
            new SentenceDb { Id = 18, Sentence = "Wide diversification is only required when investors do not understand what they are doing.", Author = "Warren Buffett" },
            new SentenceDb { Id = 19, Sentence = "The most contrarian thing of all is not to oppose the crowd but to think for yourself.", Author = "Peter Thiel" },
            new SentenceDb { Id = 20, Sentence = "When you invest, you are buying a day that you don’t have to work.", Author = "Aya Laraya" },
            
            new SentenceDb { Id = 21, Sentence = "Formal education will make you a living; self-education will make you a fortune.", Author = "Jim Rohn" },
            new SentenceDb { Id = 22, Sentence = "Investing puts money to work. The only reason to save money is to invest it.", Author = "Grant Cardone" },
            new SentenceDb { Id = 23, Sentence = "Do not put all your eggs in one basket.", Author = "" },
            new SentenceDb { Id = 24, Sentence = "Courage is note the lack of fear. It is acting in spite of it.", Author = "Mark Twain" },
            new SentenceDb { Id = 25, Sentence = "Do more of what works and less of what doesn’t.", Author = "Steve Clark" },
            new SentenceDb { Id = 26, Sentence = "Risk comes from not knowing what you are doing.", Author = "Warren Buffett" },
            new SentenceDb { Id = 27, Sentence = "If you cannot control your emotions, you can’t control your money.", Author = "Warren Buffett" },
            new SentenceDb { Id = 28, Sentence = "Learn to take losses. The most important thing in making money is not letting your losses get out of hand.", Author = "Marty Schwartz" },
            new SentenceDb { Id = 29, Sentence = "Always check correlations to see bigger picture.", Author = "" },
            new SentenceDb { Id = 30, Sentence = "Price is never \"too high\" or \"too low\".", Author = "" },
            
            new SentenceDb { Id = 31, Sentence = "Ping-Pong till you wrong.", Author = "" },
        };
    }
}