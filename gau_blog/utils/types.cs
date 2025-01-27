namespace gau_blog.utils;

public class AuthorResponse
{
    public long Id;
    public string Username;
    public string Role;

    public AuthorResponse(long id, string username, string role)
    {
        Id = id;
        Username = username;
        Role = role;
    }

    public AuthorResponse()
    {
    }
}

public class BlogResponse
{
    public long Id;
    public string Title;
    public string Body;
    public DateTime CreatedAt;
    public int Upvote;
    public int Downvote;
    public AuthorResponse Author;

    public BlogResponse(long id, string title, string body, DateTime createdAt, int upvote, int downvote,
        AuthorResponse author)
    {
        Id = id;
        Title = title;
        Body = body;
        CreatedAt = createdAt;
        Upvote = upvote;
        Downvote = downvote;
        Author = author;
    }

    public BlogResponse()
    {
    }
}