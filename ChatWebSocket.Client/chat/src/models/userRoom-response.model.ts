import { BaseResponse } from "./base-response.model";
import UserRoom from "./userRoom.model";

export type ConversationFilterResponse = BaseResponse<Array<UserRoom>>;