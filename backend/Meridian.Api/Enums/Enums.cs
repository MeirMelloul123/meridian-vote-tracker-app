namespace Meridian.Api.Enums
{
    /// <summary>
    /// How a state voted: Yes, No, Abstain or Absent.
    /// </summary>
    public enum Ballot
    {
        Y,  // Yes
        N,  // No
        A,  // Abstain
        X   // Absent
    }

    /// <summary>
    /// Processing state of an uploaded file.
    /// </summary>
    public enum ImportStatus
    {
        Pending,
        Processing,
        Completed,
        CompletedWithIssues,
        Failed
    }

    /// <summary>
    /// Role of a user: analysts import and review, admins also set positions and publish.
    /// </summary>
    public enum UserRole
    {
        Analyst,
        Admin
    }
    /// <summary>
    /// What happened to one row of an imported file.
    /// </summary>
    public enum RowStatus
    {
        New,            // stored, did not exist before
        Unchanged,      // same as the approved value
        Duplicate,      // same row twice in the file
        Invalid,        // bad data (unknown country, not a member...)
        Contradiction,  // same country twice in a vote with different values
        Conflict        // differs from the approved value, sent to review
    }
}