namespace SwipSwapMarketplace.Auth
{
    /// <summary>
    /// Represents configuration values for JSON Web Token (JWT) authentication.
    /// These values are loaded from <c>appsettings.json</c> and define how tokens
    /// are created, validated, and expired.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key used to sign and validate JWT tokens. 
        /// Must be kept private and secure.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Identifies the token issuer (the system generating the token).
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Identifies the intended audience (the system or client consuming the token).
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token lifetime in minutes before expiration.
        /// </summary>
        public int ExpireMinutes { get; set; }
    }
}
