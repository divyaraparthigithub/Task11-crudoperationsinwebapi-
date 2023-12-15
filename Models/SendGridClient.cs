namespace Task11_crud_.Models
{
    internal class SendGridClient
    {
        private string apiKey;

        public SendGridClient(string apiKey)
        {
            this.apiKey = apiKey;
        }
    }
}