namespace Blog.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool HasAlreadyBeenUsed => UsedAt != null;

        public bool IsRevoked => RevokedAt != null;
    }
}
