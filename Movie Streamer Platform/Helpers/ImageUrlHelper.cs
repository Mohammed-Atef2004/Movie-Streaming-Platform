namespace Movie_Streamer_Platform.Helpers
{
    public static class ImageUrlHelper
    {
        private const string Placeholder = "/Images/placeholder.png";

        public static string ForDisplay(string? imageUrl)
        {
            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return uri.AbsoluteUri;
            }

            if (string.IsNullOrWhiteSpace(imageUrl) ||
                !string.Equals(Path.GetFileName(imageUrl), imageUrl, StringComparison.Ordinal))
            {
                return Placeholder;
            }

            return $"/Images/{Uri.EscapeDataString(imageUrl)}";
        }
    }
}
