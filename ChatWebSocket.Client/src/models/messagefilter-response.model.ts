import { BaseResponse } from "./base-response.model";
import Message from "./message.model";

export type MessageFilterResponse = BaseResponse<Array<Message>>;