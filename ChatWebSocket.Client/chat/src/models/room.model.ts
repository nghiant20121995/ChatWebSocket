import BaseModel from "./base.model";
import MessageRequest from "./request/message.request";

export default class Room extends BaseModel
{
    RoomName!: string;
    LatestMessage?: MessageRequest;
    IsGroup!: boolean;
}