using System;

namespace SocialMediaAdapterDemo
{
    // ===== 1. Єдиний клієнтський інтерфейс =====
    public interface ISocialMediaClient
    {
        void Publish(string message);
    }

    // ===== 2. Сторонні API соцмереж (імітація, не можна змінювати) =====

    // Facebook API
    public class FacebookApi
    {
        public void SendPost(string text)
        {
            Console.WriteLine($"[Facebook] Publishing: {text}");
        }
    }

    // Instagram API
    public class InstagramApi
    {
        public void PublishStory(string text)
        {
            Console.WriteLine($"[Instagram] Story posted: {text}");
        }
    }

    // Twitter API
    public class TwitterApi
    {
        public void Tweet(string text)
        {
            Console.WriteLine($"[Twitter] Tweeted: {text}");
        }
    }

    // ===== 3. Адаптери (з інжекцією залежностей) =====

    public class FacebookAdapter : ISocialMediaClient
    {
        private readonly FacebookApi facebookApi;

        public FacebookAdapter(FacebookApi api)
        {
            facebookApi = api ?? throw new ArgumentNullException(nameof(api));
        }

        public void Publish(string message)
        {
            Validate(message);
            facebookApi.SendPost(message);
        }

        private void Validate(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.");
        }
    }

    public class InstagramAdapter : ISocialMediaClient
    {
        private readonly InstagramApi instagramApi;

        public InstagramAdapter(InstagramApi api)
        {
            instagramApi = api ?? throw new ArgumentNullException(nameof(api));
        }

        public void Publish(string message)
        {
            Validate(message);
            instagramApi.PublishStory(message);
        }

        private void Validate(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.");
        }
    }

    public class TwitterAdapter : ISocialMediaClient
    {
        private readonly TwitterApi twitterApi;

        public TwitterAdapter(TwitterApi api)
        {
            twitterApi = api ?? throw new ArgumentNullException(nameof(api));
        }

        public void Publish(string message)
        {
            Validate(message);
            twitterApi.Tweet(message);
        }

        private void Validate(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.");
        }
    }

    // ===== 4. Client =====

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ISocialMediaClient facebook = new FacebookAdapter(new FacebookApi());
                ISocialMediaClient instagram = new InstagramAdapter(new InstagramApi());
                ISocialMediaClient twitter = new TwitterAdapter(new TwitterApi());

                Console.WriteLine("=== Publishing to Facebook ===");
                facebook.Publish("Hello from Adapter Pattern!");

                Console.WriteLine("\n=== Publishing to Instagram ===");
                instagram.Publish("Adapter Pattern Story!");

                Console.WriteLine("\n=== Publishing to Twitter ===");
                twitter.Publish("Adapter pattern works!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
