namespace TelefonRehberi.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }// RequestId: isteğe özel benzersiz kimlik

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);// ShowRequestId: ID boş değilse ekranda göster
}

