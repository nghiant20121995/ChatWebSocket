export default class MessageRequest
{
    SenderId!: string;
    ReceiverId!: string;
    Content!: string;
    IsGroup!: boolean;
    CreatedDate!: Date;
}