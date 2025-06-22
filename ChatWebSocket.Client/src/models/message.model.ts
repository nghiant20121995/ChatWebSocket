export default class Message {
    Id: string | undefined;
    SenderId: string | undefined;
    ReceiverId: string | undefined;
    Content: string | undefined;
    IsGroup: boolean = false;
    GroupId: string | undefined;
    CreatedAt: Date | undefined;
    UpdatedAt: Date | undefined;
    RoomId: string | undefined;

    constructor(
        id?: string,
        senderId?: string,
        receiverId?: string,
        content?: string,
        isGroup?: boolean,
        groupId?: string,
        createdAt?: Date,
        updatedAt?: Date
    ) {
        this.Id = id;
        this.SenderId = senderId;
        this.ReceiverId = receiverId;
        this.Content = content;
        this.IsGroup = isGroup || false;
        this.GroupId = groupId;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}