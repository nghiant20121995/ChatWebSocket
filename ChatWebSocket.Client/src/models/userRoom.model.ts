import BaseModel from "./base.model";
import Room from "./room.model";
import User from "./user.model";

export default class UserRoom extends BaseModel
{
    UserId!: string;
    Room!: Room;
    Members?: Array<User>;
    Partner?: User;
    RoomName?: string;
    Avatar?: string;
    FirstLettersofRoomName?: string;
}