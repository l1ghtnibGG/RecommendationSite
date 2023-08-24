using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace RecommendationSite.Models.Data
{
    public static class SeedData
    {
        public static void EnsureData(IApplicationBuilder app)
        {
            RecommendationDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RecommendationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Vlad",
                        Email = "vlad123@mail.ru",
                        Password = "1234",
                        Status = User.StatusType.Admin,
                        CreatedDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        Score = 4.5f
                    },
                    new User
                    {
                        Name = "Vika",
                        Email = "vika123@mail.ru",
                        Password = "1234",
                        Status = User.StatusType.Admin,
                        CreatedDate = DateTime.Now.AddDays(-5),
                        LastLogin = DateTime.Now,
                        Score = 4.7f
                    },
                   new User
                   {
                       Name = "Petya",
                       Email = "petya@gmail.com",
                       Password = "1234",
                       Status = User.StatusType.User,
                       CreatedDate = DateTime.Now.AddDays(-2),
                       LastLogin = DateTime.Now.AddDays(-1),
                       Score = 4f
                   },
                   new User
                   {
                       Name = "Vova",
                       Email = "vova@gmail.com",
                       Password = "1234",
                       Status = User.StatusType.User,
                       CreatedDate = DateTime.Now,
                       LastLogin = DateTime.Now,
                       Score = 3f
                   },
                   new User
                   {
                       Name = "Masha",
                       Email = "masha12@mail.ru",
                       Password = "1234",
                       Status = User.StatusType.User,
                       CreatedDate = DateTime.Now.AddDays(-2),
                       LastLogin = DateTime.Now,
                       Score = 2.5f
                   },
                   new User
                   {
                       Name = "Nika",
                       Email = "nika3@mail.ru",
                       Password = "1234",
                       Status = User.StatusType.User,
                       CreatedDate = DateTime.Now,
                       LastLogin = DateTime.Now,
                       Score = 3.2f
                   },
                   new User
                   {
                       Name = "Shura",
                       Email = "sasha@mail.ru",
                       Password = "1234",
                       Status = User.StatusType.User,
                       CreatedDate = DateTime.Now.AddDays(-3),
                       LastLogin = DateTime.Now.AddDays(-1),
                       Score = 3.4f
                   }
                );
                context.SaveChanges();
            }

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Tag
                    {
                        Name = "Best"
                    },
                    new Tag
                    {
                        Name = "New"
                    },
                    new Tag
                    {
                        Name = "Worst"
                    },
                    new Tag
                    {
                        Name = "Oldest"
                    }
                );

                context.SaveChanges();
            }

            if (!context.Reviews.Any())
            {
                context.Reviews.AddRange(
                    new Review
                    {
                        Name = "Worth it",
                        Title = "Naruto",
                        Group = Review.GroupType.Movie,
                        Text = "Naruto and Naruto: Shippuden, is probably among " +
                        "the best that Japanese animation has to offer, as the " +
                        "fights, story, music, and characters are all written and " +
                        "choreographed so masterfully. As this show has a decent " +
                        "amount of fillers, because money, it is miles ahead of " +
                        "the lack of cohesiveness in Clone Wars and presents itself " +
                        "as an all-time best TV show when watched without regard " +
                        "for fillers. I will easily rank this as a high 9, likely " +
                        "a 9.1 or 2. That is lower than Avatar: The Last Airbender, " +
                        "but as with Clone Wars, that does not mean I did not enjoy " +
                        "it as much. In this case, I can argue that I liked Naruto " +
                        "more as it was much longer and evoked more emotion out of me " +
                        "somehow. Nonetheless, both shows are amazing, but Naruto " +
                        "takes the cake as my all-time favorite series.",
                        ImageUrl = "~/wwwroot/Images/Naruto.jpg",
                        UserId = Guid.Parse("658DEE78-FF3F-471C-52DC-08DBA4B3E157"),
                        Mark = 10
                    },
                    new Review
                    {
                        Name = "Cool",
                        Title = "Peaky Blinders",
                        Group = Review.GroupType.Movie,
                        Text = "The Peaky Blinders a BBC tv series that started in 2012 " +
                        "and has ended it’s sixth season 2022 has been one of those series " +
                        "that you just want to see more. It is like a good book you cannot " +
                        "put down and you want to keep reading because you can’t get enough" +
                        ".\r\n\r\nI loved this show and it’s characters in it. Each character " +
                        "was unique and different from one another. The writer Steven Knight " +
                        "in my opinion is a genius. It is more than a gangster story, but also" +
                        " about a family with their everyday problems, being gypsy the how " +
                        "badly they were treated, and how even though they were against the " +
                        "law, their ambition was to do better. I would have to write about " +
                        "every character to explain the story, but I am not going to do that. " +
                        "I am going to say that the show has suspense, intrigue, politics, " +
                        "family, relationships, etcetera, etcetera. The Peaky Blinders has " +
                        "everything.\r\n\r\nThomas Shelby who is in charge of the family and " +
                        "family business is some character to watch. Cilian Murphy played " +
                        "this role truthfully and purely. I see no flaws anywhere from anyone. " +
                        "Helen McCroy plays his aunt Polly, I loved her role in the show. " +
                        "The matriarch of the family since the boys did not have their " +
                        "mother or father took care of them. Arthur, Thomas, John, and " +
                        "Finn Shelby, are the brothers who made history in this tv series. T" +
                        "hey all have a tough exterior, but are sensitive at heart. Every a" +
                        "ctor in this show was perfect in my opinion.\r\n\r\nThe show takes p" +
                        "lace in the 1920’s in Birmingham, England. This is where this family " +
                        "takes over the town, but how do they do that? It starts with fixing " +
                        "the horse races and goes into other things later on. The fashion of " +
                        "the time is on point and the surroundings, the homes, enviroment of " +
                        "that time have been made to look like that era. An industrial city " +
                        "Birmingham, you see the poor side of town and the rich side.\r\n\r\nIn " +
                        "conclusion of my review, the Peaky Blinders if it were a movie should get " +
                        "an Oscar and every actor and actress deserve one as well. I loved it. So, " +
                        "by order of the Peaky Blinders, go watch the show everybody. Enjoy!",
                        ImageUrl = "~/wwwroot/Images/PeakyBlinders.jpg",
                        UserId = Guid.Parse("9A1DB068-B3D1-4420-52DD-08DBA4B3E157"),
                        Mark = 9
                    },
                    new Review
                    {
                        Name = "Overrate",
                        Title = "The art of war",
                        Group = Review.GroupType.Book,
                        Text = "“The Art of War,” written by the ancient Chinese military strategist " +
                        "Sun Tzu, is a timeless masterpiece that continues to captivate readers with " +
                        "its profound wisdom and strategic brilliance. This extraordinary work, despite" +
                        " its origins in warfare, transcends its historical context, offering invaluable " +
                        "insights into the art of successful living, be it on the battlefield or in our" +
                        " daily endeavors. Here, I present a positive review of this enduring classic.\r\n\r\n" +
                        "First and foremost, “The Art of War” stands as a testament to the enduring nature" +
                        " of human conflict and our unwavering desire for knowledge to navigate these challenges. " +
                        "The fact that this book has remained relevant for over two millennia attests to its " +
                        "universal applicability and the enduring appeal of its principles. Sun Tzu’s emphasis" +
                        " on the importance of understanding the enemy, identifying weaknesses, and leveraging " +
                        "strengths not only applies to military affairs but can also be adapted to business," +
                        " politics, and interpersonal relationships.\r\n\r\nOne of the most remarkable aspects " +
                        "of this book is its emphasis on the importance of diplomacy and peaceful resolution " +
                        "over brute force. Sun Tzu’s philosophy is rooted in the idea that the greatest victory" +
                        " lies in avoiding conflict altogether. By advocating for clever strategizing and diplomacy," +
                        " he encourages readers to seek alternatives to violent confrontation, ultimately fostering" +
                        " a world where harmony and mutual understanding prevail.\r\n\r\nFurthermore, “The Art of War”" +
                        " is not merely a collection of tactical maneuvers but also a profound exploration of leadership" +
                        " and self-awareness. Sun Tzu emphasizes the importance of understanding oneself, one’s " +
                        "strengths, weaknesses, and biases, which are crucial aspects of effective leadership in " +
                        "any domain. By embracing these principles, leaders can make informed decisions that " +
                        "benefit both their followers and the organization they serve.\r\n\r\nThe book’s elegant " +
                        "prose and concise aphorisms make it an enjoyable and accessible read. Its brevity ensures " +
                        "that its messages remain crystal clear, and its wisdom can be easily revisited and " +
                        "contemplated time and time again. Sun Tzu’s words are like a roadmap to success, guiding" +
                        " readers through the complexities of life and offering solutions to the various challenges" +
                        " they may encounter.\r\n\r\nFinally, “The Art of War” fosters a sense of respect for " +
                        "strategy and intellectual acumen. It challenges readers to think critically and strategically," +
                        " enabling them to approach problems with a heightened sense of awareness and creativity." +
                        " It celebrates intelligence and innovation as paramount assets, regardless of the arena in" +
                        " which they are employed.\r\n\r\nIn conclusion, “The Art of War” is not just a book; it is " +
                        "a treasure trove of timeless wisdom that continues to inspire and enlighten readers across" +
                        " generations. Its profound insights, elegant prose, and adaptability to various aspects of" +
                        " life make it a must-read for anyone seeking personal growth, a deeper understanding of s" +
                        "trategy, and the pursuit of excellence in all endeavors. By embracing Sun Tzu’s teachings, w" +
                        "e can become better leaders, decision-makers, and ultimately, better human beings.",
                        ImageUrl = "~/wwwroot/Images/TheArtOfWar.jpg",
                        UserId = Guid.Parse("DAE8CAA0-E4D2-4B46-52DE-08DBA4B3E157"),
                        Mark = 6,
                    },
                    new Review
                    {
                        Name = "Masterpiece",
                        Title = "Call of Duty: Modern Warfare 2",
                        Group = Review.GroupType.Game,
                        Text = "Call of Duty: Modern Warfare II (2009) came at a time when first-person shooters still" +
                        " needed a foundation. Call of Duty: Modern Warfare II (2022) drops into a saturated era for the" +
                        " genre and struggles to raise the bar Infinity Ward set for themselves. There are plenty of reasons" +
                        " here for the franchise to pause on annual releases and wind up with a higher quality package" +
                        ".\r\n\r\nEven with this all said, the new Call of Duty: Modern Warfare II still fills the void " +
                        "with a modern take of the iconic series. It still checks the boxes for an annual entertainment " +
                        "package with a story and online multiplayer. But it sticks to what it knows to stand out as a " +
                        "household name for gamers. At the end of each multiplayer session and campaign replay, I still" +
                        " warmed up to the idea of Infinity Ward channelling their old habits for better or worse.",
                        ImageUrl = "~/wwwroot/Images/COD.jpg",
                        UserId = Guid.Parse("658DEE78-FF3F-471C-52DC-08DBA4B3E157"),
                        Mark = 10
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
