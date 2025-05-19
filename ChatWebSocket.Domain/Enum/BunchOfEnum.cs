using System;
using System.Collections.Generic;
using System.Text;

namespace ChatWebSocket.Domain.Enum
{
    public enum WebSocketBaseMessageType
    {
        Message,
        Notification,
        UserStatus,
        GroupMessage,
        FileUpload,
        FileDownload,
        ImageUpload,
        ImageDownload,
        VideoUpload,
        VideoDownload,
        AudioUpload,
        AudioDownload
    }
}
