namespace Blog.Auth;

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

    public string? TryUse(string jwtId)
    {
        if (IsExpired)
            return "Refresh token has expired.";

        if (HasAlreadyBeenUsed)
            return "Refresh token has already been used.";

        if (IsRevoked)
            return "Refresh token has been revoked.";

        if (JwtId != jwtId)
            return "Access and refresh tokens do not matches.";

        UsedAt = DateTime.UtcNow;

        return null;
    }

    public void Revoke()
    {
        RevokedAt = DateTime.Now;
    }
}
