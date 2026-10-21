namespace Meridian.Api.Dto
{
	public class FileResponse
	{
		public bool Success { get; set; }
		public int? ImportId { get; set; }
		public string Message { get; set; } = null!;

		public static FileResponse Ok(int importId, string message) =>
			new() { Success = true, ImportId = importId, Message = message };

		public static FileResponse Fail(string message) =>
			new() { Success = false, Message = message };
	}
}